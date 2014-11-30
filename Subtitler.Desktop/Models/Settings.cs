using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Subtitler.Desktop.Models
{
	[Serializable]
	public class Settings : ISettings
	{
		public bool ShouldRenameFile
		{
			get { return Properties.Settings.Default.RenameFileAsSource; }
			set { Properties.Settings.Default.RenameFileAsSource = value; }
		}

		public bool ShouldUnzipFile
		{
			get { return Properties.Settings.Default.UnzipFile; }
			set { Properties.Settings.Default.UnzipFile = value; }
		}

		public string Theme
		{
			get { return Properties.Settings.Default.Theme; }
			set { Properties.Settings.Default.Theme = value; }
		}

		private LanguageCollection _languages;
		public LanguageCollection Languages
		{
			get
			{
				var langs = Properties.Settings.Default.Languages;
				if (!string.IsNullOrEmpty(Properties.Settings.Default.Languages))
				{
					_languages = Deserialize<LanguageCollection>(langs);
				}
				else
				{
					_languages = Language.GetAllLanguages();
				}
				return _languages;
			}

			set { Properties.Settings.Default.Languages = Serialize(value); }
		}

		public Settings()
		{
			
		}

		private string Serialize<T>(T item) where T : class
		{
			try
			{
				using (StringWriter sw = new StringWriter())
				using (XmlWriter xw = XmlWriter.Create(sw))
				{
					new XmlSerializer(typeof(T)).Serialize(xw, item);
					return sw.GetStringBuilder().ToString();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Failed to serialize object of type " + typeof(T).FullName + ": " + ex.Message);
				return "Failed to serialize";
			}
		}

		private T Deserialize<T>(string s_xml) where T : class
		{
			try
			{
				using (XmlReader xw = XmlReader.Create(new StringReader(s_xml)))
					return (T)new XmlSerializer(typeof(T)).Deserialize(xw);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Failed to deserialize object of type " + typeof(T).FullName + ": " + ex.Message);
				return default(T);
			}
		}
	}
}
