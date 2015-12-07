using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

using NMatrix.Schematron;

namespace WinTest
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Home : System.Windows.Forms.Form
	{
		#region Designer stuff
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtXml;
		private System.Windows.Forms.Button btnXmlFile;
		private System.Windows.Forms.Button btnXsdFile;
		private System.Windows.Forms.TextBox txtSchema;
		private System.Windows.Forms.OpenFileDialog dlgOpen;
		private System.Windows.Forms.Button btnExecute;
		private System.Windows.Forms.TextBox txtMsg;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbOutput;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtPhase;
		private System.Windows.Forms.CheckBox chkWrap;
		private System.Windows.Forms.Button btnSendToWs;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TextBox txtDocumentXml;
		private System.Windows.Forms.TextBox txtSchemaXml;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Home()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Home));
			this.txtMsg = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtXml = new System.Windows.Forms.TextBox();
			this.btnXmlFile = new System.Windows.Forms.Button();
			this.btnXsdFile = new System.Windows.Forms.Button();
			this.txtSchema = new System.Windows.Forms.TextBox();
			this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
			this.btnExecute = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.cbOutput = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtPhase = new System.Windows.Forms.TextBox();
			this.btnSendToWs = new System.Windows.Forms.Button();
			this.chkWrap = new System.Windows.Forms.CheckBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.txtDocumentXml = new System.Windows.Forms.TextBox();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.txtSchemaXml = new System.Windows.Forms.TextBox();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtMsg
			// 
			this.txtMsg.AcceptsReturn = true;
			this.txtMsg.AcceptsTab = true;
			this.txtMsg.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtMsg.Location = new System.Drawing.Point(0, 0);
			this.txtMsg.Multiline = true;
			this.txtMsg.Name = "txtMsg";
			this.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtMsg.Size = new System.Drawing.Size(560, 254);
			this.txtMsg.TabIndex = 1;
			this.txtMsg.Text = "";
			this.txtMsg.WordWrap = false;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(52, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "XML File:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Schema:";
			// 
			// txtXml
			// 
			this.txtXml.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtXml.Location = new System.Drawing.Point(64, 32);
			this.txtXml.Name = "txtXml";
			this.txtXml.Size = new System.Drawing.Size(400, 21);
			this.txtXml.TabIndex = 5;
			this.txtXml.Text = "..\\schematron\\po-instance.xml";
			this.txtXml.Validated += new System.EventHandler(this.OnSetDocument);
			// 
			// btnXmlFile
			// 
			this.btnXmlFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnXmlFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnXmlFile.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnXmlFile.Location = new System.Drawing.Point(472, 32);
			this.btnXmlFile.Name = "btnXmlFile";
			this.btnXmlFile.Size = new System.Drawing.Size(24, 20);
			this.btnXmlFile.TabIndex = 6;
			this.btnXmlFile.Text = "...";
			this.btnXmlFile.Click += new System.EventHandler(this.btnXmlFile_Click);
			// 
			// btnXsdFile
			// 
			this.btnXsdFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnXsdFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnXsdFile.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnXsdFile.Location = new System.Drawing.Point(472, 8);
			this.btnXsdFile.Name = "btnXsdFile";
			this.btnXsdFile.Size = new System.Drawing.Size(24, 20);
			this.btnXsdFile.TabIndex = 2;
			this.btnXsdFile.Text = "...";
			this.btnXsdFile.Click += new System.EventHandler(this.btnXsdFile_Click);
			// 
			// txtSchema
			// 
			this.txtSchema.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSchema.Location = new System.Drawing.Point(64, 8);
			this.txtSchema.Name = "txtSchema";
			this.txtSchema.Size = new System.Drawing.Size(400, 21);
			this.txtSchema.TabIndex = 1;
			this.txtSchema.Text = "..\\schematron\\po-schema.xsd";
			this.txtSchema.Validated += new System.EventHandler(this.OnSetSchema);
			// 
			// btnExecute
			// 
			this.btnExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExecute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnExecute.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnExecute.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnExecute.Location = new System.Drawing.Point(504, 8);
			this.btnExecute.Name = "btnExecute";
			this.btnExecute.Size = new System.Drawing.Size(72, 20);
			this.btnExecute.TabIndex = 3;
			this.btnExecute.Text = "&Execute";
			this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 16);
			this.label3.TabIndex = 8;
			this.label3.Text = "Output:";
			// 
			// cbOutput
			// 
			this.cbOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbOutput.Location = new System.Drawing.Point(64, 56);
			this.cbOutput.Name = "cbOutput";
			this.cbOutput.Size = new System.Drawing.Size(240, 21);
			this.cbOutput.TabIndex = 9;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(320, 56);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 16);
			this.label4.TabIndex = 10;
			this.label4.Text = "Phase:";
			// 
			// txtPhase
			// 
			this.txtPhase.Location = new System.Drawing.Point(360, 56);
			this.txtPhase.Name = "txtPhase";
			this.txtPhase.Size = new System.Drawing.Size(104, 21);
			this.txtPhase.TabIndex = 11;
			this.txtPhase.Text = "";
			// 
			// btnSendToWs
			// 
			this.btnSendToWs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSendToWs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSendToWs.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnSendToWs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnSendToWs.Location = new System.Drawing.Point(504, 32);
			this.btnSendToWs.Name = "btnSendToWs";
			this.btnSendToWs.Size = new System.Drawing.Size(72, 20);
			this.btnSendToWs.TabIndex = 7;
			this.btnSendToWs.Text = "&Send 2 WS";
			this.btnSendToWs.Click += new System.EventHandler(this.btnSendToWs_Click);
			// 
			// chkWrap
			// 
			this.chkWrap.Location = new System.Drawing.Point(472, 56);
			this.chkWrap.Name = "chkWrap";
			this.chkWrap.Size = new System.Drawing.Size(80, 24);
			this.chkWrap.TabIndex = 12;
			this.chkWrap.Text = "&Wrap text";
			this.chkWrap.CheckedChanged += new System.EventHandler(this.chkWrap_CheckedChanged);
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Location = new System.Drawing.Point(8, 88);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(568, 280);
			this.tabControl1.TabIndex = 6;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.txtMsg);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(560, 254);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Result";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.txtDocumentXml);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(560, 254);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Document";
			// 
			// txtDocumentXml
			// 
			this.txtDocumentXml.AcceptsReturn = true;
			this.txtDocumentXml.AcceptsTab = true;
			this.txtDocumentXml.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtDocumentXml.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtDocumentXml.Location = new System.Drawing.Point(0, 0);
			this.txtDocumentXml.Multiline = true;
			this.txtDocumentXml.Name = "txtDocumentXml";
			this.txtDocumentXml.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtDocumentXml.Size = new System.Drawing.Size(560, 254);
			this.txtDocumentXml.TabIndex = 2;
			this.txtDocumentXml.Text = "";
			this.txtDocumentXml.WordWrap = false;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.txtSchemaXml);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(560, 254);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Schema";
			// 
			// txtSchemaXml
			// 
			this.txtSchemaXml.AcceptsReturn = true;
			this.txtSchemaXml.AcceptsTab = true;
			this.txtSchemaXml.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtSchemaXml.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtSchemaXml.Location = new System.Drawing.Point(0, 0);
			this.txtSchemaXml.Multiline = true;
			this.txtSchemaXml.Name = "txtSchemaXml";
			this.txtSchemaXml.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtSchemaXml.Size = new System.Drawing.Size(560, 254);
			this.txtSchemaXml.TabIndex = 0;
			this.txtSchemaXml.Text = "";
			this.txtSchemaXml.WordWrap = false;
			// 
			// Home
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(584, 374);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.chkWrap);
			this.Controls.Add(this.btnSendToWs);
			this.Controls.Add(this.txtPhase);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cbOutput);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnExecute);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtXml);
			this.Controls.Add(this.btnXmlFile);
			this.Controls.Add(this.btnXsdFile);
			this.Controls.Add(this.txtSchema);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Home";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " NMatrix Schematron.NET Test";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.Home_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Home());
		}

		#endregion Designer stuff

		#region Dialogs and setup

		private void btnXsdFile_Click(object sender, System.EventArgs e)
		{
			if (dlgOpen.ShowDialog() == DialogResult.OK)
			{
				txtSchema.Text = dlgOpen.FileName;
				using (StreamReader sr = new StreamReader(txtSchema.Text))
				{
					txtSchemaXml.Text = MakePretty(sr.ReadToEnd());
				}
			}
		}

		private void btnXmlFile_Click(object sender, System.EventArgs e)
		{
			if (dlgOpen.ShowDialog() == DialogResult.OK)
			{
				txtXml.Text = dlgOpen.FileName;
				using (StreamReader sr = new StreamReader(txtXml.Text))
				{
					txtDocumentXml.Text = MakePretty(sr.ReadToEnd());
				}
			}
		}

		private void Home_Load(object sender, System.EventArgs e)
		{
			foreach (OutputFormatting value in Enum.GetValues(typeof(OutputFormatting)))
			{
				cbOutput.Items.Add(value);
			}
			using (StreamReader sr = new StreamReader(txtSchema.Text))
			{
				txtSchemaXml.Text = MakePretty(sr.ReadToEnd());
			}
			using (StreamReader sr = new StreamReader(txtXml.Text))
			{
				txtDocumentXml.Text = MakePretty(sr.ReadToEnd());
			}

			cbOutput.SelectedItem = OutputFormatting.Default;

		}
		#endregion

		private void btnExecute_Click(object sender, System.EventArgs e)
		{
			OutputFormatting format = (OutputFormatting) cbOutput.SelectedItem;
			Validator val = new Validator(format);
			val.AddSchema(new StringReader(txtSchemaXml.Text));
			//val.ReturnType = NavigableType.XmlDocument;
			if (txtPhase.Text.Length != 0)
				val.Phase = txtPhase.Text;

			try
			{
				// Validate using document literal in textbox.
				IXPathNavigable doc = val.Validate(new StringReader(txtDocumentXml.Text));
				// Continue processing valid document.
				txtMsg.Text = "Valid file!";
			}
			catch (ValidationException ex)
			{
				txtMsg.Text = ex.Message;
			}

			tabControl1.SelectedTab = tabPage1;
		}

		private void btnSendToWs_Click(object sender, System.EventArgs e)
		{
			txtMsg.Text = "";
			txtSchemaXml.Text = "";

			SchematronWS.ValidatedWS ws = new WinTest.SchematronWS.ValidatedWS();

			try
			{
				XmlDocument doc = new XmlDocument();
				// Validate using document literal in textbox.
				doc.Load(new StringReader(txtDocumentXml.Text));

				ws.BatchInsert(doc.DocumentElement);
				txtMsg.Text = "Valid file!";
			}
			catch (Exception ex)
			{
				string msg = ex.Message;
				// Need to decode HTML-isms.
				msg = System.Web.HttpUtility.HtmlDecode(msg);
				// Need to get back the standard line breaks.
				txtMsg.Text = msg.Replace("\n", Environment.NewLine);
			}

			tabControl1.SelectedTab = tabPage1;
		}

		string MakePretty(string xml)
		{
			// Convert tabs to 2 spaces.
			xml = xml.Replace("\t", "  ");
			// Convert 4 spaces to 2.
			xml = xml.Replace("    ", "  ");
			return xml;
		}

		private void chkWrap_CheckedChanged(object sender, System.EventArgs e)
		{
			this.txtMsg.WordWrap = chkWrap.Checked;
			if (chkWrap.Checked)
			{
				this.txtMsg.ScrollBars = ScrollBars.Vertical;
			}
			else
			{
				this.txtMsg.ScrollBars = ScrollBars.Both;
			}
		}

		private void OnSetSchema(object sender, System.EventArgs e)
		{
			// Dump schema we're working with.
			using (StreamReader sr = new StreamReader(txtSchema.Text))
			{
				txtSchemaXml.Text = MakePretty(sr.ReadToEnd());
			}		
		}

		private void OnSetDocument(object sender, System.EventArgs e)
		{
			// Dump documents we're working with.
			using (StreamReader sr = new StreamReader(txtXml.Text))
			{
				txtDocumentXml.Text = MakePretty(sr.ReadToEnd());
			}
		}

	}
}
