using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Subtitler.Desktop.Controls
{
	public class SubtitlerListView : ListView
	{
		private bool _multipleSelectOnLeftClick = false;

		public bool MultipleSelectOnLeftClick
		{
			get { return _multipleSelectOnLeftClick; }
			set { _multipleSelectOnLeftClick = value; }
		}

		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (_multipleSelectOnLeftClick)
			{
				if (e.LeftButton == MouseButtonState.Pressed)
				{
					var row = GetVisualParentByType((FrameworkElement)e.OriginalSource, typeof(ListViewItem)) as ListViewItem;
					if (row != null)
					{
						row.IsSelected = !row.IsSelected;
						e.Handled = true;
					}
				}
			}

			base.OnPreviewMouseLeftButtonDown(e);
		}

		protected override void OnMouseEnter(MouseEventArgs e)
		{
			if (_multipleSelectOnLeftClick)
			{
				if (e.OriginalSource is ListViewItem && e.LeftButton == MouseButtonState.Pressed)
				{
					var row = e.OriginalSource as ListViewItem;
					row.IsSelected = !row.IsSelected;
					e.Handled = true;
				}
			}
			base.OnMouseEnter(e);
		}

		private static DependencyObject GetVisualParentByType(DependencyObject startObject, Type type)
		{
			DependencyObject parent = startObject;
			while (parent != null)
			{
				if (type.IsInstanceOfType(parent))
					break;
				else
					parent = VisualTreeHelper.GetParent(parent);
			}

			return parent;
		} 
	}
}
