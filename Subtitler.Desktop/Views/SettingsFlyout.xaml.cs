using System.ComponentModel;
using System.Runtime.CompilerServices;
using MahApps.Metro.Controls;

namespace Subtitler.Desktop.Views
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class SettingsFlyout : Flyout, INotifyPropertyChanged
	{
		public SettingsFlyout()
		{
			InitializeComponent();
		}



		public event PropertyChangedEventHandler PropertyChanged;

		
	}
}
