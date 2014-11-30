using System;
using System.Net;

namespace Subtitler.Lib.Helpers
{
	public static class ConnectionHelper
	{
		public static bool CheckConnection(String URL)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
				request.Timeout = 5000;
				request.Credentials = CredentialCache.DefaultNetworkCredentials;
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();

				if (response.StatusCode == HttpStatusCode.OK) return true;
				throw new WebException("Can't connect to XML-RPC service. Response: " + response.StatusCode);
			}
			catch (WebException e)
			{
				throw new WebException("Can't connect to XML-RPC service. Response: " + e.Status + " Message: " + e.Message);
			}
		}

	}
}
