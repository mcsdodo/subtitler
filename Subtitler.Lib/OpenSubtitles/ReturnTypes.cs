namespace Subtitler.Lib.OpenSubtitles
{
	public class LogOutResult
	{
		public string status;
		public double seconds;
	}

	public class LogInResult
	{
		public string token;
		public string status;
		public double seconds;
	}


	public class SearchParams
	{
		public string sublanguageid;
		public string moviehash;
		public double moviebytesize;
	}

	public class SearchParamsExtended
	{
		public string sublanguageid;
		public string moviehash;
		public double moviebytesize;
		public string imdbid;
	}
}
