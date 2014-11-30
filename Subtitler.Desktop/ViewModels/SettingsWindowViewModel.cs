using GalaSoft.MvvmLight.Command;
using Subtitler.Desktop.Models;

namespace Subtitler.Desktop.ViewModels
{
	public class SettingsWindowViewModel : SubtitlerViewModelBase
	{

		#region Properties

		private ISettings _settings;

		public bool ShouldRenameFile
		{
			get { return _settings.ShouldRenameFile; }
			set { _settings.ShouldRenameFile = value; }
		}

		public bool ShouldUnzipFile
		{
			get { return _settings.ShouldUnzipFile; }
			set { _settings.ShouldUnzipFile = value; }
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
				_settings.Languages = value;
			}
		}
		#endregion

		#region Commands
		public RelayCommand<object> SelectLanguage { get { return new RelayCommand<object>(OnSelectLanguage);} }



		#endregion

		private void OnSelectLanguage(object obj)
		{
			
		}
		public SettingsWindowViewModel(ISettings settings)
		{
			_settings = settings;
			Languages.OnCollectionSave += sender =>
			{
				_settings.Languages = sender as LanguageCollection;
			};
		}

		
	}
}
