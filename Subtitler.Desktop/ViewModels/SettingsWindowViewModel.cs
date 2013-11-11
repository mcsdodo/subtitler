using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using Subtitler.Desktop.Helpers;
using Subtitler.Desktop.Models;
using Subtitler.Lib.Helpers;

namespace Subtitler.Desktop.ViewModels
{
	public class SettingsWindowViewModel : SubtitlerViewModelBase
	{

		#region Properties

		private Settings _settings;

		public bool ShouldRenameFile
		{
			get { return _settings.ShouldRenameFile; }
			set
			{
				if (_settings.ShouldRenameFile != value)
				{
					_settings.ShouldRenameFile = value;
					//RaisePropertyChanged(() => ShouldRenameFile);
				}
			}
		}

		public bool ShouldUnzipFile
		{
			get { return _settings.ShouldUnzipFile; }
			set
			{
				if (_settings.ShouldUnzipFile != value)
				{
					_settings.ShouldUnzipFile = value;
					//RaisePropertyChanged(() => ShouldUnzipFile);
				}
			}
		}

		private LanguageCollection _languages;
		public LanguageCollection Languages
		{
			get
			{
				if (_languages == null || _languages.Count == 0)
					_languages = _settings.Languages;
				return _languages;
			}
			set
			{
				//if (_settings.Languages != value)
				//{
					_settings.Languages = value;
					//RaisePropertyChanged(() => Languages);
				//}
			}
		}
		#endregion

		public SettingsWindowViewModel()
		{
			_settings = new Settings();

			Languages.OnCollectionSave += sender =>
			{
				_settings.Languages = sender as LanguageCollection;
			};
			
		}

		
	}
}
