using JkhSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleConsoleApplicationSettings
{
	class Program
	{
		class MySettings : CustomSettingsBase
		{
			public const string SomeSettingName = "SomeSettingName";
			public const double SomeSettingDefaultValue = 1.1;
		}

		static void Main(string[] args)
		{
			using(MySettings settings = new MySettings())
			{
				string myValue = settings.GetSetting<string>("MySettingName1", "MySettingValueDefault1");

				settings.PutSetting("MySettingName2", "MySettingValue2");
				settings.PutSetting(MySettings.SomeSettingName, MySettings.SomeSettingDefaultValue);


				string myValue2 = settings.GetSetting<string>("MySettingName2", "MySettingValueDefault2");
				double myValueDouble2 = settings.GetSetting(MySettings.SomeSettingName, MySettings.SomeSettingDefaultValue);
			}
		}
	}
}
