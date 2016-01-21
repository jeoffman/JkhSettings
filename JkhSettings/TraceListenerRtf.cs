using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;


namespace JkhSettings
{
	public class TraceListenerRtf : TraceListener
	{
		private readonly static TraceSource _traceSource = new TraceSource("TraceListenerRtf", SourceLevels.Error);

		RichTextBox RichTextBoxOutput = null;
		Dictionary<TraceEventType, Color> eventColor = new Dictionary<TraceEventType, Color>();
		public bool Enabled { get; set; }
		public TraceEventType LogLevel { get; set; }

		public static TraceListenerRtf InstallTraceListener(RichTextBox richTextBoxOutput)
		{
			TraceListenerRtf newTraceListener = new TraceListenerRtf(richTextBoxOutput);
			Trace.Listeners.Add(newTraceListener);

			AttachToAllTraceSources(newTraceListener);

			return newTraceListener;
		}

		//based on: http://stackoverflow.com/questions/3303126/how-to-get-the-value-of-private-field-in-c
		//		and http://stackoverflow.com/questions/23664573/tracesource-and-tracelistener-quietly-fail-to-do-anything
		// safe to call multiple times
		private static void AttachToAllTraceSources(TraceListener yourListener)
		{
			List<string> sourceList = new List<string>();

			TraceSource ts = new TraceSource("foo");
			List<WeakReference> list = (List<WeakReference>)GetInstanceField(typeof(TraceSource), ts, "tracesources");
			for(int count = 0; count < list.Count; count++)
			{
				WeakReference weakReference = list[count];
				if(weakReference.IsAlive)
				{
					TraceSource source = (weakReference.Target as TraceSource);
					if(source != null && source.Name != "foo")
					{
						if(!source.Listeners.Contains(yourListener))	//This works after overriding Equals()
						{
							source.Listeners.Add(yourListener);
							sourceList.Add(source.Name);
						}
					}
				}
			}
			
			_traceSource.TraceEvent(TraceEventType.Information, 57, "Jkh.TraceListenerRtf is listening to : {0}", string.Join(",",sourceList));
		}

		public static void UninstallTraceListener(TraceListenerRtf myListener)
		{
			RemoveFromAllTraceSources(myListener);
		}

		private static void RemoveFromAllTraceSources(TraceListener yourListener)
		{
			List<string> sourceList = new List<string>();

			TraceSource ts = new TraceSource("foo");
			List<WeakReference> list = (List<WeakReference>)GetInstanceField(typeof(TraceSource), ts, "tracesources");
			for(int count = 0; count < list.Count; count++)
			{
				WeakReference weakReference = list[count];
				if(weakReference.IsAlive)
				{
					TraceSource source = (weakReference.Target as TraceSource);
					if(source != null && source.Name != "foo")
					{
						if(source.Listeners.Contains(yourListener))    //This works after overriding Equals()
						{
							source.Listeners.Remove(yourListener);
							sourceList.Add(source.Name);
						}
					}
				}
			}

			_traceSource.TraceEvent(TraceEventType.Information, 57, "Jkh.TraceListenerRtf is removed from : {0}", string.Join(",", sourceList));
		}

