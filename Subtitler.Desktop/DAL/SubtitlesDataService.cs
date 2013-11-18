using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Subtitler.Desktop.Models;
using Subtitler.Lib.OpenSubtitles;

namespace Subtitler.Desktop.DAL
{
	public class SubtitlesDataService : IDataService
	{
		public static readonly IConnector Connector;

		private IConnector _connector;

		static SubtitlesDataService()
		{
			//todo move to app_start somewhere
			Connector = OpensubtitlesConnector.CreateConnector(App.ServerUrl);
		}

		//public SubtitlesDataService(IConnector connector)
		//{
		//	_connector = connector;
		//}

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
			var subs = new List<Subtitle>();

			try
			{
				var subtitles = Connector.SearchSubtitles(file, languages);
				//preco ked to dam to locatora, nejde Design?
				Mapper.CreateMap<SearchResult, Subtitle>();
				subs = Mapper.Map<IEnumerable<SearchResult>, IEnumerable<Subtitle>>(subtitles).ToList();
				//return subtitles.Select(p => new Subtitle(p.SubFileName, p.SubLanguageID, p.SubDownloadLink)).ToList();
			}
			catch (OpensubtitlesConnectorException) {}

			return subs;
			
		}

	}
}
