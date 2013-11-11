using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Subtitler.Desktop.Helpers;
using Subtitler.Desktop.Models;
using Subtitler.Lib;
using Subtitler.Lib.Helpers;

namespace Subtitler.Desktop.DAL
{
	public class DesignSubtitlesDataService : IDataService
	{
		public void LogIn()
		{
			//throw new NotImplementedException();
		}

		public void LogOut()
		{
			//throw new NotImplementedException();
		}

		public bool CanConnect
		{
			get { return true; }
		}

		public List<Subtitle> GetSubtitles(string file, params string[] langs)
		{
			var allSettingsLangs = (new Settings()).Languages;

			var results = new List<Subtitle>();
			for (var i = 0; i < 20; i++)
			{
				var langShort = allSettingsLangs.Where(l => l.Use).ToList().RandItem().Id;
				results.Add(new Subtitle(RandomHelper.RandomString(25, true), langShort, RandomHelper.RandomString(20, true)));
			}
			return results;
		}

	}
}
