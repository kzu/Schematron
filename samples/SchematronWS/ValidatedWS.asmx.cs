using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;

using NMatrix.Schematron;

namespace WebServiceTest
{
	/// <summary>
	/// Tests a schematron-validated webservice.
	/// </summary>
	[WebService(Namespace="http://aspnet2.com/kzu")]
	public class ValidatedWS : System.Web.Services.WebService
	{
		public ValidatedWS()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		[WebMethod]
		[Validation("po-schema.xsd", OutputFormatting.Log)]
		public void BatchInsert(XmlNode orders)
		{
			// Insert validated orders.
		}
	}
}
