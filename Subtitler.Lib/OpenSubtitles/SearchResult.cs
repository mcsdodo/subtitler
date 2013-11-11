using System;
using System.Collections.Generic;
using CookComputing.XmlRpc;

namespace Subtitler.Lib.OpenSubtitles
{
	public class SearchResult
	{
		public string SubLanguageID;
		public string SubFileName;
		public string MovieName;
		public string IDMovieImdb;
		public string MovieImdbRating;
		public string LanguageName;
		public string MovieReleaseName;
		public string SubDownloadLink;
		public string SubtitlesLink;
		public string ZipDownloadLink;
		public string MovieYear;

		private static IEnumerable<SearchResult> ParseEntry(Array subtitles)
		{
			var results = new List<SearchResult>();
			foreach (XmlRpcStruct subtitle in subtitles)
			{
				results.Add(new SearchResult()
					{
						IDMovieImdb = (string)subtitle["IDMovieImdb"],
						LanguageName = (string)subtitle["LanguageName"],
						MovieImdbRating = (string)subtitle["MovieImdbRating"],
						MovieName = (string)subtitle["MovieName"],
						SubDownloadLink = (string)subtitle["SubDownloadLink"],
						SubFileName = (string)subtitle["SubFileName"],
						SubLanguageID = (string)subtitle["SubLanguageID"],
						SubtitlesLink = (string)subtitle["SubtitlesLink"],
						MovieReleaseName = (string)subtitle["MovieReleaseName"],
						MovieYear = (string)subtitle["MovieYear"],
						ZipDownloadLink = (string)subtitle["ZipDownloadLink"]
					});
			}
			return results;
		}

		public static IEnumerable<SearchResult> ParseResponse(XmlRpcStruct response)
		{
			var results = new List<SearchResult>();

			if (response.ContainsKey("data") && !(response["data"] is bool))
				results.AddRange(ParseEntry(response["data"] as Array));

			return results;
		}
	}
}