// #define SETTINGS_NO_SQL_ENCRYPT	//--NO! use the project build settings instead!
// #define SETTINGS_NO_WINFORMS	//--NO! use the project build settings instead!
// #define SETTINGS_NO_DRAWING		//--NO! use the project build settings instead!
using System;
using System.Collections;   //ArrayList
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;   //Debug
using System.Globalization; //CultureInfo
using System.IO;
using System.Text;  //StringBuilder
using System.Xml;
using System.Xml.Serialization;

#if !SETTINGS_NO_SQL_ENCRYPT
using System.Data.SqlClient;
#endif

#if !SETTINGS_NO_WINFORMS
using System.Windows.Forms;
using System.Xml.XPath;
#endif

#if !SETTINGS_NO_DRAWING
using System.Drawing;   //Size
#endif

//based on: http://www.codeproject.com/Articles/15530/Quick-and-Dirty-Settings-Persistence-with-XML


namespace JkhSettings
{
    public class CustomSettingsBase : IDisposable
	{
		private TraceSource _traceSource = new TraceSource("JkhSettings");

		private const string _MagicalDefualtStringFromHell = "!@#$%^&*()0-987654321~`";
		private const string SettingsFileName = "settings.xml";
		private const string EmptySettingsFile = "<settings></settings>";
		private const string SettingsNode = "settings/";
		private const string WindowPlacementSuffix = "_Bounds";
		private const string RelativeWindowPlacementSuffix = "_RelativeBounds";
		private const string SizeSuffix = "_Size";
		private const string ColumnWidthsSuffix = "_ColumnWidths";

		public static string DefaultUserSettingsFilePath
		{
			get
			{
				string workingDirectory = Directory.GetParent(Application.LocalUserAppDataPath).ToString();
				string settingsDocumentPath = Path.Combine(workingDirectory, SettingsFileName);
				return settingsDocumentPath;
			}
		}

		XmlDocument xmlDocument = new XmlDocument();
#if !SETTINGS_NO_WINFORMS
		Dictionary<string, Control> Bindings = new Dictionary<string, Control>(StringComparer.OrdinalIgnoreCase);
#endif
		Dictionary<string, object> BindingDefaults = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
		bool AutoSaveSettingsMode = false;
		string DefaultSubpath = string.Empty;
		//FileSystemWatcher _watcher;

		public string DocumentPath { get; set; }

		public CustomSettingsBase(bool userSettings)
		{
			CommonUserConstructor(userSettings, null);
		}

		protected CustomSettingsBase(bool userSettings, string defaultSubpath)
		{
			CommonUserConstructor(userSettings, defaultSubpath);
		}

		protected void CommonUserConstructor(bool userSettings, string defaultSubpath)
		{
			string workingDirectory;
#if !SETTINGS_NO_WINFORMS
            if(userSettings)
				workingDirectory = Directory.GetParent(Application.LocalUserAppDataPath).ToString();
			else
				workingDirectory = Directory.GetParent(Application.ExecutablePath).ToString();
#else
			workingDirectory = Directory.GetCurrentDirectory();
#endif	//!SETTINGS_NO_WINFORMS

			string settingsDocumentPath = Path.Combine(workingDirectory, SettingsFileName);
			try
			{
				Load(settingsDocumentPath);	// will set _autoSaveSettingsMode for us automatically!
				AutoSaveSettingsMode = true;	// override AFTER we do the "load" (hack!)
			}
			catch(Exception exc)
			{
				_traceSource.TraceEvent(TraceEventType.Error, 57, "CustomSettingsBase() Exception: " + exc.Message);

				xmlDocument.LoadXml(EmptySettingsFile);
				DocumentPath = settingsDocumentPath;
				AutoSaveSettingsMode = true;	// override AFTER we do the "load" (hack!)
			}
			DefaultSubpath = defaultSubpath;
			if(!string.IsNullOrEmpty(DefaultSubpath))
			{
				if(DefaultSubpath[DefaultSubpath.Length - 1] != '/')
					DefaultSubpath += '/';
			}
		}

