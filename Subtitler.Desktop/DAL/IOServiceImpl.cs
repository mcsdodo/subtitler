using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Subtitler.Desktop.DAL
{
	public class IOServiceImpl : IOService
	{
		public string OpenFileDialog(string defaultPath)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == true)
			{
				var file = dialog.FileName;
				return file;
			}
			return "";
		}

		public Stream OpenFile(string path)
		{
			throw new NotImplementedException();
		}
	}
}
