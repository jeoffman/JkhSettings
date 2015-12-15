using JkhSettings;
using System;

namespace ExampleWindowsFormsApplicationSettings
{
	class MySettings : CustomSettingsBase
	{
		public const string SomeSettingName = "SomeSettingName";
		public const double SomeSettingDefaultValue = 1.1;

		public const string SplitterDistanceName = "SplitterDistanceName";
		public const int SplitterDistanceDefaultValue = 100;

		public const string TraceTextName = "TraceText";
		public const string TraceTextDefaultValue = "Trace something";

		public const string TraceLevelName = "TraceLevel";
		public const string TraceLevelDefaultValue = "Warning";

		public const string SwitchValueName = "SwitchValue";
		public const string SwitchValueDefaultValue = "Warning";

		public const string MyArrayName = "MyArray";
		public readonly string[] MyArrayDefault = { };
	}
}
