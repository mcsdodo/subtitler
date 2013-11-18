using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

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
			return name.Remove(name.LastIndexOf('.'));
		}

		public static string GetFileExtension(string name)
		{
			int lastIndexOfDot = name.LastIndexOf('.');
			if (lastIndexOfDot > 1 && lastIndexOfDot < name.Length-1)
			{
				return name.Substring(lastIndexOfDot+1);
			}
			return null;
		}

		public static void WriteFileFromStream(Stream rawStream, string destination)
		{
			MemoryStream mStream = new MemoryStream();
			rawStream.CopyTo(mStream);
			File.WriteAllBytes(destination, mStream.ToArray());
		}

		public static bool IsFileTypeAllowed(string extension, string allowedExtensions)
		{
			var allowedList = new List<String>( allowedExtensions.Split(',') );
			return allowedList.Contains(extension);
		}
	}
}