		// protected constructors, derive your own class from this!
		protected CustomSettingsBase()
		{
			string settingsDocumentPath;
#if !SETTINGS_NO_WINFORMS
			settingsDocumentPath = Path.Combine(Application.StartupPath, SettingsFileName);
#else
			settingsDocumentPath = Path.Combine(Directory.GetCurrentDirectory(), SettingsFileName);
#endif  //!SETTINGS_NO_WINFORMS


			try
			{
				Load(settingsDocumentPath);	// will set _autoSaveSettingsMode for us automatically!
				AutoSaveSettingsMode = true;	// override AFTER we do the "load" (hack!)
			}
			catch(FileNotFoundException exc)
			{
				_traceSource.TraceEvent(TraceEventType.Error, 57, "CustomSettingsBase() Exception: " + exc.Message);

				xmlDocument.LoadXml(EmptySettingsFile);
				DocumentPath = settingsDocumentPath;
				AutoSaveSettingsMode = true;	// override AFTER we do the "load" (hack!)
			}
		}

		private XmlNode CreateMissingNode(string xmlPath)
		{
			string[] xPathSections = xmlPath.Split('/');
			StringBuilder currentXPath = new StringBuilder();
			XmlNode testNode = null;
			XmlNode currentNode = xmlDocument.SelectSingleNode("settings");
			if(currentNode == null)
			{
				xmlDocument.LoadXml(EmptySettingsFile);
				currentNode = xmlDocument.SelectSingleNode("settings");
			}
			foreach(string xPathSection in xPathSections)
			{
				currentXPath.Append(xPathSection);

				try
				{
					testNode = xmlDocument.SelectSingleNode(currentXPath.ToString());
				}
				catch(XPathException exc)
				{
					_traceSource.TraceEvent(TraceEventType.Error, 57, $"CreateMissingNode({xmlPath}) Exception: {exc.Message}");
				}

				if(testNode == null)
				{
					currentNode.InnerXml += "<" + xPathSection + "></" + xPathSection + ">";
				}

				try
				{
					currentNode = xmlDocument.SelectSingleNode(currentXPath.ToString());
				}
				catch(XPathException exc)
				{
					_traceSource.TraceEvent(TraceEventType.Error, 57, $"CreateMissingNode({xmlPath}) Exception: {exc.Message}");
				}
				currentXPath.Append("/");
			}
			return currentNode;
		}

		public XmlDocument Clone()
		{
			return (XmlDocument)xmlDocument.Clone();
		}

