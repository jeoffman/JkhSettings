﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using JkhSettings;

namespace Jkh.CustomSettingsXml.Tests
{
	[TestClass()]
	public class CustomSettingsBaseTests
	{
		class MySettingsDefault : CustomSettingsBase
		{
			public const string SomeSettingName = "SomeSettingName";
			public const double SomeSettingDefaultValue = 1.1;
		}
		class MySettingsUser : CustomSettingsBase
		{
			public MySettingsUser() : base(true)
			{
			}
            public const string SomeSettingName = "SomeSettingName";
			public const double SomeSettingDefaultValue = 1.1;
		}


		[TestMethod()]
		public void CustomSettingsBaseTest()
		{
			using(MySettingsDefault settings = new MySettingsDefault())
			{
			}

			using(MySettingsUser settings = new MySettingsUser())
			{
			}
		}

		[TestMethod()]
		public void CloneTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void DisposeTest()
		{
			MySettingsUser settings = new MySettingsUser();
			settings.PutSetting("MyInt", 1);
			settings.PutSetting("MyBool", true);
			settings.PutSetting("MyString", "text");
			settings.PutSetting("MyDouble", 1.0);
			settings.PutSetting("MyDecimal", 1M);
			settings.PutSetting("MyArray", new int[] { 1,2,3,4 } );
			settings.PutSetting("MyColor", Color.Red);
			settings.PutSetting("MyGUID", Guid.NewGuid());
			settings.Dispose();
		}

		[TestMethod()]
		public void GetSettingTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void PutSettingTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void LoadTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void SaveTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void SaveTest1()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void ClearTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void SaveWindowSizeTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void RestoreWindowSizeTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void FormTargetFormClosingTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void SaveWindowPlacementTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void RestoreWindowPlacementTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void SaveRelativeWindowPlacementTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void RestoreRelativeWindowPlacementTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void SaveItemStringsTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void LoadItemStringsTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void ColumnWidthsToStringTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void SaveColumnWidthsTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void RestoreColumnWidthsTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void AutoSizeColumnWidthsTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void SaveColumnWidthsTest1()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void RestoreColumnWidthsTest1()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void SaveColumnWidthsTest2()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void RestoreColumnWidthsTest2()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void LoadItemStringsTest1()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void SaveItemStringsTest1()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void ColumnWidthsToStringTest1()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void SaveColumnWidthsTest3()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void RestoreColumnWidthsTest3()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void BindWindowPlacementTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void BindTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void BindTest1()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void GuiToDocTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void DocToGuiTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void GetSqlConnectionStringTest()
		{
			//Assert.Fail();	TODO
		}

		[TestMethod()]
		public void PutSqlConnectionStringTest()
		{
			//Assert.Fail();	TODO
		}
	}
}