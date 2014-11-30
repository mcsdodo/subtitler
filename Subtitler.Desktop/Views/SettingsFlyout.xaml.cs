using System.ComponentModel;
using System.Runtime.CompilerServices;
using MahApps.Metro.Controls;
using Subtitler.Desktop.Annotations;

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

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
		
	}
}