		#region IDisposable Members
		protected virtual void Dispose(bool disposing)
		{
			if(disposing)
			{
#if !SETTINGS_NO_WINFORMS
				if(Bindings != null)
				{
					Bindings.Clear();
					Bindings = null;
				}
#endif  //!SETTINGS_NO_WINFORMS
				xmlDocument = null;
				DocumentPath = null;
				DefaultSubpath = null;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
#endregion

#region Generics
		public T GetSetting<T>(string xmlPath, T defaultValue)
		{
			T retval;
			try
			{
				xmlPath = NormalizeXmlPathJkh(xmlPath);

				XmlNode xmlNode = xmlDocument.SelectSingleNode(SettingsNode + xmlPath);
				if(xmlNode != null)
				{
					Type valueType = typeof(T);
	//				if(valueType.ToString().Contains("Dictionary")) //if this one FIRST because a dictionary is a generic
	//					retval = GetDictionary(xmlNode.InnerText, defaultValue);
	//				else 
					try
					{
						if(valueType.IsArray || valueType.IsGenericType)
							retval = (T)GetArray<T>(xmlNode.InnerText);
						else
							retval = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(xmlNode.InnerText);
					}
					catch(InvalidOperationException exc)
					{
						_traceSource.TraceEvent(TraceEventType.Error, 57, $"GetSetting<{valueType}> Exception: {exc.Message}");
						retval = defaultValue;
					}
				}
				else
				{
					retval = defaultValue;
				}
			}
			catch(XPathException exc)
			{
				_traceSource.TraceEvent(TraceEventType.Error, 57, $"GetSetting({xmlPath}) Exception: {exc.Message}");
				retval = defaultValue;
			}
			return retval;
		}

		public T GetEncryptedSetting<T>(string password, string xmlPath, T defaultValue)
		{
			T retval;
			try
			{
				xmlPath = NormalizeXmlPathJkh(xmlPath);

				XmlNode xmlNode = xmlDocument.SelectSingleNode(SettingsNode + xmlPath);
				if(xmlNode != null)
				{
					Type valueType = typeof(T);
					try
					{
						string encryptedText = xmlNode.InnerText;
						password = MakeSureSaltIsLongEnough(password);

						string decryptedText = AesThenHmac.SimpleDecryptWithPassword(encryptedText, password);

						if(valueType.IsArray || valueType.IsGenericType)
							retval = (T)GetArray<T>(decryptedText);
						else
							retval = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(decryptedText);
					}
					catch(InvalidOperationException exc)
					{
						_traceSource.TraceEvent(TraceEventType.Error, 57, $"GetSetting<{valueType}>({xmlPath}) Exception: {exc.Message}");
						retval = defaultValue;
					}
				}
				else
				{
					retval = defaultValue;
				}
			}
			catch(XPathException exc)
			{
				_traceSource.TraceEvent(TraceEventType.Error, 57, $"GetSetting({xmlPath}) Exception: {exc.Message}");
				retval = defaultValue;
			}
			catch(ArgumentNullException exc)
			{
				_traceSource.TraceEvent(TraceEventType.Error, 57, $"GetSetting({xmlPath}) Exception: {exc.Message}");
				retval = defaultValue;
			}
			return retval;
		}

		private static string MakeSureSaltIsLongEnough(string password)
		{
			string retval = password;
			if(retval.Length < 12)
			{
				for(int countUpTo12 = 1; countUpTo12 < 12; countUpTo12++)
				{
					retval += retval;
					if(retval.Length > 12)
						break;
				}
			}
			return retval;
		}

		public void PutSetting(string xmlPath, object value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));

			xmlPath = NormalizeXmlPathJkh(xmlPath);

			XmlNode xmlNode = null;
			try
			{
				xmlNode = xmlDocument.SelectSingleNode(SettingsNode + xmlPath);
			}
			catch(XPathException exc)
			{
				_traceSource.TraceEvent(TraceEventType.Error, 57, $"PutSetting({xmlPath}) Exception: {exc.Message}");
			}

			if(xmlNode == null)
				xmlNode = CreateMissingNode(SettingsNode + xmlPath);

			Type valueType = value.GetType();
//			if(valueType.ToString().Contains("Dictionary"))	//FIRST if because a dictionary is a generic
//				xmlNode.InnerText = PutDictionary((IDictionary)value);
//			else
			if(valueType.IsArray || valueType.IsGenericType)
				xmlNode.InnerText = PutArray(value);
			else
				xmlNode.InnerText = TypeDescriptor.GetConverter(valueType).ConvertToString(value);
			if(AutoSaveSettingsMode)
			{
				//_watcher.EnableRaisingEvents = false;
				xmlDocument.Save(DocumentPath);
				//_watcher.EnableRaisingEvents = true;
			}
		}

		public void PutEncryptedSetting(string password, string xmlPath, object value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));

			xmlPath = NormalizeXmlPathJkh(xmlPath);

			XmlNode xmlNode = null;
			try
			{
				xmlNode = xmlDocument.SelectSingleNode(SettingsNode + xmlPath);
			}
			catch(XPathException exc)
			{
				_traceSource.TraceEvent(TraceEventType.Error, 57, $"PutSetting({xmlPath}) Exception: {exc.Message}");
			}

			if(xmlNode == null)
				xmlNode = CreateMissingNode(SettingsNode + xmlPath);

			string unencryptedText;
			Type valueType = value.GetType();
			if(valueType.IsArray || valueType.IsGenericType)
				unencryptedText = PutArray(value);
			else
				unencryptedText = TypeDescriptor.GetConverter(valueType).ConvertToString(value);

