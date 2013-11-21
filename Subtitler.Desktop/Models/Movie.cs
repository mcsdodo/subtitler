using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subtitler.Lib.Helpers;

namespace Subtitler.Desktop.Models
{
	public class Movie : INullable
	{
		
		public string NameWithoutExt { get; set; }
		public string Directory { get; set; }
		public string Extension { get; set; }
		public string Name { get; set; }
		public string FullPath { get; set; }

		protected Movie(){}
		private Movie(FileInfo info)
		{
			Name = info.Name;
			Directory = info.Directory.FullName;
			Extension = info.Extension;
			NameWithoutExt = info.Name.Remove(info.Name.IndexOf(info.Extension), info.Extension.Length);
			FullPath = info.FullName;
		}

		public static Movie FromFile(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return NullMovie();
			}

			var info = new FileInfo(path);

			if (!info.Exists || !FileHelper.IsFileTypeAllowed(info.Extension, App.AllowedExtensions))
			{
				return NullMovie();
			}
			return new Movie(info);
		}

		public static Movie NullMovie()
		{
			return new NullMovie();
		}

		public virtual bool IsNull { get { return false; } }
	}

	public class NullMovie : Movie
	{
		internal NullMovie() { }
		public override bool IsNull { get { return true; } }
	}
}
