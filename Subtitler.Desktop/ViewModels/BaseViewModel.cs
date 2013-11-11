using System.Windows;
using GalaSoft.MvvmLight;
using MahApps.Metro.Controls;
using Subtitler.Desktop.Helpers;

namespace Subtitler.Desktop.ViewModels
{
	public class SubtitlerViewModelBase : ViewModelBase
	{
		public void ShowMessage(string header, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
		{
			((MetroWindow) Application.Current.MainWindow).ShowMessageAsync(header, message, style);
		}
	}
}