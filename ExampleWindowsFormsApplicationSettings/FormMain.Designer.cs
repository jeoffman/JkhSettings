namespace ExampleWindowsFormsApplicationSettings
{
	partial class FormMain
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.richTextBoxTraces = new System.Windows.Forms.RichTextBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonAbout = new System.Windows.Forms.ToolStripButton();
			this.buttonTrace = new System.Windows.Forms.Button();
			this.comboBoxTraceLevel = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxTraceText = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.comboBoxSwitchValue = new System.Windows.Forms.ComboBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.checkBoxHookUpTraceListener = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
			this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView1.Location = new System.Drawing.Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(435, 98);
			this.listView1.TabIndex = 1;
			this.toolTip1.SetToolTip(this.listView1, "Resize the colums, restored next time you run the app");
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(12, 90);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.listView1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.richTextBoxTraces);
			this.splitContainer1.Size = new System.Drawing.Size(435, 200);
			this.splitContainer1.SplitterDistance = 98;
			this.splitContainer1.TabIndex = 2;
			// 
			// richTextBoxTraces
			// 
			this.richTextBoxTraces.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.richTextBoxTraces.Location = new System.Drawing.Point(3, 3);
			this.richTextBoxTraces.Name = "richTextBoxTraces";
			this.richTextBoxTraces.Size = new System.Drawing.Size(429, 95);
			this.richTextBoxTraces.TabIndex = 0;
			this.richTextBoxTraces.Text = "";
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAbout});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(459, 25);
			this.toolStrip1.TabIndex = 3;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButtonAbout
			// 
			this.toolStripButtonAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonAbout.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAbout.Image")));
			this.toolStripButtonAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonAbout.Name = "toolStripButtonAbout";
			this.toolStripButtonAbout.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonAbout.Text = "toolStripButton1";
			this.toolStripButtonAbout.Click += new System.EventHandler(this.toolStripButtonAbout_Click);
			// 
			// buttonTrace
			// 
			this.buttonTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonTrace.Location = new System.Drawing.Point(397, 28);
			this.buttonTrace.Name = "buttonTrace";
			this.buttonTrace.Size = new System.Drawing.Size(50, 23);
			this.buttonTrace.TabIndex = 4;
			this.buttonTrace.Text = "Trace";
			this.toolTip1.SetToolTip(this.buttonTrace, "Trace an event!");
			this.buttonTrace.UseVisualStyleBackColor = true;
			this.buttonTrace.Click += new System.EventHandler(this.buttonTrace_Click);
			// 
			// comboBoxTraceLevel
			// 
			this.comboBoxTraceLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxTraceLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxTraceLevel.FormattingEnabled = true;
			this.comboBoxTraceLevel.Location = new System.Drawing.Point(305, 30);
			this.comboBoxTraceLevel.Name = "comboBoxTraceLevel";
			this.comboBoxTraceLevel.Size = new System.Drawing.Size(86, 21);
			this.comboBoxTraceLevel.TabIndex = 5;
			this.toolTip1.SetToolTip(this.comboBoxTraceLevel, "TraceEvent level to write the event");
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 33);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(63, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "TraceEvent";
			// 
			// textBoxTraceText
			// 
			this.textBoxTraceText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxTraceText.Location = new System.Drawing.Point(81, 30);
			this.textBoxTraceText.Name = "textBoxTraceText";
			this.textBoxTraceText.Size = new System.Drawing.Size(218, 20);
			this.textBoxTraceText.TabIndex = 7;
			this.toolTip1.SetToolTip(this.textBoxTraceText, "words, words, words...");
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 59);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(66, 13);
			this.label2.TabIndex = 8;
			this.label2.Text = "SwitchValue";
			// 
			// comboBoxSwitchValue
			// 
			this.comboBoxSwitchValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxSwitchValue.FormattingEnabled = true;
			this.comboBoxSwitchValue.Location = new System.Drawing.Point(84, 56);
			this.comboBoxSwitchValue.Name = "comboBoxSwitchValue";
			this.comboBoxSwitchValue.Size = new System.Drawing.Size(118, 21);
			this.comboBoxSwitchValue.TabIndex = 9;
			this.toolTip1.SetToolTip(this.comboBoxSwitchValue, "Set the TraceSource named \"MyTraceSource\" switch level");
			this.comboBoxSwitchValue.SelectedIndexChanged += new System.EventHandler(this.comboBoxSwitchValue_SelectedIndexChanged);
			// 
			// checkBoxHookUpTraceListener
			// 
			this.checkBoxHookUpTraceListener.AutoSize = true;
			this.checkBoxHookUpTraceListener.Checked = true;
			this.checkBoxHookUpTraceListener.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxHookUpTraceListener.Location = new System.Drawing.Point(342, 58);
			this.checkBoxHookUpTraceListener.Name = "checkBoxHookUpTraceListener";
			this.checkBoxHookUpTraceListener.Size = new System.Drawing.Size(102, 17);
			this.checkBoxHookUpTraceListener.TabIndex = 10;
			this.checkBoxHookUpTraceListener.Text = "Listen to Traces";
			this.checkBoxHookUpTraceListener.UseVisualStyleBackColor = true;
			this.checkBoxHookUpTraceListener.CheckedChanged += new System.EventHandler(this.checkBoxHookUpTraceListener_CheckedChanged);
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(459, 302);
			this.Controls.Add(this.checkBoxHookUpTraceListener);
			this.Controls.Add(this.comboBoxSwitchValue);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBoxTraceText);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboBoxTraceLevel);
			this.Controls.Add(this.buttonTrace);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.splitContainer1);
			this.MinimumSize = new System.Drawing.Size(290, 230);
			this.Name = "FormMain";
			this.Text = "Test Form";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton toolStripButtonAbout;
		private System.Windows.Forms.RichTextBox richTextBoxTraces;
		private System.Windows.Forms.Button buttonTrace;
		private System.Windows.Forms.ComboBox comboBoxTraceLevel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxTraceText;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboBoxSwitchValue;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBox checkBoxHookUpTraceListener;
	}
}

