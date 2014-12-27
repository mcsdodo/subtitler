using System;
using System.Configuration;
using System.Windows;
using Microsoft.Practices.Unity;
using Subtitler.Desktop.DAL;
using Subtitler.Desktop.ViewModels;

namespace Subtitler.Desktop
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static string ServerUrl;
		public static string AllowedExtensions;
		public static string UserAgent;
		
		public App()
		{
			ServerUrl = ConfigurationManager.AppSettings["serverUrl"];
			AllowedExtensions = ConfigurationManager.AppSettings["allowedFileExtensions"];
			UserAgent = ConfigurationManager.AppSettings["userAgent"];
		}

		protected override void OnExit(ExitEventArgs e)
		{
			try
			{
				Desktop.Properties.Settings.Default.Save();
			}
			finally
			{
				base.OnExit(e);	
			}
		}
	}
}
