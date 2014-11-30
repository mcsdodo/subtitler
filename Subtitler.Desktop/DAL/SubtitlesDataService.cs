using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Subtitler.Desktop.Models;
using Subtitler.Lib.OpenSubtitles;

namespace Subtitler.Desktop.DAL
{
	public class SubtitlesDataService : IDataService
	{
		private readonly IConnector _connector;

		public SubtitlesDataService(IConnector connector)
		{
			_connector = connector;
		}

		public void LogOut()
		{
			_connector.LogOut();
		}

		public List<Subtitle> GetSubtitles(string file, params string[] languages)
		{
			_connector.LogIn();
			if (_connector.LoggedIn)
			{
				var subtitles = _connector.SearchSubtitles(file, languages);
				Mapper.CreateMap<SearchResult, Subtitle>();
				return Mapper.Map<IEnumerable<SearchResult>, IEnumerable<Subtitle>>(subtitles).ToList();
			}
			return new List<Subtitle>();
		}

	}
}
