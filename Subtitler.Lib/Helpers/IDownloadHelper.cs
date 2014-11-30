using System;

namespace Subtitler.Lib.Helpers
{
	public interface IDownloadHelper
	{
		void DownloadFileAsync(string url, string targetDirectory, string targetFileName, Action<string> callback);
	}
}
