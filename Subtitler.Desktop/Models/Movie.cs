using System.IO;

namespace Subtitler.Desktop.Models
{
	public class Movie
	{
		
		public string NameWithoutExt { get; set; }
		public string Directory { get; set; }
		public string Extension { get; set; }
		public string Name { get; set; }
		public string FullPath { get; set; }

		public Movie(FileInfo info)
		{
			Name = info.Name;
			Directory = info.DirectoryName;
			Extension = info.Extension;
			NameWithoutExt = info.Name.Remove(info.Name.IndexOf(info.Extension), info.Extension.Length);
			FullPath = info.FullName;
		}

	}

}