			password = MakeSureSaltIsLongEnough(password);

			xmlNode.InnerText = AesThenHmac.SimpleEncryptWithPassword(unencryptedText, password);

			if(AutoSaveSettingsMode)
			{
				//_watcher.EnableRaisingEvents = false;
				xmlDocument.Save(DocumentPath);
				//_watcher.EnableRaisingEvents = true;
			}
		}

		private string NormalizeXmlPathJkh(string xmlPath)
		{
			if(xmlPath == null)
				throw new ArgumentNullException(nameof(xmlPath));

			xmlPath = XmlConvert.EncodeLocalName(xmlPath);

			return xmlPath;
		}

		private static T GetArray<T>(string innerText)
		{
			T retval;
			var serializer = new XmlSerializer(typeof(T));
			using(TextReader reader = new StringReader(innerText))    //HEY! this might throw if you pass bad XML!
				retval = (T)serializer.Deserialize(reader);
			return retval;
		}

		private static string PutArray(object array)
		{
			XmlSerializer serializer = new XmlSerializer(array.GetType());
			StringBuilder sbXml = new StringBuilder();
			using(XmlWriter writer = XmlWriter.Create(sbXml, new XmlWriterSettings { OmitXmlDeclaration = true }))
			{
				XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
				ns.Add("", ""); //get rid of some extra XML name garbage (make settings file readable)
				serializer.Serialize(writer, array, ns);
			}
			return sbXml.ToString();
		}

		private static object GetDictionary<T>(string innerText, T defaultValue) where T : IDictionary, new()
		{
			Debug.Assert(false);    //I wish I was cool... how to defeat CS0310 and CS0314?
			return new T();
		}
		private static T GetDictionary<T>(string innerText) where T : IDictionary, new()
		{
			Debug.Assert(false);    //I wish I was cool... how to defeat CS0310 and CS0314?
			return new T();
////			Type valueType = value.GetType();
////			Type[] typeArguments = valueType.GetGenericArguments();
////
////			Type typeKey = typeArguments[0];
////			Type typeValue = typeArguments[1];
////			//SerializableDictionary < typeKey.GetType(), typeValue.GetType() > newDict = (SerializableDictionary<int, string>)value;
//			T retval = new T();
//			List<DataItem> tempdataitems = GetArray<List<DataItem>>(innerText);
//			foreach(DataItem datum in tempdataitems)
//				retval[datum.Key] = datum.Value;
//			return retval;
		}

		private string PutDictionary(IDictionary value)
		{
			Debug.Assert(false);    //I wish I was cool...
			return null;

			List<DataItem> tempdataitems = new List<DataItem>(value.Count);
			Type keyType = value.Keys.GetType();
			foreach(string key in value.Keys)	//this works, but only if your KEY is a string!
				tempdataitems.Add(new DataItem(key.ToString(), value[key].ToString()));
			return PutArray(tempdataitems);
       }

		private class DataItem
		{
			public string Key { get; set; }
			public string Value { get; set; }
			private DataItem() { }	//no, use the constructor below

			public DataItem(string key, string value)
			{
				Key = key;
				Value = value;
			}
		}

#endregion Generics

#if !SETTINGS_NO_WINFORMS
		static private string GetControlUniversalName(Control ctrl)
		{
			StringBuilder retval = new StringBuilder();
			if(ctrl != null)
			{
				Control parent = ctrl.Parent;
				retval.Append(ctrl.Name);
				while(parent != null)
				{
					if(!string.IsNullOrEmpty(parent.Name))
					{
						retval.Insert(0, '.');
						retval.Insert(0, parent.Name);
					}
					parent = parent.Parent;
				}
			}
			return retval.ToString();
		}
#endif //!SETTINGS_NO_WINFORMS

