using System.Collections.Generic;
using CookComputing.XmlRpc;

namespace Subtitler.Lib.OpenSubtitles
{
	public interface IConnector
	{
		void LogIn();
		void LogOut();
		IEnumerable<SearchResult> SearchSubtitles(string file, string[] languages);
	}
}
