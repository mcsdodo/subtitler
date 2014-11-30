namespace Subtitler.Desktop.Models
{
	public interface ISettings
	{
		bool ShouldRenameFile { get; set; }
		bool ShouldUnzipFile { get; set; }
		string Theme { get; set; }
		LanguageCollection Languages { get; set; }
	}
}
