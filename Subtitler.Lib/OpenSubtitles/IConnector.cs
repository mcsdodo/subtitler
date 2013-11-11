using CookComputing.XmlRpc;

namespace Subtitler.Lib.OpenSubtitles
{
	public interface IConnector
	{
		void LogIn();
		void LogOut();
		XmlRpcStruct SearchSubtitles(string file, string[] languages);
	}
}
