using CookComputing.XmlRpc;

namespace Subtitler.Lib.OpenSubtitles
{
	public interface IOpensubtitlesProxy : IXmlRpcProxy
	{
		[XmlRpcMethod]
		XmlRpcStruct LogIn(string userName, string password, string language, string userAgent);

		[XmlRpcMethod]
		LogOutResult LogOut(string token);

		[XmlRpcMethod]
		XmlRpcStruct SearchSubtitles(string token, SearchParams[] searchParams);

		[XmlRpcMethod]
		XmlRpcStruct NoOperation(string token);
	}
}