using System;
using System.Configuration;
using System.Windows;
using Subtitler.Lib.Helpers;

namespace Subtitler.Desktop
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static readonly string ServerUrl;
		public static string TestFile;

		static App()
		{
			ServerUrl = ConfigurationManager.AppSettings["serverUrl"];
			TestFile = ArgumentsHelper.ParseArguments(Environment.GetCommandLineArgs(), "file");
		}


		public App()
		{
			
		}


		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

		}

		protected override void OnExit(ExitEventArgs e)
		{
			try
			{
				
			}
			finally
			{
				base.OnExit(e);	
			}
		}
	}
}
