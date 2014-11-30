using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using CookComputing.XmlRpc;

namespace Subtitler.Lib.OpenSubtitles
{
	/// <exception cref="OpensubtitlesConnectorException">Throws if something went wrong.</exception>
	public class OpensubtitlesConnector : IConnector
	{
		public event EventHandler<ConnectorEventArgs> OnStatusUpdate;

		#region privates
		private string _token;
		private IOpensubtitlesProxy _proxy;
		private readonly string _serverUrl;
		public bool LoggedIn { get; private set; }
		private readonly string _userAgent;
		private readonly string _language;
		private readonly string _userName;
		private readonly string _password;
		#endregion


		#region ctor
		public OpensubtitlesConnector(string serverUrl, string userAgent, string language = "", string userName ="", string password = "")
		{
			_serverUrl = serverUrl;
			_userAgent = userAgent;
			_language = language;
			_userName = userName;
			_password = password;
			CreateProxy();
		}

		private void CreateProxy()
		{
			_proxy = XmlRpcProxyGen.Create<IOpensubtitlesProxy>();
			_proxy.Url = _serverUrl;
			_proxy.EnableCompression = true;
		}
		private OpensubtitlesConnector(){}
		#endregion

		#region API
		public void LogIn()
		{
			var response = _proxy.LogIn(_userName, _password, _language, _userAgent);
			LoggedIn = ParseStatus(response);
			_token = response["token"] as string;
		}

		public void LogOut()
		{
			var response = _proxy.LogOut(_token);
		}

		public void KeepAlive()
		{
			var response = _proxy.NoOperation(_token);
			if (!ParseStatus(response))
				LogIn();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="file">Full path to movie file with allowed extension enumerated in config entry allowedExtensions</param>
		/// <param name="languages"></param>
		/// <returns></returns>
		public IEnumerable<SearchResult> SearchSubtitles(string file, string[] languages)
		{
			KeepAlive();
			if (!IsValidFile(file))
				throw new OpensubtitlesConnectorException("Provided file not valid.");

			CheckLogin();
			var searchParams = CreateSearchParams(file, languages);
			XmlRpcStruct response = _proxy.SearchSubtitles(_token, new[] { searchParams });
			return ParseResponse(response).ToList(); 
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
		private bool IsValidFile(string file)
		{
			return !string.IsNullOrEmpty(file) && new FileInfo(file).Exists;
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
				if (status == Status.E406)
					return false;
				if (status == Status.E414)
					throw new OpensubtitlesConnectorException("414 Unknown User Agent.");
				if (status == Status.E503)
					throw new OpensubtitlesConnectorException("503 Service unavailable", new WebException("503 Service unavailable"));
			}				
			throw new OpensubtitlesConnectorException("No status? Really weird ...");
		}
		#endregion

	}
}
