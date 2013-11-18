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
	}

	public static class Status
	{
		public const string OK = "200 OK";
		public const string PartialOK = "206 Partial content; message";

		public const string Moved = "301 Moved (host)";

		public const string E401 = "401 Unauthorized";
		public const string E402 = "402 Subtitles has invalid format";
		public const string E403 = "403 SubHashes (content and sent subhash) are not same!";
		public const string E404 = "404 Subtitles has invalid language!";
		public const string E405 = "405 Not all mandatory parameters was specified";
		public const string E406 = "406 No session";
		public const string E407 = "407 Download limit reached";
		public const string E408 = "408 Invalid parameters";
		public const string E409 = "409 Method not found";
		public const string E410 = "410 Other or unknown error";
		public const string E411 = "411 Empty or invalid useragent";
		public const string E412 = "412 %s has invalid format (reason)";
		public const string E413 = "413 Invalid ImdbID";
		public const string E414 = "414 Unknown User Agent";
		public const string E415 = "415 Disabled user agent";
		public const string E416 = "416 Internal subtitle validation failed";

		public const string E503 = "503 Service Unavailable";
	}
}