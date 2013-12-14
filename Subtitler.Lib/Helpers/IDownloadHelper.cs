using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subtitler.Lib.Helpers
{
	public interface IDownloadHelper
	{
		void DownloadFileAsync(string url, string targetDirectory, string targetFileName, Action<string> callback);
	}
}
