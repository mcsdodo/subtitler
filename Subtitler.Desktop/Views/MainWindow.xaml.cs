using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using Subtitler.Desktop.Helpers;

namespace Subtitler.Desktop.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			Loaded += (sender, args) =>
			{
				if (this.DataContext is INeedInternet)
				{
					if ((this.DataContext as INeedInternet).HasInternet == false)
						this.Close();
				}
			};

		}

		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}

		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			var s = new Settings();

			var col = s.Languages;

			Properties.Settings.Default.Save();
			base.OnClosing(e);
		}


		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			this.ShowMessageAsync("title", "message");
		}
	}
}
