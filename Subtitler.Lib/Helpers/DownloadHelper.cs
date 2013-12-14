using System;
using System.IO;
using System.Net;

namespace Subtitler.Lib.Helpers
{
	public class DownloadHelper : IDownloadHelper
	{
		#region properties
		private string _url;
		private string _directory;
		private string _zipFileName;
		private string _fullZipPath;
		#endregion

		public void DownloadFileAsync(string url, string targetDirectory, string targetFileName, Action<string> callback)
		{
			_url = url;
			_directory = targetDirectory + "\\";
			_zipFileName = targetFileName + ".gz";
			_fullZipPath = _directory + _zipFileName;
			DownloadFileAsync(callback);
		}

		private void DownloadFileAsync(Action<string> callback)
		{
			using (var client = new WebClient())
			{
				client.DownloadFileAsync(new Uri(_url), _fullZipPath);
				client.DownloadFileCompleted += (sender, args) =>
				{
					callback(_fullZipPath);
				};
			}
		}

		private string ResolveFileNameFromHeaders(WebHeaderCollection responseHeaders)
		{
			string serverFileName = "";
			string contentDisposition = responseHeaders["content-disposition"];

			if (!String.IsNullOrEmpty(contentDisposition))
			{
				string lookFor = "filename=";
				int index = contentDisposition.IndexOf(lookFor, StringComparison.CurrentCultureIgnoreCase);
				if (index >= 0)
					serverFileName = contentDisposition.Substring(index + lookFor.Length);
			}
			if (serverFileName.Length > 0)
			{
				serverFileName = serverFileName.Replace("\"", "");
			}

			return serverFileName;
		}
	}
}
