namespace JkhSettings
{
	partial class FormAbout
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.icon = new System.Windows.Forms.PictureBox();
			this.groupBox = new System.Windows.Forms.GroupBox();
			this.labelCopyright = new System.Windows.Forms.Label();
			this.labelAppName = new System.Windows.Forms.Label();
			this.labelVersion = new System.Windows.Forms.Label();
			this.labelCompany = new System.Windows.Forms.Label();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonBrowseApp = new System.Windows.Forms.Button();
			this.buttonBrowseSettings = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
			this.groupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// icon
			// 
			this.icon.Location = new System.Drawing.Point(89, 39);
			this.icon.Name = "icon";
			this.icon.Size = new System.Drawing.Size(32, 32);
			this.icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.icon.TabIndex = 0;
			this.icon.TabStop = false;
			// 
			// groupBox
			// 
			this.groupBox.Controls.Add(this.labelCopyright);
			this.groupBox.Controls.Add(this.icon);
			this.groupBox.Controls.Add(this.labelAppName);
			this.groupBox.Controls.Add(this.labelVersion);
			this.groupBox.Controls.Add(this.labelCompany);
			this.groupBox.Location = new System.Drawing.Point(12, 12);
			this.groupBox.Name = "groupBox";
			this.groupBox.Size = new System.Drawing.Size(212, 196);
			this.groupBox.TabIndex = 1;
			this.groupBox.TabStop = false;
			// 
			// labelCopyright
			// 
			this.labelCopyright.Location = new System.Drawing.Point(6, 152);
			this.labelCopyright.Name = "labelCopyright";
			this.labelCopyright.Size = new System.Drawing.Size(200, 13);
			this.labelCopyright.TabIndex = 3;
			this.labelCopyright.Text = "Copyright Here";
			this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelAppName
			// 
			this.labelAppName.Location = new System.Drawing.Point(6, 83);
			this.labelAppName.Name = "labelAppName";
			this.labelAppName.Size = new System.Drawing.Size(200, 13);
			this.labelAppName.TabIndex = 0;
			this.labelAppName.Text = "App Name Here";
			this.labelAppName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelVersion
			// 
			this.labelVersion.Location = new System.Drawing.Point(6, 120);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(200, 13);
			this.labelVersion.TabIndex = 1;
			this.labelVersion.Text = "Version Here";
			this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelCompany
			// 
			this.labelCompany.Location = new System.Drawing.Point(6, 168);
			this.labelCompany.Name = "labelCompany";
			this.labelCompany.Size = new System.Drawing.Size(200, 13);
			this.labelCompany.TabIndex = 2;
			this.labelCompany.Text = "Company Here";
			this.labelCompany.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(225, 185);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 0;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonBrowseApp
			// 
			this.buttonBrowseApp.Location = new System.Drawing.Point(245, 12);
			this.buttonBrowseApp.Name = "buttonBrowseApp";
			this.buttonBrowseApp.Size = new System.Drawing.Size(60, 21);
			this.buttonBrowseApp.TabIndex = 2;
			this.buttonBrowseApp.Text = "EXE...";
			this.buttonBrowseApp.UseVisualStyleBackColor = true;
			this.buttonBrowseApp.Click += new System.EventHandler(this.buttonBrowseApp_Click);
			// 
			// buttonBrowseSettings
			// 
			this.buttonBrowseSettings.Location = new System.Drawing.Point(245, 32);
			this.buttonBrowseSettings.Margin = new System.Windows.Forms.Padding(1);
			this.buttonBrowseSettings.Name = "buttonBrowseSettings";
			this.buttonBrowseSettings.Size = new System.Drawing.Size(60, 21);
			this.buttonBrowseSettings.TabIndex = 3;
			this.buttonBrowseSettings.Text = "Settings";
			this.buttonBrowseSettings.UseVisualStyleBackColor = true;
			this.buttonBrowseSettings.Click += new System.EventHandler(this.buttonBrowseSettings_Click);
			// 
			// FormAbout
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(312, 220);
			this.Controls.Add(this.buttonBrowseSettings);
			this.Controls.Add(this.buttonBrowseApp);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.groupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "FormAbout";
			this.Text = "About";
			this.Load += new System.EventHandler(this.FormAbout_Load);
			((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
			this.groupBox.ResumeLayout(false);
			this.groupBox.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox;
		private System.Windows.Forms.PictureBox icon;
		private System.Windows.Forms.Label labelCopyright;
		private System.Windows.Forms.Label labelCompany;
		private System.Windows.Forms.Label labelAppName;
		private System.Windows.Forms.Label labelVersion;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonBrowseApp;
		private System.Windows.Forms.Button buttonBrowseSettings;
	}
}