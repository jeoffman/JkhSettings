using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;


namespace JkhSettings
{
	public static class RichTextBoxHelper
	{
		public static void AppendLine(RichTextBox richTextBox, string format, params object[] values)
		{
			AppendLine(richTextBox, Color.Empty, Color.Empty, format, values);
// 			try
// 			{
// 				string text = string.Format(CultureInfo.CurrentCulture, format, values);
// 				if(!text.EndsWith("\n", StringComparison.Ordinal))
// 					text += "\n";
// 				AppendInternal(richTextBox, Color.Empty, Color.Empty, text);
// 			}
// 			catch(FormatException exc)
// 			{
// 			}
		}

		public static void AppendLine(RichTextBox richTextBox, Color colorFore, string format, params object[] values)
		{
			AppendLine(richTextBox, colorFore, Color.Empty, format, values);
// 			string text = string.Format(CultureInfo.CurrentCulture, format, values);
// 			if(!text.EndsWith("\n", StringComparison.Ordinal))
// 				text += "\n";
// 			AppendInternal(richTextBox, colorFore, Color.Empty, text);
		}

		public static void AppendLine(RichTextBox richTextBox, Color colorFore, Color colorBack, string format, params object[] values)
		{
			try
			{
				string text = string.Format(CultureInfo.CurrentCulture, format, values);
				if(!text.EndsWith("\n", StringComparison.Ordinal))
					text += "\n";
				AppendInternal(richTextBox, colorFore, colorBack, text);
			}
			catch(FormatException /*exc*/)
			{
				string text = string.Format("***RichTextBoxHelper.AppendLine: FormatException on {0}", format);
				AppendInternal(richTextBox, colorFore, colorBack, text);
			}
		}

		public static void AppendLine(RichTextBox richTextBox, string text)
		{
			if(text == null)
				throw new ArgumentNullException("text");
			if(!text.EndsWith("\n", StringComparison.Ordinal))
				text += "\n";
			AppendInternal(richTextBox, Color.Empty, Color.Empty, text);
		}

		public static void AppendLine(RichTextBox richTextBox, Color colorFore, string text)
		{
			if(text == null)
				throw new ArgumentNullException("text");
			if(!text.EndsWith("\n", StringComparison.Ordinal))
				text += "\n";
			AppendInternal(richTextBox, colorFore, Color.Empty, text);
		}

		public static void Append(RichTextBox richTextBox, string format, params object[] values)
		{
			Append(richTextBox, Color.Empty, Color.Empty, format, values);
		}

		public static void Append(RichTextBox richTextBox, Color colorFore, string format, params object[] values)
		{
			Append(richTextBox, colorFore, Color.Empty, format, values);
		}

		public static void Append(RichTextBox richTextBox, Color colorFore, Color colorBack, string format, params object[] values)
		{
			AppendStyle(richTextBox, 0, colorFore, colorBack, format, values);
		}

		public static void Append(RichTextBox richTextBox, string text)
		{
			AppendInternal(richTextBox, Color.Empty, Color.Empty, text);
		}

		public static void Append(RichTextBox richTextBox, Color colorFore, string text)
		{
			AppendInternal(richTextBox, colorFore, Color.Empty, text);
		}

		public static void AppendStyle(RichTextBox richTextBox, FontStyle style, Color colorFore, Color colorBack, string format, params object[] values)
		{
			try
			{
				string text;
				if(values != null && values.Length > 0)
					text = string.Format(CultureInfo.CurrentCulture, format, values);
				else
					text = format;
				AppendInternal(richTextBox, colorFore, colorBack, style, text);
			}
			catch(FormatException /*exc*/)
			{
				string text = string.Format("***RichTextBoxHelper.AppendStyle: FormatException on {0}", format);
				AppendInternal(richTextBox, colorFore, colorBack, text);
			}
		}


		private const int _maxConsoleTextLength = 1024 * 1024;
		private static void AppendInternal(RichTextBox richTextBox, Color colorFore, Color colorBack, string text)
		{
			if(richTextBox == null)
				throw new ArgumentNullException("richTextBox");
			AppendInternal(richTextBox, colorFore, colorBack, richTextBox.Font.Style, text);
		}
		public  static void AppendInternal(RichTextBox richTextBox, Color colorFore, Color colorBack, FontStyle newStyle, string text)
		{
			if(richTextBox != null && !string.IsNullOrEmpty(text) && !richTextBox.IsDisposed)
			{
				if(richTextBox.InvokeRequired)
				{
					richTextBox.BeginInvoke(new MethodInvoker(delegate() { AppendInternal(richTextBox, colorFore, colorBack, newStyle, text); }));
				}
				else
				{
					lock(richTextBox)
					{
						richTextBox.SuspendLayout();

						//Truncate as necessary
						if(richTextBox.Text.Length + text.Length > _maxConsoleTextLength)
						{
							int truncateLength = _maxConsoleTextLength / 4;
							int endmarker = richTextBox.Text.IndexOf('\n', truncateLength) + 1;
							if(endmarker < truncateLength)
								endmarker = truncateLength;
							richTextBox.Select(0, endmarker);
							richTextBox.Cut();
						}

						int originalTextEnd = richTextBox.Text.Length;
						richTextBox.AppendText(text);
						richTextBox.Select(originalTextEnd, text.Length);
						if(colorFore != Color.Empty)
							richTextBox.SelectionColor = colorFore;
						if(colorBack != Color.Empty)
							richTextBox.SelectionBackColor = colorBack;

						//if(newStyle != richTextBox.Font.Style)
						richTextBox.SelectionFont = new Font(richTextBox.Font, newStyle);

						richTextBox.SelectionLength = 0;
						richTextBox.ScrollToCaret();
						richTextBox.ResumeLayout();
						richTextBox.Update();
					}
				}
			}
		}
	}
}
