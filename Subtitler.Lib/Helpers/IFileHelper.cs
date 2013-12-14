using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subtitler.Lib.Helpers
{
	public interface IFileHelper
	{
		void ExtractArchive(string archivePath, string outPath, string fileName);
	}
}
