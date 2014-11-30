using GalaSoft.MvvmLight;
using Microsoft.Practices.Unity;
using Subtitler.Desktop.DAL;
using Subtitler.Desktop.Models;
using Subtitler.Lib.Helpers;
using Subtitler.Lib.OpenSubtitles;

namespace Subtitler.Desktop.ViewModels
{
	/// <summary>
	/// This class contains static references to all the view models in the
	/// application and provides an entry point for the bindings.
	/// </summary>
	public class ViewModelLocator
	{

		public static IUnityContainer Container;

		/// <summary>
		/// Initializes a new instance of the ViewModelLocator class.
		/// </summary>
		public ViewModelLocator()
		{
			Container = new UnityContainer();

			if (ViewModelBase.IsInDesignModeStatic)
			{
				// Create design time view services and models
				Container.RegisterType<IDataService, DesignSubtitlesDataService>();
			}
			else
			{
				// Create run time view services and models
				Container.RegisterType<IDataService, SubtitlesDataService>();
			}

			Container.RegisterType<ISettings, Settings>();
			Container.RegisterType<IOService, IOServiceImpl>();
			Container.RegisterType<IDownloadHelper, DownloadHelper>();
			Container.RegisterType<IFileHelper, FileHelper>();
			Container.RegisterInstance(typeof (IConnector), new OpensubtitlesConnector(App.ServerUrl, App.UserAgent, "slo"),
			                            new PerThreadLifetimeManager());


			Container.RegisterType<MainWindowViewModel>();
			Container.RegisterType<SettingsWindowViewModel>();			
		}

		public MainWindowViewModel MainWindow
		{
			get { return Container.Resolve<MainWindowViewModel>(); }
		}

		public SettingsWindowViewModel SettingsFlyout
		{
			get { return Container.Resolve<SettingsWindowViewModel>(); }
		}
	}
}