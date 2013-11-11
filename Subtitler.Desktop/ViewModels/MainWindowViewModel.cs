using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro;
using MahApps.Metro.Controls;
using Subtitler.Desktop.DAL;
using Subtitler.Desktop.Helpers;
using Subtitler.Desktop.Models;
using Subtitler.Lib.Helpers;

namespace Subtitler.Desktop.ViewModels
{


	public class MainWindowViewModel : SubtitlerViewModelBase, INeedInternet
	{
		#region Properties

		private ObservableCollection<Subtitle> _subtitlesCollection;
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


		public bool ShouldRenameFile
		{
			get { return (bool)Properties.Settings.Default["RenameFileAsSource"]; }
		}

		public bool ShouldUnzipFile
		{
			get { return (bool)Properties.Settings.Default["UnzipFile"]; }
		}
		private IDataService _dataService;
		private int _downloadFinishedCount = 0;
		private int _totalDownloadCount = 0;
		#endregion

		#region Commands
		public ICommand DoNothingCommand { get { return new RelayCommand(OnDoNothing, CanExecuteDoNothing); } }
		public RelayCommand<object> Download { get { return new RelayCommand<object>(OnDownloadCommand); } }
		public RelayCommand OpenSettings { get { return new RelayCommand(OnShowSettings);} }
		#endregion

		#region ctor
		public bool HasInternet { get; set; }
		public MainWindowViewModel(IDataService dataService)
		{
			_dataService = dataService;
			HasInternet = _dataService.CanConnect;

			if (IsInDesignMode || IsInDesignModeStatic)
			{
				LoadData();
			}
			else
			{
				if (HasInternet)
				{
					LoadData();
				}
				else
					DisplayNoConnectionErrorMessage();
			}
			
		}

		#endregion

		#region Command Handlers

		private void OnDownloadCommand(object param)
		{
			var file = MovieFile.ParseFile(App.TestFile);
			var selectedItems = (System.Collections.IList)param;
			List<Subtitle> subtitles = selectedItems.Cast<Subtitle>().ToList();
			_totalDownloadCount = subtitles.Count;

			if (_totalDownloadCount > 1)
			{
				foreach (Subtitle subtitle in subtitles)
				{
					subtitle.Download(file.Directory, subtitle.SubFileName, ShouldUnzipFile, IncreaseCountOfFinishedDownloads);
				}
			}
			else
			{
				var subtitle = subtitles.Single();
				subtitle.Download(file.Directory, ResolveFinalFileName(subtitle.SubFileName, file.Name), ShouldUnzipFile,
					() => ShowMessage("", "Download finished."));
			}

			
			
		}

		private void OnDoNothing()
		{
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

		private string ResolveFinalFileName(string subFileName, string movieFileName)
		{
			if (ShouldRenameFile)
				return movieFileName;
			else
				return FileHelper.StripExtension(subFileName);
		}

		private void LoadData()
		{
			_dataService.LogIn();
			SubtitlesCollection = new ObservableCollection<Subtitle>();
			//var languages = new List<string>() { "ar", "bg", "cs", "ca", "da", "de", "en", "es", "et", "fa", "fi", "fr",  "el", "he", "hr", "hu", "id", "is", "it", "ja", "ka", "ko",  "mk", "nl", "no", "pb", "pl", "pt", "ro", "ru", "sk", "sl", "sq", "sr", "sv", "th", "tr", "uk", "vi", "zh", "gl", "ms", "oc", "si", "tl", "uk", "eu", "hi", "km" };

			var subtitles = _dataService.GetSubtitles(App.TestFile, new[] { "slo", "cze", "eng", "pol", "spa", "rus" });

			foreach (Subtitle subtitle in subtitles)
			{
				SubtitlesCollection.Add(subtitle);
			}
			_dataService.LogOut();
		}

		private void IncreaseCountOfFinishedDownloads()
		{
			_downloadFinishedCount++;
			if (_downloadFinishedCount == _totalDownloadCount)
			{
				ShowMessage("", "All subtitles downloaded");
			}
			
		}

		private void DisplayNoConnectionErrorMessage()
		{
			MessageBox.Show("No internet connection.", "Subtitler warning!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}

		#endregion

	}
}