#region Document stuff (Load/Save - when you are not using this class for global settings)
		public void Load(string filePath)
		{
			xmlDocument.RemoveAll();
			xmlDocument.Load(filePath);
			DocumentPath = filePath;
			AutoSaveSettingsMode = false;

// 			if(_watcher == null)
// 			{
// 				_watcher = new FileSystemWatcher();
// 
// 				_watcher.Filter = Path.GetFileName(filePath);
// 				_watcher.Changed += new FileSystemEventHandler(watcher_FileChanged);
// 				string pathTest = Path.GetDirectoryName(filePath);
// 				_watcher.Path = pathTest;
// 				_watcher.EnableRaisingEvents = true;
// 			}
		}

// 		void watcher_FileChanged(object sender, FileSystemEventArgs e)
// 		{
// 			Console.WriteLine("Settings changed!");
// 		}

		public void Save()
		{
			// DEVELOPER: you don't need to save, it is done automagically
			//	each time you "put" anything (including when you save window 
			//	placement, etc.)
			Debug.Assert(AutoSaveSettingsMode == false);
			Debug.Assert(!string.IsNullOrEmpty(DocumentPath));
			xmlDocument.Save(DocumentPath);
		}

		public void Save(string filePath)
		{
			// DEVELOPER: you don't need to save, it is done automagically
			//	each time you "put" anything (including when you save window 
			//	placement, etc.)
			Debug.Assert(AutoSaveSettingsMode == false);
			Debug.Assert(!string.IsNullOrEmpty(filePath));
			xmlDocument.Save(filePath);
			DocumentPath = filePath;
		}

		public void Clear()
		{
			xmlDocument.RemoveAll();
			DocumentPath = string.Empty;
		}

		string GetDefaultNode()
		{
			string retval;
			if(!string.IsNullOrEmpty(DefaultSubpath))
				retval = SettingsNode + DefaultSubpath;
			else
				retval = SettingsNode;
			return retval;
		}
		#endregion Document stuff (Load/Save - when you are not using this class for global settings)

#region WinForms, Controls, Bindings
#if !SETTINGS_NO_WINFORMS
#region Forms (Size, Placement/Bounds, Relative Placement)
		public void SaveWindowSize(Form formTarget)
		{
			if(formTarget != null)
			{
				string valueName = formTarget.Name + SizeSuffix;
				PutSetting(valueName, formTarget.Size);
			}
		}

		public void RestoreWindowSize(Form formTarget)
		{
			if(formTarget != null)
			{
				Size formSize = GetSetting(formTarget.Name + SizeSuffix, formTarget.Size);
				if(!formSize.IsEmpty)
					formTarget.Size = formSize;
			}
		}

		public void FormTargetFormClosing(object sender, FormClosingEventArgs e)
		{
			this.SaveWindowPlacement((Form)sender);
		}

		public void SaveWindowPlacement(Control target)
		{
			if(target != null)
			{
				string settingString = SettingsStaticHelpers.SaveWindowPlacement(target);
				PutSetting(target.Name + WindowPlacementSuffix, settingString);
			}
		}

		public void RestoreWindowPlacement(Control target)
		{
			try
			{
				if(target != null)
				{
					RectangleConverter converter = new RectangleConverter();
					var settingString = GetSetting(target.Name + WindowPlacementSuffix, converter.ConvertToString(Rectangle.Empty));
					SettingsStaticHelpers.RestoreWindowPlacement(target, settingString);
				}
			}
			catch(Exception exc)
			{
				_traceSource.TraceEvent(TraceEventType.Error, 57, "RestoreWindowPlacement Exception: " + exc.Message);
			}
		}

		public void SaveRelativeWindowPlacement(Form formTarget)
		{
			if(formTarget != null && formTarget.Owner != null)
			{
				string settingString = SettingsStaticHelpers.SaveRelativeWindowPlacement(formTarget);
				PutSetting(formTarget.Name + RelativeWindowPlacementSuffix, settingString);
			}
			else
			{
				Debug.Assert(false);	// null form or no owner?
			}
		}

		public void RestoreRelativeWindowPlacement(Form formTarget)
		{
			if(formTarget != null && formTarget.Owner != null)
			{
				RectangleConverter converter = new RectangleConverter();
				var settingString = GetSetting(formTarget.Name + RelativeWindowPlacementSuffix, converter.ConvertToString(Rectangle.Empty));
				SettingsStaticHelpers.RestoreRelativeWindowPlacement(formTarget, settingString);
			}
			else
			{
				Debug.Assert(false);	// null form or no owner?
			}
		}
