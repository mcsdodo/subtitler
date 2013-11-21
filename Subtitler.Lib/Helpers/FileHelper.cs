using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;

namespace Subtitler.Lib.Helpers
{
	public static class FileHelper
	{
		public static void ExtractArchive(string archivePath, string outPath)
		{
			try
			{
				using (FileStream fInStream = new FileStream(archivePath, FileMode.Open, FileAccess.Read))
				{
					using (GZipStream zipStream = new GZipStream(fInStream, CompressionMode.Decompress))
					{
						using (FileStream fOutStream = new FileStream(outPath, FileMode.Create, FileAccess.Write))
						{
							byte[] tempBytes = new byte[4096];
							int i;
							while ((i = zipStream.Read(tempBytes, 0, tempBytes.Length)) != 0)
							{
								fOutStream.Write(tempBytes, 0, i);
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		public static void WriteFileFromBytes(string outPath, byte[] byteArray)
		{
			using (FileStream fOutStream = new FileStream(outPath, FileMode.Create, FileAccess.Write))
			{	
				fOutStream.Write(byteArray, 0, byteArray.Length);
			}
		}

		public static string StripExtension(string name)
		{
			var ext = Path.GetExtension(name);
			return name.Remove(name.LastIndexOf(ext));
		}

		public static string GetExtensionFromString(string name)
		{
			var ext = Path.GetExtension(name);
			return ext;
		}

		public static void WriteFileFromStream(Stream rawStream, string destination)
		{
			MemoryStream mStream = new MemoryStream();
			rawStream.CopyTo(mStream);
			File.WriteAllBytes(destination, mStream.ToArray());
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
