using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using CookComputing.XmlRpc;
using Subtitler.Lib.Helpers;

namespace Subtitler.Lib.OpenSubtitles
{
	//TODO more search options
	//todo extract to DLL
	//todo various exceptions
	/// <exception cref="OpensubtitlesConnectorException">Throws if something went wrong.</exception>
	public class OpensubtitlesConnector : IConnector
	{
		private string _token;
		private IOpensubtitlesProxy _proxy;
		private bool LoggedIn { get; set; }

		#region factory
		public static OpensubtitlesConnector CreateConnector(string serverUrl)
		{
			try
			{
				ConnectionHelper.CheckConnection(serverUrl);
				var connector = new OpensubtitlesConnector(serverUrl);
				return connector;
			}
			catch (WebException e)
			{
				return null;
			}
		} 
		#endregion

		#region ctor
		private OpensubtitlesConnector(string serverUrl)
		{
			_proxy = XmlRpcProxyGen.Create<IOpensubtitlesProxy>();
			_proxy.Url = serverUrl;
			_proxy.EnableCompression = true;
		} 
		#endregion

		#region API
		public void LogIn()
		{
			var response = _proxy.LogIn("", "", "slo", "Subtitler MCS");
			LoggedIn = ParseStatus(response);
			_token = response["token"] as string;
			CheckLogin();
		}

		public void LogOut()
		{
			CheckLogin();
			var response = _proxy.LogOut(_token);
		}

		public void KeepAlive()
		{
			CheckLogin();
			var response = _proxy.NoOperation(_token);
			LoggedIn = ParseStatus(response);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="file">Full path to movie file with allowed extension enumerated in config entry allowedExtensions</param>
		/// <param name="languages"></param>
		/// <returns></returns>
		public IEnumerable<SearchResult> SearchSubtitles(string file, string[] languages)
		{
			var result = new List<SearchResult>();

			if (IsValidFile(file))
			{
				CheckLogin();
				var searchParams = CreateSearchParams(file, languages);
				XmlRpcStruct response = _proxy.SearchSubtitles(_token, new[] { searchParams });
				result = ParseResponse(response).ToList(); 
			}
			else
			{
				throw new OpensubtitlesConnectorException("Provided file not valid.");
			}

			return result;
		} 
		#endregion

		#region helper methods
		/// <summary>
		/// Checks if Connector is logged in, throws InvalidOperationException if the LogIn method was not called or failed.
		/// </summary>
		private void CheckLogin()
		{
			if (!LoggedIn || string.IsNullOrEmpty(_token))
				throw new OpensubtitlesConnectorException("Need to call LogIn() method first.");
		}

		/// <summary>
		///  Check if file exists, todo checks if allowed
		/// </summary>
		private static bool IsValidFile(string file)
		{
			if (!string.IsNullOrEmpty(file))
			{
				var info = new FileInfo(file);
				if (info.Exists)
					return true;
			}
			return false;
		}

		private IEnumerable<SearchResult> ParseResponse(XmlRpcStruct response)
		{
			var results = new List<SearchResult>();
			
			var ok = ParseStatus(response);

			if (ok && response.ContainsKey("data") && !(response["data"] is bool))
			{
				results.AddRange(ParseEntry(response["data"] as Array));
			}
			return results;
		}

		private IEnumerable<SearchResult> ParseEntry(Array subtitles)
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


		private SearchParams CreateSearchParams(string filePath, string[] languages)
		{
			FileInfo info = new FileInfo(filePath);
			double size = info.Length;
			byte[] hash = GetHash.Main.ComputeHash(filePath);
			string hashs = GetHash.Main.ToHexadecimal(hash);

			string lang = string.Join(",", languages);

			var searchParams = new SearchParams()
				{
					sublanguageid = lang,
					moviehash = hashs,
					moviebytesize = size
				};
			return searchParams;
		}

		/// <summary>
		/// Checks status response, returns true if 200, false otherwise, else throws an Exception.
		/// </summary>
		private bool ParseStatus(XmlRpcStruct response)
		{
			if (response.ContainsKey("status"))
			{
				var status = response["status"] as string;
				if (status == Status.OK)
					return true;
				if (status == Status.E503)
					throw new OpensubtitlesConnectorException("503 Service unavailable", new WebException("503 Service unavailable"));
			}				
			throw new OpensubtitlesConnectorException("No status? Really weird ...");
		}
		#endregion

	}

	public class OpensubtitlesConnectorException : Exception
	{
		public OpensubtitlesConnectorException() {}
		public OpensubtitlesConnectorException(string message) : base (message){}
		public OpensubtitlesConnectorException(string message, Exception inner) : base(message, inner) {}
	}
}
