using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subtitler.Desktop.Models;

namespace Subtitler.Desktop.DAL
{
	public interface IDataService
	{
		void LogIn();
		void LogOut();
		bool CanConnect { get; }
		List<Subtitle> GetSubtitles(string file, params string[] languages);
	}
}
