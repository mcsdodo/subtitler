using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;
using Subtitler.Desktop.Annotations;
using Subtitler.Desktop.Helpers;
using Subtitler.Desktop.ViewModels;

namespace Subtitler.Desktop.Models
{
	[Serializable]
	public class LanguageCollection : ObservableCollection<Language>
	{
		public LanguageCollection(){}


		public delegate void SaveCollectionHandler(object sender);
		public event SaveCollectionHandler OnCollectionSave = delegate { };

		//assigned event handlers for saving collection on Use parameter changes
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				foreach (Language item in e.NewItems)
				{
					item.UsePropertyChanged += UseAttributeChanged;
				}
			}

			base.OnCollectionChanged(e);
		}

		void UseAttributeChanged(object sender)
		{
			OnCollectionSave(this);
		}

	}

	[Serializable]
	public class Language
	{
		public delegate void SaveCollectionHandler(object sender);
		public event SaveCollectionHandler UsePropertyChanged;

		public Language(){}

		public static LanguageCollection GetAllLanguages()
		{
			return Application.Current.Resources["LanguageCollection"] as LanguageCollection;
		}
		
		public string Id { get; set; }
		public string Name { get; set; }
		public string ImgPos { get; set; }
		public string ShortId { get; set; }

		private bool _use;

		public bool Use
		{
			get { return _use; }
			set
			{
				if (_use != value)
				{
					_use = value;
					if (UsePropertyChanged != null) UsePropertyChanged(this);
				}
			}
		}

		public Int32Rect Rect
		{
			get { return new Int32Rect(int.Parse(ImgPos), 0, 18, 12); }
		}

	}
}
