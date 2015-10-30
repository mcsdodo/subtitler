using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subtitler.Desktop.DAL;
using Subtitler.Desktop.Models;
using Subtitler.Desktop.ViewModels;
using Subtitler.Lib.Helpers;

namespace Subtitler.Test
{
	[TestClass]
	public class OpenFileTest
	{
		private Mock<IOpenFile> _ioService;
		private Mock<IDataService> _dataService;
		private Mock<ISettings> _settings;

		[TestInitialize]
		public void InitializeTest()
		{
			_ioService = new Mock<IOpenFile>();
			_dataService = new Mock<IDataService>();
			_settings = new Mock<ISettings>();
		}

		[TestMethod]
		public void OpenFileCommand_UserSelectsInvalidPath_SelectedPathSetToEmpty()
		{
			//We use null to indicate invalid path in our implementation
			_ioService.Setup(ioServ => ioServ.OpenFileDialog(It.IsAny<string>()))
			             .Returns(() => "");

			//Setup target and test
			var viewModel = new MainWindowViewModel(_dataService.Object, _settings.Object, _ioService.Object, null, null);
			viewModel.OpenFile.Execute(null);

			Assert.IsTrue(!viewModel.HasMovie());
		}



	}
}
