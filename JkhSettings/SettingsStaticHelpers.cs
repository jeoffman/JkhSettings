using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace JkhSettings
{
    public static class SettingsStaticHelpers
    {
#if !SETTINGS_NO_WINFORMS
		public static string ColumnWidthsToString(ListView lv)
		{
			StringBuilder retval = new StringBuilder();
			if (lv != null)
			{
				foreach (ColumnHeader column in lv.Columns)
				{
					if (retval.Length > 0)
						retval.Append(",");
					retval.Append(column.Width.ToString(CultureInfo.CurrentCulture));
				}
			}
			return retval.ToString();
		}
		
		public static bool RestoreColumnWidths(ListView lv, string columnWidths)
        {
			bool retval = false;
			if (!string.IsNullOrEmpty(columnWidths))
			{
				string[] widths = columnWidths.Split(',');
				if (widths.Length > 0 && !string.IsNullOrEmpty(widths[0]))
				{
					for (int count = 0; count < widths.Length; count++)
					{
						if (lv.Columns.Count > count)
						{
							int width;
							if (int.TryParse(widths[count].Trim(), out width))
								lv.Columns[count].Width = width;
							else
								lv.Columns[count].Width = -1;   //autosize
						}
						else
						{
							//Debug.Assert(false);
							break;  // too many initializers!
						}
					}
					retval = widths.Length > 0;
				}
			}
			return retval;
		}

		public static bool RestoreColumnWidths(CustomSettingsXmlRestoreColumnWidths callback, string columnWidths)
		{
			bool retval = false;
			if (!string.IsNullOrEmpty(columnWidths))
			{
				string[] widthTexts = columnWidths.Split(',');
				if (widthTexts.Length > 0)
				{
					int[] widths = new int[widthTexts.Length];
					retval = true;
					for (int count = 0; count < widthTexts.Length; count++)
					{
						if (!int.TryParse(widthTexts[count].Trim(), out widths[count]))
						{
							widths[count] = -1; // autosize
							retval = false;
						}
					}
					retval &= callback(widths);
				}
				else
				{
					callback(null);
				}
			}
			return retval;
		}

		public static bool RestoreColumnWidths(IJkhColumnWidthSettings callback, string columnWidths)
		{
			bool retval = false;
			if (!string.IsNullOrEmpty(columnWidths))
			{
				string[] widthTexts = columnWidths.Split(',');
				if (widthTexts.Length > 0 && !string.IsNullOrEmpty(widthTexts[0]))
				{
					int[] widths = new int[widthTexts.Length];
					retval = true;
					for (int count = 0; count < widthTexts.Length; count++)
					{
						if (!int.TryParse(widthTexts[count].Trim(), out widths[count]))
						{
							widths[count] = -1; // autosize
							retval = false;
						}
					}
					retval &= callback.RestoreColumnWidths(widths);
				}
				else
				{
					callback.RestoreColumnWidths(null);
				}
			}
			return retval;
		}

		public static string ColumnWidthsToString(DataGridView dataGridView)
		{
			StringBuilder retval = new StringBuilder();
			if (dataGridView != null)
			{
				foreach (DataGridViewColumn column in dataGridView.Columns)
				{
					if (retval.Length > 0)
						retval.Append(",");
					retval.Append(column.Width.ToString(CultureInfo.CurrentCulture));
				}
			}
			return retval.ToString();
		}

		public static bool RestoreColumnWidths(DataGridView dataGridView, string columnWidths)
        {
			bool retval = false;
			if(!string.IsNullOrEmpty(columnWidths))
			{
				string[] widths = columnWidths.Split(',');
				for (int count = 0; count < widths.Length; count++)
				{
					if (dataGridView.Columns.Count > count)
					{
						int width;
						if (int.TryParse(widths[count].Trim(), out width))
							dataGridView.Columns[count].Width = width;
					}
				}
				retval = widths.Length > 0;
			}
			return retval;
		}

		public static string SaveWindowPlacement(Control target)
		{
			Rectangle rect;
			Form formTarget = target as Form;
			if (formTarget != null)
			{
				if (formTarget.WindowState == FormWindowState.Minimized)
					rect = formTarget.RestoreBounds;
				else
					rect = target.Bounds;
			}
			else
			{
				rect = target.Bounds;
			}
			RectangleConverter converter = new RectangleConverter();
			return converter.ConvertToString(rect);
		}
		
		public static void RestoreWindowPlacement(Control target, string settingString)
        {
			if (!string.IsNullOrEmpty(settingString))
			{
				RectangleConverter converter = new RectangleConverter();
				Rectangle formBounds = (Rectangle)converter.ConvertFromString(settingString);
				if (!formBounds.IsEmpty)
				{
					// Get the working area of the monitor that contains this rectangle
					//  (in case it's a multi-display system)
					Rectangle workingArea = Screen.GetWorkingArea(formBounds);

					// If the bounds are outside of the screen's work area, move the
					// formTarget so it's not outside of the work area. This can happen if the
					// user changes their resolution and we then restore the application
					// into its position — it may be off screen and then they can't see it
					// or move it.
					if (formBounds.Left < workingArea.Left)
						formBounds.Location = new Point(workingArea.Location.X, formBounds.Location.Y);
					if (formBounds.Top < workingArea.Top)
						formBounds.Location = new Point(formBounds.Location.X, workingArea.Location.Y);
					if (formBounds.Right > workingArea.Right)
						formBounds.Location = new Point(formBounds.X - (formBounds.Right - workingArea.Right), formBounds.Location.Y);
					if (formBounds.Bottom > workingArea.Bottom)
						formBounds.Location = new Point(formBounds.X, formBounds.Y - (formBounds.Bottom - workingArea.Bottom));

					Form formTarget = target as Form;
					if (formTarget != null)
					{
						switch (formTarget.FormBorderStyle)
						{
							case FormBorderStyle.Sizable:
							case FormBorderStyle.SizableToolWindow:
								target.Bounds = formBounds;
								break;
							default:
								formBounds.Width = target.Bounds.Width;
								formBounds.Height = target.Bounds.Height;
								target.Bounds = formBounds;
								break;
						}
					}
					else
					{
						target.Bounds = formBounds;
					}
				}
			}
		}

		public static string SaveRelativeWindowPlacement(Form formTarget)
		{
			Rectangle bounds = formTarget.Bounds;
			bounds.Offset(formTarget.Owner.Bounds.X, -formTarget.Owner.Bounds.Y);
			RectangleConverter converter = new RectangleConverter();
			return converter.ConvertToString(bounds);
		}
		
		public static void RestoreRelativeWindowPlacement(Form formTarget, string settingString)
        {
			if (!string.IsNullOrEmpty(settingString))
			{
				RectangleConverter converter = new RectangleConverter();
				Rectangle formBounds = (Rectangle)converter.ConvertFromString(settingString);
				if (!formBounds.IsEmpty)
				{
					formBounds.Offset(+formTarget.Owner.Bounds.X, +formTarget.Owner.Bounds.Y);

					// Get the working area of the monitor that contains this rectangle
					//  (in case it’s a multi-display system)
					Rectangle workingArea = Screen.GetWorkingArea(formBounds);

					// If the bounds are outside of the screen’s work area, move the
					// formTarget so it’s not outside of the work area. This can happen if the
					// user changes their resolution and we then restore the application
					// into its position — it may be off screen and then they can’t see it
					// or move it.
					if (formBounds.Left < workingArea.Left)
						formBounds.Location = new Point(workingArea.Location.X, formBounds.Location.Y);
					if (formBounds.Top < workingArea.Top)
						formBounds.Location = new Point(formBounds.Location.X, workingArea.Location.Y);
					if (formBounds.Right > workingArea.Right)
						formBounds.Location = new Point(formBounds.X - (formBounds.Right - workingArea.Right), formBounds.Location.Y);
					if (formBounds.Bottom > workingArea.Bottom)
						formBounds.Location = new Point(formBounds.X, formBounds.Y - (formBounds.Bottom - workingArea.Bottom));

					formTarget.Bounds = formBounds;
				}
			}
		}
#endif	// !SETTINGS_NO_WINFORMS
	}
}