		#region Override Equals et al
		public override bool Equals(object obj)
		{
			bool retval = false;
			TraceListenerRtf item = obj as TraceListenerRtf;

			if(item != null)
				retval = (this.RichTextBoxOutput.Equals(item.RichTextBoxOutput));

			return retval;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion Override Equala et al

		internal static object GetInstanceField(Type type, object instance, string fieldName)
		{
			BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
			FieldInfo field = type.GetField(fieldName, bindFlags);
			return field.GetValue(instance);
		}

		public TraceListenerRtf()
		{
			Init();
		}

		public TraceListenerRtf(string initializeData)
		{
			Init();
		}

		public TraceListenerRtf(RichTextBox richTextBoxOutput)
		{
			RichTextBoxOutput = richTextBoxOutput;

			Init();
		}

		private void Init()
		{
			Enabled = true;

			LogLevel = TraceEventType.Warning;

            eventColor.Add(TraceEventType.Verbose, Color.DarkGray);
            eventColor.Add(TraceEventType.Information, Color.Gray);
            eventColor.Add(TraceEventType.Warning, Color.OrangeRed);
            eventColor.Add(TraceEventType.Error, Color.DarkRed);
            eventColor.Add(TraceEventType.Critical, Color.Red);
            eventColor.Add(TraceEventType.Start, Color.DarkCyan);
            eventColor.Add(TraceEventType.Stop, Color.DarkCyan);
		}

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
			if(data != null && eventType < LogLevel)
				AppendLine(eventColor[eventType], data.ToString());
			else
				AppendLine(eventColor[eventType], "<TraceData = NULL>");
		}

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
			if(data != null && eventType <= LogLevel)
				AppendLine(eventColor[eventType], data.ToString());
			else
				AppendLine(eventColor[eventType], "<TraceData[] = NULL>");
		}

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
			if(eventType <= LogLevel)
				this.TraceEvent(eventCache, source, eventType, id, string.Empty);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
			if(eventType <= LogLevel)
				AppendLine(eventColor[eventType], message);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
			if(eventType <= LogLevel)
			{
				string message = string.Empty;
				if (args == null)
				{
					message = format;
				}
				else
				{
					message = string.Format(CultureInfo.InvariantCulture, format, args);
				}
				this.TraceEvent(eventCache, source, eventType, id, message);
			}
        }

        public override void Write(string message)
        {
			AppendLine(message);
        }

        public override void WriteLine(string message)
        {
            this.Write(message);
        }

        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            string messageToSend = string.Format(CultureInfo.InvariantCulture, "Activity: {0} | {1}", relatedActivityId, message);
			if(TraceEventType.Transfer > LogLevel)
				this.TraceEvent(eventCache, source, TraceEventType.Transfer, id, messageToSend);
        }


		public void AppendLine(string format, params object[] values)
		{
			AppendLine(Color.Empty, Color.Empty, format, values);
		}

		public void AppendLine(Color colorFore, string format, params object[] values)
		{
			AppendLine(colorFore, Color.Empty, format, values);
		}

		public void AppendLine(Color colorFore, Color colorBack, string format, params object[] values)
		{
			try
			{
				string text = string.Format(CultureInfo.CurrentCulture, format, values);
				if(!text.EndsWith("\n", StringComparison.Ordinal))
					text += "\n";
				AppendInternal(colorFore, colorBack, text);
			}
			catch(FormatException /*exc*/)
			{
				string text = string.Format("***RichTextBoxHelper.AppendLine: FormatException on {0}", format);
				AppendInternal(colorFore, colorBack, text);
			}
		}

		public void AppendLine(string text)
		{
			if(text == null)
				throw new ArgumentNullException("text");
			if(!text.EndsWith("\n", StringComparison.Ordinal))
				text += "\n";
			AppendInternal(Color.Empty, Color.Empty, text);
		}

		public void AppendLine(Color colorFore, string text)
		{
			if(text == null)
				throw new ArgumentNullException("text");
			if(!text.EndsWith("\n", StringComparison.Ordinal))
				text += "\n";
			AppendInternal(colorFore, Color.Empty, text);
		}

		public void Append(string format, params object[] values)
		{
			Append(Color.Empty, Color.Empty, format, values);
		}

		public void Append(Color colorFore, string format, params object[] values)
		{
			Append(colorFore, Color.Empty, format, values);
		}

		public void Append(Color colorFore, Color colorBack, string format, params object[] values)
		{
			AppendStyle(0, colorFore, colorBack, format, values);
		}

		public void Append(string text)
		{
			AppendInternal(Color.Empty, Color.Empty, text);
		}

		public void Append(Color colorFore, string text)
		{
			AppendInternal(colorFore, Color.Empty, text);
		}

		public void AppendStyle(FontStyle style, Color colorFore, Color colorBack, string format, params object[] values)
		{
			try
			{
				string text = string.Format(CultureInfo.CurrentCulture, format, values);
				AppendInternal(colorFore, colorBack, style, text);
			}
			catch(FormatException /*exc*/)
			{
				string text = string.Format("***RichTextBoxHelper.AppendStyle: FormatException on {0}", format);
				AppendInternal(colorFore, colorBack, text);
			}
		}

		private void AppendInternal(Color colorFore, Color colorBack, string text)
		{
			if(RichTextBoxOutput != null)
				AppendInternal(colorFore, colorBack, RichTextBoxOutput.Font.Style, text);
		}

		private void AppendInternal(Color colorFore, Color colorBack, FontStyle newStyle, string text)
		{
			if(Enabled)
			{
				RichTextBoxHelper.AppendStyle(RichTextBoxOutput, newStyle, colorFore, colorBack, DateTime.Now.ToString(CultureInfo.CurrentCulture) + " - " + text);
			}
		}




		public static void SetTraceSourceLevel(string traceSourceName, SourceLevels yourLevel)
		{
			TraceSource ts = new TraceSource("foo");
			List<WeakReference> list = (List<WeakReference>)GetInstanceField(typeof(TraceSource), ts, "tracesources");
			for(int count = 0; count < list.Count; count++)
			{
				WeakReference weakReference = list[count];
				if(weakReference.IsAlive)
				{
					TraceSource source = (weakReference.Target as TraceSource);
					if(source != null && source.Name == traceSourceName)
					{
						source.Switch.Level = yourLevel;
					}
				}
			}
		}

	}
}
