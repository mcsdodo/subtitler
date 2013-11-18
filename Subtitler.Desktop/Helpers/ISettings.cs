using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subtitler.Desktop.Models;

namespace Subtitler.Desktop.Helpers
{
	public interface ISettings
	{
		bool ShouldRenameFile { get; set; }
		bool ShouldUnzipFile { get; set; }
		string Theme { get; set; }
		LanguageCollection Languages { get; set; }
	}
}
