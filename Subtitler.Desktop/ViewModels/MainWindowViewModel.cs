using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro;
using MahApps.Metro.Controls;
using Microsoft.Practices.ServiceLocation;
using Subtitler.Desktop.DAL;
using Subtitler.Desktop.Helpers;
using Subtitler.Desktop.Models;
using Subtitler.Lib.Helpers;
using Subtitler.Lib.OpenSubtitles;

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
		private int _downloadFinishedCount = 0;
		private int _totalDownloadCount = 0;

		#endregion

		#region Commands and delegates
		public RelayCommand<object> Download { get { return new RelayCommand<object>(OnDownload); } }
		public RelayCommand OpenSettings { get { return new RelayCommand(OnShowSettings);} }
		public RelayCommand ReloadData { get { return new RelayCommand(OnLoadData, CanExecuteLoadData); } }
		public RelayCommand OpenFile {get {return new RelayCommand(OnOpenFile);}}
		public RelayCommand<DragEventArgs> DropFile { get { return new RelayCommand<DragEventArgs>(OnDropFile); } }
		#endregion

		#region ctor
		
		public MainWindowViewModel(IDataService dataService, ISettings settings, IOService ioService)
		{
			_dataService = dataService;
			_settings = settings;
			_ioService = ioService;

			var filePath = ArgumentsHelper.ParseArguments(Environment.GetCommandLineArgs(), "file");
			_movie = Movie.FromFile(filePath);

			_dataService.LogIn();

			if (_dataService.CanConnect)
			{
				GetSubtitlesAsync(Movie.FullPath);
			}
			else
				DisplayNoConnectionErrorMessage();
		}

		#endregion

		#region Command Handlers

		private void OnDropFile(DragEventArgs e)
		{
			var files = FileHelper.GetFilesFromDropEvent(e);

			var fileInfo = new FileInfo(files[0]); 
			if (FileHelper.IsFileTypeAllowed(fileInfo.Extension, App.AllowedExtensions))
			{
				GetSubtitlesAsync(fileInfo.FullName);
			}
			else
			{
				SubtitlesCollection.Clear();
				ShowMessage("Warning!",string.Format("Filetype {0} not allowed.", fileInfo.Extension));
			}
		}

		private void OnOpenFile()
		{
			var pathFromDialog = _ioService.OpenFileDialog("");
			Movie = Movie.FromFile(pathFromDialog);
			GetSubtitlesAsync(Movie.FullPath);
		}

		private void OnDownload(object selectedSubtitles)
		{
			var subtitles = ((System.Collections.IList)selectedSubtitles).Cast<Subtitle>().ToList();
			_totalDownloadCount = subtitles.Count;

			if (_totalDownloadCount > 1)
			{
				foreach (Subtitle subtitle in subtitles)
				{
					subtitle.DownloadAsync(Movie.Directory, ResolveFinalFileName(subtitle.SubFileName, Movie.NameWithoutExt, false), _settings.ShouldUnzipFile,
						callback:
							() =>
							{
								_downloadFinishedCount++;
								if (_downloadFinishedCount == _totalDownloadCount)
								{
									ShowMessage("Info", "All subtitles downloaded");
								}
							});
				}
			}
			else
			{
				var subtitle = subtitles.Single();
				subtitle.DownloadAsync(Movie.Directory, ResolveFinalFileName(subtitle.SubFileName, Movie.NameWithoutExt, _settings.ShouldRenameFile), _settings.ShouldUnzipFile,
					callback: () => ShowMessage("Info", "Download finished."));
			}
		}

		private void OnLoadData()
		{
			if (Movie.IsNull)
			{
				ShowMessage("Warning!","No movie selected. Please use drag'n'drop or Open file dialog.");
			}
			else
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

		private string ResolveFinalFileName(string subFileName, string movieFileName, bool rename)
		{
			if (rename)
				return movieFileName;
			else
				return FileHelper.StripExtension(subFileName);
		}

		private void GetSubtitlesAsync(string file)
		{			
			IsLoading = true;

			using (var asyncExecutor = new AsyncExecutor())
			{
				asyncExecutor.ExecuteTask(GetSubtitles, file);
				asyncExecutor.OnExecutionComplete += PopulateSubtitleCollection;
			}
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