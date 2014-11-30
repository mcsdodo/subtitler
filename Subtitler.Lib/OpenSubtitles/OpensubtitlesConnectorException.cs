using System;

namespace Subtitler.Lib.OpenSubtitles
{
	public class OpensubtitlesConnectorException : Exception
	{
		public OpensubtitlesConnectorException() {}
		public OpensubtitlesConnectorException(string message) : base (message){}
		public OpensubtitlesConnectorException(string message, Exception inner) : base(message, inner) {}
	}
}