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
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
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

				settings.RestoreWindowPlacement(this);
				settings.RestoreColumnWidths(listView1);
            }
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			using(MySettings settings = new MySettings())
			{
				settings.SaveWindowPlacement(this);
				settings.SaveColumnWidths(listView1);
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
	}

	class MySettings : CustomSettingsBase
	{
		public const string SomeSettingName = "SomeSettingName";
		public const double SomeSettingDefaultValue = 1.1;

	}

}
