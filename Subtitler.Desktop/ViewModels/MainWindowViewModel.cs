using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

		private string _file;
		public string File
		{
			get { return _file; }
		}

		private IDataService _dataService;
		private ISettings _settings;
		private IOService _ioService;
		private int _downloadFinishedCount = 0;
		private int _totalDownloadCount = 0;
		

		#endregion

		#region Commands and delegates
		public ICommand DoNothingCommand { get { return new RelayCommand(OnDoNothing, CanExecuteDoNothing); } }
		public RelayCommand<object> Download { get { return new RelayCommand<object>(OnDownloadCommand); } }
		public RelayCommand OpenSettings { get { return new RelayCommand(OnShowSettings);} }
		public RelayCommand ReloadData { get { return new RelayCommand(OnLoadData); } }
		public RelayCommand OpenFile {get {return new RelayCommand(OnOpenFile);}}
		#endregion

		#region ctor

		public bool HasInternet { get; set; }
		public MainWindowViewModel(IDataService dataService, ISettings settings, IOService ioService)
		{
			_dataService = dataService;
			_settings = settings;
			_ioService = ioService;
			_file = ArgumentsHelper.ParseArguments(Environment.GetCommandLineArgs(), "file");

			_dataService.LogIn();

			HasInternet = _dataService.CanConnect;

			if (HasInternet)
			{
				LoadDataAsync(_file);
			}
			else
				DisplayNoConnectionErrorMessage();
		}

		#endregion

		#region Command Handlers

		private void OnOpenFile()
		{
			_file = _ioService.OpenFileDialog("");
			LoadDataAsync(_file);
		}

		private void OnDownloadCommand(object selectedSubtitles)
		{
			var file = Movie.ParseFile(_file);
			var subtitles = ((System.Collections.IList)selectedSubtitles).Cast<Subtitle>().ToList();
			_totalDownloadCount = subtitles.Count;

			if (_totalDownloadCount > 1)
			{
				foreach (Subtitle subtitle in subtitles)
				{
					subtitle.DownloadAsync(file.Directory, ResolveFinalFileName(subtitle.SubFileName, file.Name, false), _settings.ShouldUnzipFile,
						callback:
							() =>
							{
								_downloadFinishedCount++;
								if (_downloadFinishedCount == _totalDownloadCount)
								{
									ShowMessage("", "All subtitles downloaded");
								}
							});
				}
			}
			else
			{
				var subtitle = subtitles.Single();
				subtitle.DownloadAsync(file.Directory, ResolveFinalFileName(subtitle.SubFileName, file.Name, _settings.ShouldRenameFile), _settings.ShouldUnzipFile,
					callback: () => ShowMessage("", "Download finished."));
			}
		}

		private void OnDoNothing()
		{
		}

		private void OnLoadData()
		{
			LoadDataAsync(_file);
		}

		private void OnShowSettings()
		{
			IsSettingsFlyoutOpened = !IsSettingsFlyoutOpened;
		}

		private bool CanExecuteDoNothing()
		{
			return false;
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

		private void LoadDataAsync(string file)
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
				foreach (Subtitle subtitle in subtitles)
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
			ShowMessage("No internet connection.", "Warning!");
		}

		#endregion

	}
}