#endregion Forms (Size, Placement/Bounds, Relative Placement)

#region ListView (Items)
		public void SaveItemStrings(ListView lv)
		{
			if(lv != null)
			{
				//List view items into array of array of strings
				List<List<string>> items = new List<List<string>>();
				foreach(ListViewItem lvi in lv.Items)
				{
					List<string> subItems = new List<string>();
					foreach(ListViewItem.ListViewSubItem lvsi in lvi.SubItems)
						subItems.Add(lvsi.Text);
					items.Add(subItems);
				}

				//array of array of strings to XML
 				XmlSerializer serializer = new XmlSerializer(typeof(List<List<string>>));
				StringBuilder sbXml = new StringBuilder();
				using(StringWriter swXml = new StringWriter(sbXml, CultureInfo.CurrentCulture))
				{
					XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
					ns.Add("", "");	//get rid of some extra XML name garbage (make settings file readable)
					serializer.Serialize(swXml, items, ns);

					XmlDocument doc = new XmlDocument();
					doc.LoadXml(swXml.ToString());
					XmlNode tempNode = doc.FirstChild.NextSibling;	// skip the namespace node

					// add XML to settings doc
					XmlNode xmlNode = xmlDocument.SelectSingleNode(GetDefaultNode() + XmlConvert.EncodeLocalName(GetControlUniversalName(lv)));
					if(xmlNode == null)
						xmlNode = CreateMissingNode(GetDefaultNode() + XmlConvert.EncodeLocalName(GetControlUniversalName(lv)));
					xmlNode.RemoveAll();
					xmlNode.AppendChild(xmlDocument.ImportNode(tempNode, true));

					if(AutoSaveSettingsMode)
					{
						//_watcher.EnableRaisingEvents = false;
						xmlDocument.Save(DocumentPath);
						//_watcher.EnableRaisingEvents = true;
					}
				}
			}
		}

		public void LoadItemStrings(ListView lv)
		{
			if(lv != null)
			{
				XmlNode xmlNode = xmlDocument.SelectSingleNode(GetDefaultNode() + XmlConvert.EncodeLocalName(GetControlUniversalName(lv)));
				if(xmlNode != null)
				{
					//load XML into stream
					XmlSerializer serializer = new XmlSerializer(typeof(List<List<string>>));
					using(MemoryStream stm = new MemoryStream())
					{
						StreamWriter stw = new StreamWriter(stm);
						stw.Write(xmlNode.FirstChild.OuterXml);
						stw.Flush();
						stm.Position = 0;

						//stream into array of array of strings 
						List<List<string>> items = (List<List<string>>)serializer.Deserialize(stm);

						//array of array of strings to list view items
						foreach(List<string> row in items)
						{
							if(row.Count > 0)
							{
								ListViewItem lvi = lv.Items.Add(row[0], row[0], 0);
								for(int countColumns = 1; countColumns < row.Count; countColumns++)
									lvi.SubItems.Add(row[countColumns]);
							}
							else
							{
								//QuickLog.WriteError("ERROR: LoadItemStrings for {0} has row with no values?", GetControlUniversalName(lv));
							}
						}
					}
				}
			}
		}
#endregion ListView (Items)

#region ListView (Column Widths)
		public void SaveColumnWidths(ListView lv)
		{
			string columnWidths = SettingsStaticHelpers.ColumnWidthsToString(lv);
			PutSetting(GetControlUniversalName(lv) + ColumnWidthsSuffix, columnWidths);
		}

		public bool RestoreColumnWidths(ListView lv)
		{
			bool retval = false;
			if(lv != null)
			{
				string columnWidths = GetSetting(GetControlUniversalName(lv) + ColumnWidthsSuffix, "");
				retval = SettingsStaticHelpers.RestoreColumnWidths(lv, columnWidths);
			}
			return retval;
		}

		static public void AutoSizeColumnWidths(ListView lv)
		{
			if(lv == null)
				throw new ArgumentNullException("lv");
			foreach(ColumnHeader col in lv.Columns)
				col.Width = -1;
		}

