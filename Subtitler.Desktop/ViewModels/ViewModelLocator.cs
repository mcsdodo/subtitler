/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Subtitler.Desktop"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Subtitler.Desktop.DAL;
using Subtitler.Desktop.Helpers;
using Subtitler.Desktop.Models;
using Subtitler.Lib.OpenSubtitles;

namespace Subtitler.Desktop.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			if (ViewModelBase.IsInDesignModeStatic)
			{
				// Create design time view services and models
				SimpleIoc.Default.Register<IDataService, DesignSubtitlesDataService>();
			}
			else
			{
				// Create run time view services and models
				//SimpleIoc.Default.Register<IDataService, SubtitlesDataService>();
				SimpleIoc.Default.Register<IDataService, DesignSubtitlesDataService>();
			}

			SimpleIoc.Default.Register<ISettings, Settings>();
			SimpleIoc.Default.Register<IOService, IOServiceImpl>();

			SimpleIoc.Default.Register<MainWindowViewModel>();
			SimpleIoc.Default.Register<SettingsWindowViewModel>();			
        }

		public MainWindowViewModel MainWindow
        {
            get
            {
				return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }

	    public SettingsWindowViewModel SettingsFlyout
	    {
		    get
		    {
			    return ServiceLocator.Current.GetInstance<SettingsWindowViewModel>();
		    }
	    }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}