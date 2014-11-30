using System;

namespace Subtitler.Lib.OpenSubtitles
{
	public class ConnectorEventArgs : EventArgs
	{
		public string Message { get; set; }
		public bool CanConnect { get; set; }
		public ConnectorEventArgs(string message, bool canConnect)
		{
			Message = message;
			CanConnect = canConnect;
		}
	}
}