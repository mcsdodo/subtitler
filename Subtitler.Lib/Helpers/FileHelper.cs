using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;
using Ionic.Zip;

namespace Subtitler.Lib.Helpers
{
	public class FileHelper : IFileHelper
	{
		public void ExtractArchive(string archivePath, string outPath, string fileName= "")
		{
			var subtitleExtensions = new List<string>() { ".srt", ".sub" };
				using (ZipFile zipFileEntries = ZipFile.Read(archivePath))
				{
					foreach (ZipEntry zipEntry in zipFileEntries)
					{
						var originalExtension = (new FileInfo(zipEntry.FileName)).Extension;
						if (subtitleExtensions.Contains(originalExtension.ToLower()))
						{
							if (!string.IsNullOrEmpty(fileName))
							{
								zipEntry.FileName = fileName + originalExtension;
							}
							zipEntry.Extract(outPath);
							return;
						}
						
					}
				}
		}

		/// <summary>
		/// Takes comma separated list of extensions (".mpeg, .avi, ...") and extension to compare with
		/// </summary>
		public static bool IsFileTypeAllowed(string extension, string allowedExtensions)
		{
			var allowedList = new List<String>(allowedExtensions.Split(',')).Select(s => s.Trim());
			return allowedList.Contains(extension.ToLower());
		}

		public static string[] GetFilesFromDropEvent(DragEventArgs e)
		{
			if (e.Data == null)
				return null;

			var files = (e.Data.GetData(DataFormats.FileDrop) as string[]);
			if (files == null || files.Length == 0)
				return null;
			return files;
		}
	}
}