#endregion ListView (Column Widths)

#region Generic/Callback (Column Widths)
		public void SaveColumnWidths(Control control, CustomSettingsXmlSaveColumnWidths callback)
		{
			Debug.Assert(control != null && callback != null);
			if(control != null && callback != null)
			{
				string columnWidths = callback();
				PutSetting(GetControlUniversalName(control) + ColumnWidthsSuffix, columnWidths);
			}
		}

		public bool RestoreColumnWidths(Control control, CustomSettingsXmlRestoreColumnWidths callback)
		{
			Debug.Assert(control != null && callback != null);
			bool retval = false;
			if(control != null && callback != null)
			{
				string columnWidths = GetSetting(GetControlUniversalName(control) + ColumnWidthsSuffix, "");
				retval = SettingsStaticHelpers.RestoreColumnWidths(callback, columnWidths);
			}
			return retval;
		}

		public void SaveColumnWidths(Control control, IJkhColumnWidthSettings callback)
		{
			Debug.Assert(control != null && callback != null);
			if(control != null && callback != null)
			{
				string columnWidths = callback.ColumnWidthsToString();
				PutSetting(GetControlUniversalName(control) + ColumnWidthsSuffix, columnWidths);
			}
		}

		public bool RestoreColumnWidths(Control control, IJkhColumnWidthSettings callback)
		{
			Debug.Assert(control != null && callback != null);
			bool retval = false;
			if(control != null && callback != null)
			{
				string columnWidths = GetSetting(GetControlUniversalName(control) + ColumnWidthsSuffix, "");
				retval = SettingsStaticHelpers.RestoreColumnWidths(callback, columnWidths);
			}
			return retval;
		}
#endregion Generic/Callback (Column Widths)

#region ComboBoxItems
		public void LoadItemStrings(ComboBox comboBox)
		{
			if(comboBox == null)
				throw new ArgumentNullException("comboBox");
			string[] items = GetSetting(GetControlUniversalName(comboBox), new string[0]);
			comboBox.Items.AddRange(items);
		}

		public void SaveItemStrings(ComboBox comboBox, int maxItems)
		{
			if(comboBox == null)
				throw new ArgumentNullException("comboBox");
			if(comboBox.Items.IndexOf(comboBox.Text) < 0)
				comboBox.Items.Add(comboBox.Text);
			while(comboBox.Items.Count > maxItems)
				comboBox.Items.RemoveAt(comboBox.Items.Count - 1);
			string[] fileNames = new string[comboBox.Items.Count];
			comboBox.Items.CopyTo(fileNames, 0);
			PutSetting(GetControlUniversalName(comboBox), fileNames);
		}
#endregion ComboBoxItems

#region DataGridView (Column Widths)
		public void SaveColumnWidths(DataGridView dataGridView, string keyTag)
		{
			string columnWidths = SettingsStaticHelpers.ColumnWidthsToString(dataGridView);
			PutSetting(GetControlUniversalName(dataGridView) + "_" + keyTag + ColumnWidthsSuffix, columnWidths.ToString());
		}

		public bool RestoreColumnWidths(DataGridView dataGridView, string keyTag)
		{
			bool retval = false;
			if(dataGridView != null)
			{
				string columnWidths = GetSetting(GetControlUniversalName(dataGridView) + "_" + keyTag + ColumnWidthsSuffix, "");
				retval = SettingsStaticHelpers.RestoreColumnWidths(dataGridView, columnWidths);
			}
			return retval;
		}
#endregion DataGridView (Column Widths)

