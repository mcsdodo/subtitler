﻿using System;
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

		#region ctor

		public Subtitle(){}

		public Subtitle(string subFileName, string langShort, string downloadZipLink)
		{
			SubFileName = subFileName;
			_language = Language.GetAllLanguages().First(l => l.Id == langShort);
			ZipDownloadLink = downloadZipLink;
		} 
		#endregion


		#region Properties

		public string SubLanguageID { get; set; }
		public string SubFileName { get; set; }
		public string MovieName { get; set; }
		public string IDMovieImdb { get; set; }
		public string MovieImdbRating { get; set; }
		public string LanguageName { get; set; }
		public string MovieReleaseName { get; set; }
		public string SubDownloadLink { get; set; }
		public string SubtitlesLink { get; set; }
		public string ZipDownloadLink { get; set; }
		public string MovieYear { get; set; }

		private Language _language;
		public Language Language
		{
			get { return _language ?? (_language = Language.GetAllLanguages().First(l => l.Id == SubLanguageID)); }
		}

		#endregion	
	}
}
