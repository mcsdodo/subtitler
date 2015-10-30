using Microsoft.Win32;

namespace Subtitler.Desktop.DAL
{
	public class OpenFile : IOpenFile
	{
		public string OpenFileDialog(string defaultPath = "")
		{
			OpenFileDialog dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == true)
			{
				var file = dialog.FileName;
				return file;
			}
			return "";
		}

	}
}