#region Control bindings (Bind, GuiToDoc, DocToGui)
		public void BindWindowPlacement(Form formTarget)
		{
			if(formTarget == null)
				throw new ArgumentNullException("formTarget");
			RestoreWindowPlacement(formTarget);
			formTarget.FormClosing += FormTargetFormClosing;
		}

		public bool Bind(Control ctrl, string keyName)
		{
			bool retval = true;
			Bindings[keyName] = ctrl;
			return retval;
		}

		public bool Bind(Control ctrl, string keyName, object defaultValue)
		{
			bool retval = true;
			Bindings[keyName] = ctrl;
			BindingDefaults[keyName] = defaultValue;
			return retval;
		}

		public void GuiToDoc()
		{
			foreach(KeyValuePair<string, Control> kvp in Bindings)
			{
				if(kvp.Value.GetType() == typeof(System.Windows.Forms.CheckBox))
					PutSetting(kvp.Key, ((System.Windows.Forms.CheckBox)kvp.Value).Checked);
				else
					PutSetting(kvp.Key, kvp.Value.Text);
			}
		}

		public void DocToGui()
		{
			foreach(KeyValuePair<string, Control> kvp in Bindings)
			{
				string val = GetSetting(kvp.Key, _MagicalDefualtStringFromHell);
				if(val == _MagicalDefualtStringFromHell)
				{
					if(BindingDefaults.ContainsKey(kvp.Key))
					{
						val = BindingDefaults[kvp.Key].ToString();
					}
					else
					{
						//QuickLog.WriteError("ERROR: The setting {0} has no saved value and no default!", kvp.Key);
						val = string.Empty;
					}
				}
	
				if(kvp.Value.GetType() == typeof(System.Windows.Forms.CheckBox))
				{
					bool valBool;
					if(!bool.TryParse(val, out valBool))
					{
						//QuickLog.WriteError("ERROR: The setting {0} is not a valid bool: \"{1}\"", kvp.Key, kvp.Value.ToString());
						valBool = false;	//safe default
					}
					((System.Windows.Forms.CheckBox)kvp.Value).Checked = valBool;
				}
				else
				{
					kvp.Value.Text = val;
				}
			}
		}
		#endregion Control bindings (Bind, GuiToDoc, DocToGui)
#endif //!SETTINGS_NO_WINFORMS
#endregion WinForms, Controls, Bindings

#region Encrypted String
#if !SETTINGS_NO_SQL_ENCRYPT
		/// <summary>Decrypts ConnectionString password after reading it from settings, WARN: any script kiddie can "crack" this - you have been warned!</summary>
		public string GetSqlConnectionString(string xmlPath, string defaultValue)
		{
			xmlPath = NormalizeXmlPathJkh(xmlPath);

			string retval = GetSetting(xmlPath, defaultValue);
			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(retval);
			try
			{
				builder.Password = AesThenHmac.SimpleDecryptWithPassword(builder.Password, "jkh.CustomSettingsXML");
				retval = builder.ToString();
			}
			catch(FormatException exc)
			{
				_traceSource.TraceEvent(TraceEventType.Error, 57, "!!!!!GetSqlConnectionString EXCEPTION: " + exc.Message);
			}
			catch(ArgumentException exc)
			{
				_traceSource.TraceEvent(TraceEventType.Error, 57, "!!!!!GetSqlConnectionString EXCEPTION: " + exc.Message);
			}
			return retval;
		}

		/// <summary>Encrypts ConnectionString password before writing it to settings, WARN: any script kiddie can "crack" this - you have been warned!</summary>
		public void PutSqlConnectionString(string xmlPath, string connectionString)
		{
			xmlPath = NormalizeXmlPathJkh(xmlPath);

			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
			if(builder.Password.Length == 0)
				builder.Password = "1234";	// an example of a very bad password!
			builder.Password = AesThenHmac.SimpleEncryptWithPassword(builder.Password, "jkh.CustomSettingsXML");
			PutSetting(xmlPath, builder.ToString());
		}
#endif  //SETTINGS_NO_SQL_ENCRYPT
#endregion Encrypted String
	}
}
