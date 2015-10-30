using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
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
		private IDataService _dataService;
		private ISettings _settings;
		private IOpenFile _openFile;
		private IDownloadHelper _downloadHelper;
		private IFileHelper _fileHelper;

		#endregion

		#region Commands and delegates
		public RelayCommand<object> Download { get { return new RelayCommand<object>(OnDownload); } }
		public RelayCommand OpenSettings { get { return new RelayCommand(OnShowSettings); } }
		public RelayCommand ReloadData { get { return new RelayCommand(OnReloadData, HasMovie); } }
		public RelayCommand OpenFile {get {return new RelayCommand(OnOpenFile);}}
		public RelayCommand<DragEventArgs> DropFile { get { return new RelayCommand<DragEventArgs>(OnDropFile); } }
		#endregion

		#region ctor
		
		public MainWindowViewModel(IDataService dataService, ISettings settings, IOpenFile openFile, IDownloadHelper downloadHelper, IFileHelper fileHelper)
		{
			_dataService = dataService;

			_settings = settings;
			_openFile = openFile;
			_downloadHelper = downloadHelper;
			_fileHelper = fileHelper;

			if (IsInDesignModeStatic)
			{
				GetSubtitles("");	
			}
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
			var pathFromDialog = _openFile.OpenFileDialog();
			if (!string.IsNullOrEmpty(pathFromDialog))
			{
				ProcessFileAndGetSubtitles(pathFromDialog);
			}
		}

		private void OnDownload(object selectedSubtitles)
		{
			var subtitles = ((IList)selectedSubtitles).Cast<Subtitle>().ToList();
			var downloadCallBack = GetDownloadCallback(subtitles.Count);

			foreach (Subtitle subtitle in subtitles)
			{
				_downloadHelper.DownloadFileAsync(subtitle.ZipDownloadLink, _movie.Directory, subtitle.SubFileName, downloadCallBack);
			}
		}

		private void OnReloadData()
		{
			GetSubtitles(_movie.FullPath);
		}

		private void OnShowSettings()
		{
			IsSettingsFlyoutOpened = !IsSettingsFlyoutOpened;
		}

		public bool HasMovie()
		{
			return _movie != null;
		}

		#endregion

		#region Misc methods

		private void ProcessFileAndGetSubtitles(string filePath)
		{
			var fileInfo = new FileInfo(filePath);
			if (FileHelper.IsFileTypeAllowed(fileInfo.Extension, App.AllowedExtensions) && fileInfo.Exists)
			{
				_movie = new Movie(fileInfo);
				GetSubtitles(_movie.FullPath);
			}
			else
			{
				SubtitlesCollection.Clear();
				ShowMessage("Warning!", string.Format("Filetype {0} not allowed.", fileInfo.Extension));
			}
		}

		private Action<string> GetDownloadCallback(int totalDownloadCount)
		{
			var downloadFinishedCount = 0;

			Action<string> downloadCallBack = pathToFile =>
			{
				if (++downloadFinishedCount == totalDownloadCount)
				{
					if (_settings.ShouldUnzipFile)
					{
						_fileHelper.ExtractArchive(pathToFile, _movie.Directory, _settings.ShouldRenameFile ? _movie.NameWithoutExt : "");
					}
					ShowMessage("Info", totalDownloadCount > 1 ? "All subtitles downloaded" : "Download finished.");
				}
			};

			return downloadCallBack;
		}

		private void GetSubtitles(string file)
		{			
			IsLoading = true;

			Task.Run(() =>
				{
					var langs = _settings.Languages.Where(l => l.Use).Select(l => l.Id).ToArray();
					var subtitles = _dataService.GetSubtitles(file, langs);
					Application.Current.Dispatcher.Invoke(() => PopulateSubtitleCollection(subtitles));
				});
		}

		private void PopulateSubtitleCollection(IEnumerable<Subtitle> subtitles)
		{
			SubtitlesCollection.Clear();
			foreach (var subtitle in subtitles)
			{
				SubtitlesCollection.Add(subtitle);
			}
			IsLoading = false;
		}
		#endregion

	}
}