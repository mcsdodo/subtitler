using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using Subtitler.Desktop.DAL;
using Subtitler.Desktop.Models;
using Subtitler.Lib.Helpers;

namespace Subtitler.Desktop.ViewModels
{
	public class MainWindowViewModel : SubtitlerViewModelBase
	{
		#region Properties
		private ObservableCollection<Subtitle> _subtitlesCollection = new ObservableCollection<Subtitle>();
		public ObservableCollection<Subtitle> SubtitlesCollection
		{
			get { return _subtitlesCollection; }
			set
			{
				if (_subtitlesCollection != value)
				{
					_subtitlesCollection = value;
					RaisePropertyChanged(() => SubtitlesCollection);
				}
			}
		}

		private bool _isSettingsFlyoutOpened;
		public bool IsSettingsFlyoutOpened
		{
			get { return _isSettingsFlyoutOpened; }
			set
			{
				if (_isSettingsFlyoutOpened != value)
				{
					_isSettingsFlyoutOpened = value;
					RaisePropertyChanged(() => IsSettingsFlyoutOpened);
				}
			}
		}

		private bool _isLoading;
		public bool IsLoading
		{
			get { return _isLoading; }
			set
			{
				if (_isLoading != value)
				{
					_isLoading = value;
					RaisePropertyChanged(() => IsLoading);
					RaisePropertyChanged(() => EmptyTemplateString);
				}
			}
		}

		public string EmptyTemplateString
		{
			get
			{
				if (IsLoading)
				{
					return "Loading ...";
				}
				else
				{
					return "No subtitles found ...";
				}
			}
		}

		private Movie _movie;
		public Movie Movie
		{
			get { return _movie ?? Movie.NullMovie(); }
			set { _movie = value; }
		}


		private IDataService _dataService;
		private ISettings _settings;
		private IOService _ioService;
		private IDownloadHelper _downloadHelper;
		private IFileHelper _fileHelper;

		#endregion

		#region Commands and delegates
		public RelayCommand<object> Download { get { return new RelayCommand<object>(OnDownload); } }
		public RelayCommand OpenSettings { get { return new RelayCommand(OnShowSettings); } }
		public RelayCommand ReloadData { get { return new RelayCommand(OnReloadData, CanExecuteLoadData); } }
		public RelayCommand OpenFile {get {return new RelayCommand(OnOpenFile);}}
		public RelayCommand<DragEventArgs> DropFile { get { return new RelayCommand<DragEventArgs>(OnDropFile); } }
		#endregion

		#region ctor
		
		public MainWindowViewModel(IDataService dataService, ISettings settings, IOService ioService, IDownloadHelper downloadHelper, IFileHelper fileHelper)
		{
			_dataService = dataService;

			_settings = settings;
			_ioService = ioService;
			_downloadHelper = downloadHelper;
			_fileHelper = fileHelper;
		}

		#endregion

		#region Command Handlers

		private void OnDropFile(DragEventArgs e)
		{
			var files = FileHelper.GetFilesFromDropEvent(e);
			ProcessFileAndGetSubtitles(files[0]);
		}

		private void OnOpenFile()
		{
			var pathFromDialog = _ioService.OpenFileDialog("");
			if (string.IsNullOrEmpty(pathFromDialog))
			{
				return;
			}
			ProcessFileAndGetSubtitles(pathFromDialog);
		}

		private void ProcessFileAndGetSubtitles(string filePath)
		{
			var fileInfo = new FileInfo(filePath);
			if (FileHelper.IsFileTypeAllowed(fileInfo.Extension, App.AllowedExtensions))
			{
				Movie = Movie.FromFile(filePath);
				GetSubtitlesAsync(Movie.FullPath);
			}
			else
			{
				SubtitlesCollection.Clear();
				ShowMessage("Warning!", string.Format("Filetype {0} not allowed.", fileInfo.Extension));
			}
		}

		private void OnDownload(object selectedSubtitles)
		{
			var subtitles = ((System.Collections.IList)selectedSubtitles).Cast<Subtitle>().ToList();
			var totalDownloadCount = subtitles.Count;
			var downloadFinishedCount = 0;

			Action<string> renameAndUnzipAction = pathToArchive => { };
			if (_settings.ShouldUnzipFile)
			{
				renameAndUnzipAction =
					pathToArchive =>
					_fileHelper.ExtractArchive(pathToArchive, Movie.Directory, _settings.ShouldRenameFile ? Movie.NameWithoutExt : "");
			}

			Action<string> downloadCallBack;
			if (totalDownloadCount > 1)
			{
				downloadCallBack = pathToFile =>
				{
					downloadFinishedCount++;
					if (downloadFinishedCount == totalDownloadCount)
					{
						renameAndUnzipAction(pathToFile);
						ShowMessage("Info", "All subtitles downloaded");
					}
				};
			}
			else
			{
				downloadCallBack = pathToFile => { renameAndUnzipAction(pathToFile); ShowMessage("Info", "Download finished."); };
			}

			foreach (Subtitle subtitle in subtitles)
			{
				_downloadHelper.DownloadFileAsync(subtitle.ZipDownloadLink, Movie.Directory, subtitle.SubFileName, downloadCallBack);
			}
		}

		private void OnReloadData()
		{
			GetSubtitlesAsync(Movie.FullPath);
		}

		private void OnShowSettings()
		{
			IsSettingsFlyoutOpened = !IsSettingsFlyoutOpened;
		}

		private bool CanExecuteLoadData()
		{
			return !Movie.IsNull;
		}

		#endregion

		#region Other methods

		private void GetSubtitlesAsync(string file)
		{			
			IsLoading = true;

			Task.Run(() =>
				{
					var subtitles = GetSubtitles(file);
					Application.Current.Dispatcher.Invoke(() => PopulateSubtitleCollection(subtitles));
				});
		}

		private List<Subtitle> GetSubtitles(string file)
		{
			var langs = _settings.Languages.Where(l => l.Use).Select(l => l.Id).ToArray();
			return _dataService.GetSubtitles(file, langs);
		}

		private void PopulateSubtitleCollection(object parameters)
		{
			var subtitles = parameters as List<Subtitle>;
			
			if (subtitles.Count > 0)
			{
				SubtitlesCollection.Clear();
				foreach (var subtitle in subtitles)
				{
					SubtitlesCollection.Add(subtitle);
				}
			}
			else
			{
				SubtitlesCollection.Clear();
			}
			
			IsLoading = false;
		}

		private void DisplayNoConnectionErrorMessage()
		{
			SubtitlesCollection.Clear();
			ShowMessage("Warning!", "No internet connection.");
		}

		#endregion

	}
}