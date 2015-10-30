using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subtitler.Desktop.DAL
{
	public interface IOpenFile
	{
		string OpenFileDialog(string defaultPath ="");
	}
}
