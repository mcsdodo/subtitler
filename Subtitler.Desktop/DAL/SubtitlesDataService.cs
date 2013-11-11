using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subtitler.Desktop.Models;
using Subtitler.Lib.OpenSubtitles;

namespace Subtitler.Desktop.DAL
{
	public class SubtitlesDataService : IDataService
	{
		public static readonly IConnector Connector;

		static SubtitlesDataService()
		{
			Connector = OpensubtitlesConnector.CreateConnector(App.ServerUrl);
		}

		public void LogIn()
		{
			Connector.LogIn();
		}

		public void LogOut()
		{
			Connector.LogOut();
		}

		public bool CanConnect 
		{ 
			get
			{
				return Connector != null;
			} 
		}

		public List<Subtitle> GetSubtitles(string file, params string[] languages)
		{
			var subtitles = Connector.SearchSubtitles(file, languages);

			var parsed = SearchResult.ParseResponse(subtitles);

			return parsed.Select(p => new Subtitle(p.SubFileName, p.SubLanguageID, p.SubDownloadLink)).ToList();
		}

	}
}
