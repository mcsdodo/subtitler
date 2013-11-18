using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtitler.Lib.Helpers;

namespace Subtitler.Test
{
	[TestClass]
	public class HelperTests
	{
		[TestMethod]
		public void ParseExtension_CorrectExtension()
		{
			string fileName = "movie.avi";

			string ext = FileHelper.GetFileExtension(fileName);

			Assert.AreEqual("avi", ext);

		}

		[TestMethod]
		public void ParseExtension_NoExtension()
		{
			string fileName = "movie";

			string ext = FileHelper.GetFileExtension(fileName);

			Assert.AreEqual(null, ext);
		}

		[TestMethod]
		public void StripExtension_Ok()
		{
			string fileName = "movie.avi";
			string stripped = FileHelper.StripExtension(fileName);
			Assert.AreEqual("movie", stripped);
		}


		[TestMethod]
		public void ParseExtension_DotExtension()
		{
			string fileName = "movie.";

			string ext = FileHelper.GetFileExtension(fileName);

			Assert.AreEqual(null, ext);
		}

		[TestMethod]
		public void IsExtensionAllowed_Is()
		{
			string ext = "avi";
			bool isAllowed = FileHelper.IsFileTypeAllowed(ext, "avi,mpeg,divx");

			Assert.IsTrue(isAllowed);
		}

		[TestMethod]
		public void IsExtensionAllowed_IsNot()
		{
			string ext = "aaa";
			bool isAllowed = FileHelper.IsFileTypeAllowed(ext, "avi,mpeg,divx");

			Assert.IsFalse(isAllowed);
		}
	}
}
