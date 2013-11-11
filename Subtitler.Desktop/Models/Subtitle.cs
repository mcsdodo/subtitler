using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Media;
using Subtitler.Desktop.Helpers;
using Subtitler.Lib;
using Subtitler.Lib.Helpers;

namespace Subtitler.Desktop.Models
{
	public class Subtitle : NotificationObject
	{
		private string _downloadLink;

		public Subtitle(string subFileName, string langShort, string downloadZipLink)
		{
			SubFileName = subFileName;
			Language = Language.GetLanguages().First(l => l.Id == langShort);
			_downloadLink = downloadZipLink;
		}


		private string _subFileName;
		public string SubFileName
		{
			get { return _subFileName; }
			set
			{
				if (_subFileName != value)
				{
					_subFileName = value;
					RaisePropertyChanged(() => SubFileName);
				}
			}
		}

		public Language Language { get; set; }

		public void Download(string directory, string fileName, bool unzip, Action callback)
		{
			var dh = new DownloadHelper(_downloadLink, directory, fileName, unzip);
			dh.DownloadFileAsync(callback);
		}	
	}
}
