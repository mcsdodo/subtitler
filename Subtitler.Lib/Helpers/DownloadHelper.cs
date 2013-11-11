using System;
using System.IO;
using System.Net;

namespace Subtitler.Lib.Helpers
{
	public class DownloadHelper
	{
		#region properties
		private string _url;
		private string _directory;
		private string _newName;
		private string _zipFileName;
		private bool _extract;
		#endregion

		#region ctor
		public DownloadHelper(string url, string directory, string newName, bool extract = false)
		{
			_url = url;
			_directory = directory + "\\";
			_newName = newName + ".srt";
			_zipFileName = _newName + ".gz";
			_extract = extract;
		} 
		#endregion

		public void DownloadFileAsync(Action callback)
		{
			using (var client = new WebClient())
			{
				client.DownloadFileAsync(new Uri(_url), _directory + _zipFileName);
				client.DownloadFileCompleted += (sender, args) =>
				{
					if (_extract)
					{
						FileHelper.ExtractArchive(_directory + _zipFileName, _directory + _newName);
					}
					callback();
				};
			}
		}

		/// <summary>
		/// Downloads file from 'url' to 'directory', optionally extracts and renames with 'newName'
		/// </summary>
		public static void DownloadFile(string url, string directory, string newSubtitleFileName, bool extract)
		{
			string zipFileName = newSubtitleFileName + ".srt.gz";
			directory = directory + "\\";
			using (var client = new WebClient())
			{
				client.DownloadFile(url, directory + zipFileName);
				
			}

			if (extract)
			{
				FileHelper.ExtractArchive(directory + zipFileName, directory + newSubtitleFileName + ".srt");
			}
		}
		
		private static string ResolveFileNameFromHeaders(WebHeaderCollection responseHeaders)
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
