using System;
using System.Net;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subtitler.Desktop.DAL;
using Subtitler.Desktop.Helpers;
using Subtitler.Desktop.ViewModels;
using Subtitler.Lib.OpenSubtitles;
using Subtitler.Lib.Helpers.Fakes;

namespace Subtitler.Test
{
	[TestClass]
	public class OpenSubtitlesConnectorTest
	{
		[TestMethod]
		[ExpectedException(typeof(OpensubtitlesConnectorException))]
		public void TestLoginMandancy()
		{
			var connector = OpensubtitlesConnector.CreateConnector("http://api.opensubtitles.org/xml-rpc");
			connector.SearchSubtitles("file", new[] {"slo"});
		}

		[TestMethod]
		public void TestNoConnection()
		{
			using (ShimsContext.Create())
			{
				ShimConnectionHelper.CheckConnectionString = s => { throw new WebException(); };
				var connector = OpensubtitlesConnector.CreateConnector("http://api.opensubtitles.org/xml-rpc");
				Assert.IsNull(connector);
			}
		}

		[TestMethod]
		public void TestNoConnectionWarning()
		{
			var dataService = new Mock<IDataService>();
			var settings = new Mock<ISettings>();
			var ioServiceStub = new Mock<IOService>();
			dataService.Setup(d => d.CanConnect).Returns(false);

			//var viewModel = new MainWindowViewModel(dataService.Object, settings.Object, ioServiceStub.Object);
		}
	}
}
