using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Subtitler.Desktop.Annotations;
using Subtitler.Desktop.Helpers;

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
