using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtitler.Lib.Helpers;

namespace Subtitler.Test
{
	[TestClass]
	public class HelperTests
	{

		[TestMethod]
		public void IsExtensionAllowed_Is()
		{
			string ext = ".avi";
			bool isAllowed = FileHelper.IsFileTypeAllowed(ext, ".avi,.mpeg,.divx");

			Assert.IsTrue(isAllowed);
		}

		[TestMethod]
		public void IsExtensionAllowed_IsNot()
		{
			string ext = ".aaa";
			bool isAllowed = FileHelper.IsFileTypeAllowed(ext, ".avi,.mpeg,.divx");

			Assert.IsFalse(isAllowed);
		}
	}
}
