using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
			var SubtitleNames = new[] { "Two and a Half Men - 10x04 - You Do Know What The Lollipop Is For.LOL.Bulgarian.HI.C.updat.srt",
				"Two.and.a.Half.Men..10x06..Ferrets,.Attack!.LOL.Bulgarian.HI.C.updated.Addic7ed.com.srt", "Two and a Half Men - 10x06 - Ferrets, Attack!.LOL.Bulgarian.HI.C.updated.Addic7ed.com.srt", 
				"Two.and.a.Half.Men.S10E05.HDTV.x264-LOL.HI.srt", "Two and a Half Men - 10x06 - Ferrets, Attack!.LOL.Bulgarian.HI.C.updated.Addic7ed.com.srt", "Two.and.a.Half.Men.S10E06.HDTV.x264-LOL.srt" };


			var allSettingsLangs = (new Settings()).Languages;
			Thread.Sleep(2000);
			var results = new List<Subtitle>();
			for (var i = 0; i < 200; i++)
			{
				var langShort = allSettingsLangs.ToList().RandItem().Id;
				var name = SubtitleNames.ToList().RandItem();
				results.Add(new Subtitle(name, langShort, RandomHelper.RandomString(20, true)));
			}
			return results.Where(s => langs.Contains(s.Language.Id)).ToList();
		}

	}
}
