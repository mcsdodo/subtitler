using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subtitler.Desktop.Models
{
	public class MovieFile
	{
		public string Name { get; set; }
		public string Directory { get; set; }
		public string Extension { get; set; }
		public string FullName { get; set; }

		public static MovieFile ParseFile(string path)
		{
			FileInfo info = new FileInfo(path);
			return new MovieFile()
				{
					FullName = info.Name,
					Directory = info.Directory.FullName,
					Extension = info.Extension,
					Name = info.Name.Remove(info.Name.IndexOf(info.Extension), info.Extension.Length)
				};
		}
	}
}
