﻿using System;
using System.Configuration;
using System.Windows;
using AutoMapper;
using MahApps.Metro;
using Microsoft.Practices.ServiceLocation;
using Subtitler.Desktop.DAL;
using Subtitler.Lib.Helpers;

namespace Subtitler.Desktop
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static readonly string ServerUrl;
		public static readonly string AllowedExtensions;

		static App()
		{
			ServerUrl = ConfigurationManager.AppSettings["serverUrl"];
			AllowedExtensions = ConfigurationManager.AppSettings["allowedFileExtensions"];
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
				var service = ServiceLocator.Current.GetInstance<IDataService>();
				service.LogOut();
				Desktop.Properties.Settings.Default.Save();
			}
			finally
			{
				base.OnExit(e);	
			}
		}
	}
}
