using JkhSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExampleWindowsFormsApplicationSettings
{
	public partial class FormMain : Form
	{
		private TraceSource _traceSource = new TraceSource("MyTraceSource");
		private TraceSource _traceSource2 = new TraceSource("MySecondTraceSource");

		TraceListenerRtf myListener;

		public FormMain()
		{
			InitializeComponent();
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
//			myListener = TraceListenerRtf.InstallTraceListener(richTextBoxTraces);

			comboBoxTraceLevel.DataSource = Enum.GetNames(typeof(TraceEventType));
			comboBoxSwitchValue.DataSource = Enum.GetNames(typeof(SourceLevels));

			using(MySettings settings = new MySettings())
			{
				string myValue = settings.GetSetting<string>("MySettingName1", "MySettingValueDefault1");

				settings.PutSetting("MySettingName2", "MySettingValue2");
				settings.PutSetting(MySettings.SomeSettingName, MySettings.SomeSettingDefaultValue);

				string myValue2 = settings.GetSetting<string>("MySettingName2", "MySettingValueDefault2");
				double myValueDouble2 = settings.GetSetting(MySettings.SomeSettingName, MySettings.SomeSettingDefaultValue);

				settings.PutSetting("MyIntArray", new int[] { 1, 2, 3, 4 });
				int[] myIntArray = settings.GetSetting("MyIntArray", new int[] { });

				settings.PutSetting("MyStringArray", new string[] { "one", "two", "three", "four" });
				string[] myStringArray = settings.GetSetting("MyStringArray", new string[] { "blah", "Blah" });

				settings.PutSetting("MyStringList", new List<string> { "test1", "Test2", "Test3" } );
				List<string> myStringList = settings.GetSetting("MyStringList", new List<string>());

				settings.PutSetting("MyDictionary", new SerializableDictionary<int, string> { [0]="test0", [2]="Test2", [99]="Test99" });
				SerializableDictionary<int, string> myDictionary = settings.GetSetting("MyDictionary", new SerializableDictionary<int, string>());

				settings.PutSetting("MyGuid", Guid.NewGuid());
				Guid myGuid = settings.GetSetting("MyGuid", new Guid());

				settings.PutSetting("MyDecimal", 99.571M);
				Decimal myDecimal = settings.GetSetting("MyDecimal", 0.0M);

				settings.PutSetting("MyPoint", new Point(4, 5));
				Point myPoint = settings.GetSetting("MyPoint", new Point(0, 0));

				settings.PutSetting("MySize", new Size(5, 5));
				Size mySize = settings.GetSetting("MySize", new Size(0, 0));

				settings.PutSetting("MyEnum", MyEnum.Enum3);
				MyEnum myEnum = settings.GetSetting("MyEnum", MyEnum.Value1);

				string[] myArray = settings.GetSetting(MySettings.MyArrayName, settings.MyArrayDefault);
				settings.PutSetting(MySettings.MyArrayName, new string[] { });
				string[] myArray2 = settings.GetSetting(MySettings.MyArrayName, settings.MyArrayDefault);

				settings.RestoreWindowPlacement(this);
				settings.RestoreColumnWidths(listView1);

				splitContainer1.SplitterDistance = settings.GetSetting(MySettings.SplitterDistanceName, MySettings.SplitterDistanceDefaultValue);
				textBoxTraceText.Text = settings.GetSetting(MySettings.TraceTextName, MySettings.TraceTextDefaultValue);

				comboBoxTraceLevel.SelectedItem = settings.GetSetting(MySettings.TraceLevelName, MySettings.TraceLevelDefaultValue);
				comboBoxSwitchValue.SelectedItem = settings.GetSetting(MySettings.SwitchValueName, MySettings.SwitchValueDefaultValue);

				Task t = Task.Run(async delegate
				{
					await Task.Delay(TimeSpan.FromSeconds(5));
					_traceSource2.TraceEvent(TraceEventType.Information, 57, "Second add:");
					myListener = TraceListenerRtf.InstallTraceListener(richTextBoxTraces);
					myListener.LogLevel = TraceEventType.Verbose;
					_traceSource.TraceEvent(TraceEventType.Information, 57, "Added after 5 seconds");
				});
			}
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			using(MySettings settings = new MySettings())
			{
				settings.SaveWindowPlacement(this);
				settings.SaveColumnWidths(listView1);

				settings.PutSetting(MySettings.TraceLevelName, comboBoxTraceLevel.SelectedItem);
				settings.PutSetting(MySettings.SwitchValueName, comboBoxSwitchValue.SelectedItem);

				settings.PutSetting(MySettings.SplitterDistanceName, splitContainer1.SplitterDistance);
			}
		}

		private void buttonViewSettingsFile_Click(object sender, EventArgs e)
		{
			using(MySettings settings = new MySettings())
			{
				using(Process p = new Process())
				{
					p.StartInfo.FileName = "explorer.exe";
					p.StartInfo.Arguments = string.Format("/e,/select,\"{0}\"", settings.DocumentPath);
					p.Start();
				}
			}
		}
		enum MyEnum
		{
			Value1,
			Val2,
			Enum3
		}

		private void toolStripButtonAbout_Click(object sender, EventArgs e)
		{
			using(FormAbout dlg = new FormAbout())
			{
				dlg.ShowDialog(this);
			}
		}

		private void buttonTrace_Click(object sender, EventArgs e)
		{
			TraceEventType eventTypeFromCombo = (TraceEventType)Enum.Parse(typeof(TraceEventType), comboBoxTraceLevel.SelectedItem.ToString());
			_traceSource.TraceEvent(eventTypeFromCombo, 57, textBoxTraceText.Text);
			_traceSource2.TraceEvent(eventTypeFromCombo, 57, "#2" + textBoxTraceText.Text);
		}

		private void comboBoxSwitchValue_SelectedIndexChanged(object sender, EventArgs e)
		{
			SourceLevels eventTypeFromCombo = (SourceLevels)Enum.Parse(typeof(SourceLevels), comboBoxSwitchValue.SelectedItem.ToString());
			SetTraceLevel(eventTypeFromCombo);
		}

		private void SetTraceLevel(SourceLevels eventTypeFromCombo)
		{
			TraceListenerRtf.SetTraceSourceLevel("MyTraceSource", eventTypeFromCombo);
			//throw new NotImplementedException();
		}

		private void checkBoxHookUpTraceListener_CheckedChanged(object sender, EventArgs e)
		{
			if(checkBoxHookUpTraceListener.Checked)
			{
				myListener = TraceListenerRtf.InstallTraceListener(richTextBoxTraces);
				myListener.LogLevel = TraceEventType.Verbose;
			}
			else
			{
				if(myListener != null)
				{
					TraceListenerRtf.UninstallTraceListener(myListener);
					myListener.Dispose();
					myListener = null;
				}
			}
		}
	}
}
