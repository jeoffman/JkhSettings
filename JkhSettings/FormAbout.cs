using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;


namespace JkhSettings
{
	public partial class FormAbout : Form
	{
		public enum AssemblyInfoSource
		{
			FromExe,
			FromDll
		}

		AssemblyInfoSource InfoSource = AssemblyInfoSource.FromExe;

		public FormAbout(AssemblyInfoSource source = AssemblyInfoSource.FromExe)
		{
			InfoSource = source;

			InitializeComponent();
		}

		private void FormAbout_Load(object sender, EventArgs e)
		{
			Assembly theAssembly;
			if(InfoSource == AssemblyInfoSource.FromExe)
				theAssembly = System.Reflection.Assembly.GetEntryAssembly();	//Get the name of the EXE
			else
				theAssembly = Assembly.GetExecutingAssembly();	//get the name of this DLL

			AssemblyName theAssemblyName = theAssembly.GetName();

			this.Text = "About " + theAssemblyName.Name + "...";
			labelAppName.Text = theAssemblyName.Name;
			labelVersion.Text = "Version: " + theAssemblyName.Version.ToString();

			AssemblyCopyrightAttribute copyright = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(theAssembly, typeof(AssemblyCopyrightAttribute));
			if(copyright != null && !string.IsNullOrEmpty(copyright.Copyright))
				labelCopyright.Text = copyright.Copyright;
			else
				labelCopyright.Visible = false;

			AssemblyCompanyAttribute company = (AssemblyCompanyAttribute)Attribute.GetCustomAttribute(theAssembly, typeof(AssemblyCompanyAttribute));
			if(company != null && !string.IsNullOrEmpty(company.Company))
				labelCompany.Text = company.Company;
			else
				labelCompany.Visible = false;

			Icon iconToUse = null;
			FormCollection fc = Application.OpenForms;
			if(fc.Count > 0)
				iconToUse = fc[0].Icon;

			//Icon iconToUse = System.Windows.Forms.ApplicationContext.MainForm.Icon;
//			 System.Windows.Forms.Form MainForm
//			Icon iconToUse = null;
// 			if(this.Icon != null)
// 				iconToUse = this.Icon;
// 			else if(this.ParentForm != null && this.ParentForm.Icon != null)
// 				iconToUse = this.ParentForm.Icon;
// 			else
// 				Debug.Assert(false);	// need some way to get the application's icon...

			if(iconToUse != null)
			{
				this.Icon = iconToUse;
				icon.Image = iconToUse.ToBitmap();
			}
			else
			{
				//Debug.Assert(false);
			}
			CenterToParent();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		[SecurityCritical()]
		private void buttonBrowseApp_Click(object sender, EventArgs e)
		{
			string funkyPathWithHashtags = System.Windows.Forms.Application.ExecutablePath;
			string path = funkyPathWithHashtags.Replace('/', '\\');	//fixes a path with the # symbol in it (i.e. my C# source folder)
			//string path = System.Reflection.Assembly.GetExecutingAssembly().Location;	NO! this gives the path to our DLL, not the EXE
			ShowExplorer(path);
		}

		//a pecking-order search for where the settings are stored...
		private void buttonBrowseSettings_Click(object sender, EventArgs e)
		{
			//Are you missing a reference to System.Configuration?
			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
			if(File.Exists(config.FilePath))
			{
				ShowExplorer(config.FilePath);
			}
			else
			{
				config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoaming);
				if(File.Exists(config.FilePath))
					ShowExplorer(config.FilePath);
				else
					ShowExplorer(Application.LocalUserAppDataPath);
			}
		}

		//[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust"), SecurityCritical()]
		//[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		//[PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
		public static void ShowExplorer(string path)
		{
			using(Process p = new Process())
			{
				p.StartInfo.FileName = "explorer.exe";
				p.StartInfo.Arguments = string.Format("/e,/select,\"{0}\"", path);

				try
				{
					p.Start();
				}
				catch(InvalidOperationException exc)
				{
					MessageBox.Show(exc.ToString());
				}
				catch(Win32Exception exc)
				{
					MessageBox.Show(exc.ToString());
				}
			}
		}
	}
}
