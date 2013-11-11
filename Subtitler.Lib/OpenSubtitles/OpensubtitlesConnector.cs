using System;
using System.IO;
using System.Net;
using CookComputing.XmlRpc;
using Subtitler.Lib.Helpers;

namespace Subtitler.Lib.OpenSubtitles
{
	public class OpensubtitlesConnector : IConnector
	{
		private string _token;
		private IOpensubtitlesProxy _proxy;
		public bool LoggedIn { get; set; }

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

		private OpensubtitlesConnector(string serverUrl)
		{
			_proxy = XmlRpcProxyGen.Create<IOpensubtitlesProxy>();
			_proxy.Url = serverUrl;
			_proxy.EnableCompression = true;
		}

		public void LogIn()
		{
			var response = _proxy.LogIn("", "", "slo", "Subtitler MCS");
			LoggedIn = true;
			_token = response.token;
		}

		public void LogOut()
		{
			var response = _proxy.LogOut(_token);
		}

		public XmlRpcStruct SearchSubtitles(string file, string[] languages)
		{
			CheckLogin();
			var searchParams = CreateSearchParams(file, languages);
			XmlRpcStruct response = _proxy.SearchSubtitles(_token, new[] { searchParams });
			ParseStatus(response);

			return response;
		}

		#region helper methods
		/// <summary>
		/// Checks if Connector is logged in, throws InvalidOperationException if the LogIn method was not called.
		/// </summary>
		private void CheckLogin()
		{
			if (!LoggedIn)
				throw new InvalidOperationException("Need to call LogIn() method first.");
		}


		private static SearchParams CreateSearchParams(string filePath, string[] languages)
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
		private static bool ParseStatus(XmlRpcStruct response)
		{
			if (response.ContainsKey("status") && ParseStatus(response["status"] as string))
				return true;
			if (response.ContainsKey("status"))
				return false;
			throw new Exception("Something went wrong. Response: " + response.ToString());
		}

		private static bool ParseStatus(string status)
		{
			return status == "200 OK";
		} 
		#endregion

	}
}
