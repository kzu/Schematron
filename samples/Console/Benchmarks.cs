using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml.Schema;
//using MSXML2;
using System.Diagnostics;
using NMatrix.Schematron;

namespace Test
{
    /// <summary />
    public class Benchmarks
	{

		private Benchmarks()
		{
		}

		/*
		/// <summary>
		/// Embedded schematron validation.
		/// </summary>
		/// <remarks>
		/// Metastylesheets and validating schema preloaded.
		/// </remarks>
		public static void EmbeddedSchemaExec(int iterations)
		{
			//Console.WriteLine(new String('-', 60));
			//Console.WriteLine(new string('-', 20) + " EmbeddedSchemaExec " + new string('-', 20));
			//Console.WriteLine(new String('-', 60));
			
			XPathDocument doc = new XPathDocument(@"..\Files\Tournament.xml");

			Schema sch = new Schema();
			sch.Load(@"..\Files\tournament-schema.xml");

			XslTransform meta = new XslTransform();
			meta.Load(@"..\Files\diag.xsl");
			MemoryStream mem = new MemoryStream();
			XmlTextWriter xw = new XmlTextWriter(mem, System.Text.Encoding.UTF8);           
			meta.Transform(new XPathDocument(@"..\Files\tournament-schema.xml"), null, xw);

			mem.Seek(0, SeekOrigin.Begin);
			XslTransform xsh = new XslTransform();
			xsh.Load(new XPathDocument(mem));
 
			XPathNavigator nav = doc.CreateNavigator();
 
			long start;
			long end;

			//XSLT execution.
			start = DateTime.Now.Ticks;
			for (int i = 0; i < iterations; i++)
			{
				nav.MoveToRoot();
				xsh.Transform(nav, null, new MemoryStream());
			}
			end = DateTime.Now.Ticks;

			//Stream o = Console.OpenStandardOutput();
			//xsh.Transform(nav, null, o);
			////Console.WriteLine(string.Empty);
			////Console.WriteLine(string.Empty);
			////Console.WriteLine(string.Empty);
            
			//Console.WriteLine("XSLT execution : {0}", TimeSpan.FromTicks(end - start).TotalMilliseconds);
			//Console.WriteLine(string.Empty);
			//Console.WriteLine(string.Empty);

			Validator val = new Validator();
			val.AddSchema(sch);
			//val.EvaluationContext.Formatter = new Formatters.BooleanFormatter();

			//Schematron execution.
			start = DateTime.Now.Ticks;
			for (int i = 0; i < iterations; i++)
			{
				nav.MoveToRoot();
				try
				{
					val.ValidateSchematron(nav);
				}
				catch {}
			}
			end = DateTime.Now.Ticks;

			try
			{
				val.ValidateSchematron(nav);
			}
			catch (ValidationException ex)
			{
				//Console.Write(ex.Message);
			}

			//Console.WriteLine("Schematron execution : {0}", TimeSpan.FromTicks(end - start).TotalMilliseconds);
			//Console.WriteLine(String.Empty);
		}
		*/

        /// <summary>
        /// Standalone schematron schema validating another schematron schema.
        /// </summary>
        /// <remarks>
        /// Metastylesheets and validating schema preloaded.
        /// Embedded schematron validation (for the WXS part) is not 
        /// benchmarked as XSLT implementation only performs schematron validation.
        /// </remarks>
        public static void SchematronSchemaExec(object it)
        {
			int iterations = (int) it;
			Console.WriteLine("SchematronSchemaExec " + iterations.ToString());
			Debug.Listeners["SchematronSchemaExec"].Write(iterations.ToString() + ";");

			//Console.WriteLine(new String('-', 60));
			//Console.WriteLine(new String('-', 20) + " SchematronSchemaExec " + new string('-', 20));
			//Console.WriteLine(new String('-', 60));
			
			XPathDocument doc = new XPathDocument(@"..\Files\tournament-schema.xml");

            Schema sch = new Schema();
            sch.Load(@"..\Files\schematron1-5.sch");

            XslTransform meta = new XslTransform();
            meta.Load(@"..\Files\diag.xsl");
            MemoryStream mem = new MemoryStream();
            XmlTextWriter xw = new XmlTextWriter(mem, System.Text.Encoding.UTF8);
            meta.Transform(new XPathDocument(@"..\Files\schematron1-5.sch"), null, xw);

            mem.Seek(0, SeekOrigin.Begin);
			XmlDocument xsldoc = new XmlDocument();
			xsldoc.Load(mem);
			mem.Close();

            XslTransform xsh = new XslTransform();
            xsh.Load(xsldoc);
 
            XPathNavigator nav = doc.CreateNavigator();
 
            long start;
            long end;

            //XSLT execution.
            start = DateTime.Now.Ticks;
            for (int i = 0; i < iterations; i++)
            {
                nav.MoveToRoot();
                xsh.Transform(nav, null, new MemoryStream());
            }
            end = DateTime.Now.Ticks;

            //Stream o = Console.OpenStandardOutput();
            //xsh.Transform(nav, null, o);
            //Console.WriteLine(string.Empty);
			//Console.WriteLine(string.Empty);
            
			//Console.WriteLine("XSLT execution : {0}", TimeSpan.FromTicks(end - start).TotalMilliseconds);
			//Console.WriteLine(new String('-', 60));
            //Console.WriteLine(string.Empty);
			Debug.Listeners["SchematronSchemaExec"].Write((TimeSpan.FromTicks(end - start).TotalMilliseconds).ToString() + ";");
			
			Validator val = new Validator();
			val.AddSchema(sch);
			//val.EvaluationContext.Formatter = new Formatters.BooleanFormatter();

            //Schematron execution.
            start = DateTime.Now.Ticks;
            for (int i = 0; i < iterations; i++)
            {
                nav.MoveToRoot();
				try
				{
					val.ValidateSchematron(nav);
				}
				catch {}
            }
            end = DateTime.Now.Ticks;

			/*
			try
			{
				val.ValidateSchematron(nav);
			}
			catch (ValidationException ex)
			{
				Console.Write(ex.Message);
			}
			*/
			//Console.WriteLine(string.Empty);
			//Console.WriteLine(string.Empty);

            //Console.WriteLine("Schematron execution : {0}", TimeSpan.FromTicks(end - start).TotalMilliseconds);
			//Console.WriteLine(new String('-', 60));
			//Console.WriteLine(string.Empty);
			Debug.Listeners["SchematronSchemaExec"].Write((TimeSpan.FromTicks(end - start).TotalMilliseconds).ToString() + ";");

			// **** MSXML4 preparation
//			MSXML2.FreeThreadedDOMDocument msdoc = new MSXML2.FreeThreadedDOMDocument40Class() as FreeThreadedDOMDocument;
//			msdoc.async = false;
//			msdoc.load(@"..\Files\tournament-schema.xml");
//
//			MSXML2.FreeThreadedDOMDocument xdoc = new MSXML2.FreeThreadedDOMDocument40Class() as FreeThreadedDOMDocument;
//			xdoc.async = false;
//			xdoc.loadXML(xsldoc.DocumentElement.OuterXml);
//
//			//MSXML4 execution.
//			start = DateTime.Now.Ticks;
//			for (int i = 0; i < iterations; i++)
//			{
//				msdoc.transformNode(xdoc);
//			}
//			end = DateTime.Now.Ticks;

			//Console.WriteLine(msdoc.transformNode(xdoc));
			//Console.WriteLine(string.Empty);
			//Console.WriteLine(string.Empty);

			//Console.WriteLine("MSXML4 execution : {0}", TimeSpan.FromTicks(end - start).TotalMilliseconds);
			//Console.WriteLine(new String('-', 60));
			//Console.WriteLine(string.Empty);
//			Debug.Listeners["SchematronSchemaExec"].WriteLine((TimeSpan.FromTicks(end - start).TotalMilliseconds).ToString());
			Debug.Listeners["SchematronSchemaExec"].Flush();
		}

		/// <summary>
		/// Embedded schematron schema extraction and loading.
		/// </summary>
		/// <remarks>
		/// Metastylesheets preloaded.
		/// In XSLT timing we take into accound that the WXS has to be read too,
		/// becasue we want to validate both.
		/// </remarks>
		public static void EmbeddedSchemaExtractionAndLoading(object it)
		{
			int iterations = (int) it;
			Console.WriteLine("EmbeddedSchemaExtractionAndLoading " + iterations.ToString());
			Debug.Listeners["EmbeddedSchemaExtractionAndLoading"].Write(iterations.ToString() + ";");
			//Console.WriteLine(new String('-', 60));
			//Console.WriteLine(new string('-', 20) + " EmbeddedSchemaExtractionAndLoading " + new string('-', 20));
			//Console.WriteLine(new String('-', 60));

			//Metastylesheets preloaded (test loading these too)
			XslTransform xsd = new XslTransform();
			xsd.Load(@"..\Files\xsd.xsl");
			XslTransform meta = new XslTransform();
			meta.Load(@"..\Files\diag.xsl");

			XslTransform xsh = null;

			long start;
			long end;

			//XSLT extraction and schema loading.
			start = DateTime.Now.Ticks;
			for (int i = 0; i < iterations; i++)
			{
				using (FileStream fs = new FileStream((@"..\Files\Tournament.xsd"), FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					XmlSchema s = XmlSchema.Read(fs, null);
					fs.Seek(0, SeekOrigin.Begin);

					MemoryStream mem = new MemoryStream();
					XmlTextWriter xw = new XmlTextWriter(mem, System.Text.Encoding.UTF8);
					xsd.Transform(new XPathDocument(fs), null, xw);
					mem.Seek(0, SeekOrigin.Begin);

					MemoryStream ms = new MemoryStream();
					xw = new XmlTextWriter(ms, System.Text.Encoding.UTF8);
					meta.Transform(new XPathDocument(mem), null, ms);
					
					ms.Seek(0, SeekOrigin.Begin);
					xsh = new XslTransform();
					xsh.Load(new XPathDocument(ms));
				}
			}
			end = DateTime.Now.Ticks;

			//Stream o = Console.OpenStandardOutput();
			//xsh.Transform(new XPathDocument(@"..\Files\Tournament.xml"), null, o);
			////Console.WriteLine(string.Empty);
			////Console.WriteLine(string.Empty);
			////Console.WriteLine(string.Empty);

			//Console.WriteLine("XSLT loading : {0}", TimeSpan.FromTicks(end - start).TotalMilliseconds);
			//Console.WriteLine(new String('-', 60));
			//Console.WriteLine(string.Empty);
			Debug.Listeners["EmbeddedSchemaExtractionAndLoading"].Write((TimeSpan.FromTicks(end - start).TotalMilliseconds).ToString() + ";");

			Validator val = null;

			//Schematron loading.
			start = DateTime.Now.Ticks;
			for (int i = 0; i < iterations; i++)
			{
				val = new Validator();
				val.AddSchema(@"..\Files\Tournament.xsd");
			}
			end = DateTime.Now.Ticks;

			/*
			try
			{
				val.Validate(@"..\Files\Tournament.xml");
			}
			catch (ValidationException ex)
			{
				//Console.Write(ex.Message);
			}
			*/

			//Console.WriteLine("Schematron loading : {0}", TimeSpan.FromTicks(end - start).TotalMilliseconds);
			//Console.WriteLine(new String('-', 60));
			//Console.WriteLine(string.Empty);
			Debug.Listeners["EmbeddedSchemaExtractionAndLoading"].Write((TimeSpan.FromTicks(end - start).TotalMilliseconds).ToString() + ";");


			// **** MSXML4 preparation
//			MSXML2.FreeThreadedDOMDocument msxsd = new MSXML2.FreeThreadedDOMDocument40Class() as FreeThreadedDOMDocument;
//			msxsd.async = false;
//			msxsd.load(@"..\Files\xsd.xsl");
//
//			MSXML2.FreeThreadedDOMDocument msmeta = new MSXML2.FreeThreadedDOMDocument40Class() as FreeThreadedDOMDocument;
//			msmeta.async = false;
//			msmeta.load(@"..\Files\diag.xsl");
//
//			MSXML2.FreeThreadedDOMDocument msval = new MSXML2.FreeThreadedDOMDocument40Class() as FreeThreadedDOMDocument;
//			MSXML2.FreeThreadedDOMDocument msdoc = null;
//
//			//XSLT extraction and schema loading.
//			start = DateTime.Now.Ticks;
//			for (int i = 0; i < iterations; i++)
//			{
//				XmlSchema s = XmlSchema.Read(new XmlTextReader(@"..\Files\Tournament.xsd"), null);
//				msdoc = new FreeThreadedDOMDocument40Class() as FreeThreadedDOMDocument;
//				msdoc.load(@"..\Files\Tournament.xsd");
//				msval.loadXML(msdoc.transformNode(msxsd));
//				msdoc.loadXML(msval.transformNode(msmeta));
//			}
//			end = DateTime.Now.Ticks;

			//msval.load(@"..\Files\Tournament.xml");
			////Console.WriteLine(msval.transformNode(msdoc));
			////Console.WriteLine(string.Empty);
			////Console.WriteLine(string.Empty);

			//Console.WriteLine("MSXML4 execution : {0}", TimeSpan.FromTicks(end - start).TotalMilliseconds);
			//Console.WriteLine(new String('-', 60));
			//Console.WriteLine(string.Empty);
//			Debug.Listeners["EmbeddedSchemaExtractionAndLoading"].WriteLine((TimeSpan.FromTicks(end - start).TotalMilliseconds).ToString());
			Debug.Listeners["EmbeddedSchemaExtractionAndLoading"].Flush();
		}

		/// <summary>
		/// Standalone schematron schema loading.
		/// </summary>
		/// <remarks>
		/// Metastylesheet preloaded.
		/// </remarks>
		public static void StandaloneSchemaLoading(object it)
		{
			int iterations = (int) it;
			Console.WriteLine("StandaloneSchemaLoading " + iterations.ToString());
			Debug.Listeners["StandaloneSchemaLoading"].Write(iterations.ToString() + ";");
			//Console.WriteLine(new String('-', 60));
			//Console.WriteLine(new string('-', 20) + " StandaloneSchemaLoading " + new string('-', 20));
			//Console.WriteLine(new String('-', 60));

			XslTransform meta = new XslTransform();
			meta.Load(@"..\Files\diag.xsl");
 
			long start;
			long end;

			//XSLT execution.
			start = DateTime.Now.Ticks;
			for (int i = 0; i < iterations; i++)
			{
				MemoryStream mem = new MemoryStream();
				XmlTextWriter xw = new XmlTextWriter(mem, System.Text.Encoding.UTF8);           
				meta.Transform(new XPathDocument(@"..\Files\schematron1-5.sch"), null, xw);

				mem.Seek(0, SeekOrigin.Begin);
				XslTransform xsh = new XslTransform();
				xsh.Load(new XPathDocument(mem));
			}
			end = DateTime.Now.Ticks;

			//Console.WriteLine("XSLT preparation time : {0}", TimeSpan.FromTicks(end - start).TotalMilliseconds);
			//Console.WriteLine(new String('-', 60));
			//Console.WriteLine(string.Empty);
			Debug.Listeners["StandaloneSchemaLoading"].Write((TimeSpan.FromTicks(end - start).TotalMilliseconds).ToString() + ";");

			//Schematron execution.
			start = DateTime.Now.Ticks;
			for (int i = 0; i < iterations; i++)
			{
				Schema sch = new Schema();
				sch.Load(@"..\Files\schematron1-5.sch");
			}
			end = DateTime.Now.Ticks;

			//Console.WriteLine("Schematron preparation time : {0}", TimeSpan.FromTicks(end - start).TotalMilliseconds);
			//Console.WriteLine(new String('-', 60));
			//Console.WriteLine(string.Empty);
			Debug.Listeners["StandaloneSchemaLoading"].Write((TimeSpan.FromTicks(end - start).TotalMilliseconds).ToString() + ";");

		
			// **** MSXML4 preparation
//			MSXML2.FreeThreadedDOMDocument msmeta = new MSXML2.FreeThreadedDOMDocument40Class() as FreeThreadedDOMDocument;
//			msmeta.async = false;
//			msmeta.load(@"..\Files\diag.xsl");
//
//			MSXML2.FreeThreadedDOMDocument msval = new MSXML2.FreeThreadedDOMDocument40Class() as FreeThreadedDOMDocument;
//			MSXML2.FreeThreadedDOMDocument msdoc = null;
//
//			//MSXSLT execution.
//			start = DateTime.Now.Ticks;
//			for (int i = 0; i < iterations; i++)
//			{
//				msdoc = new FreeThreadedDOMDocument40Class() as FreeThreadedDOMDocument;
//				msdoc.load(@"..\Files\schematron1-5.sch");
//				msval.loadXML(msdoc.transformNode(msmeta));
//			}
//			end = DateTime.Now.Ticks;

			////Console.WriteLine(msdoc.transformNode(msmeta));
			////Console.WriteLine(string.Empty);
			////Console.WriteLine(string.Empty);

			//Console.WriteLine("MSXML4 preparation time : {0}", TimeSpan.FromTicks(end - start).TotalMilliseconds);
			//Console.WriteLine(new String('-', 60));
			//Console.WriteLine(string.Empty);		
//			Debug.Listeners["StandaloneSchemaLoading"].WriteLine((TimeSpan.FromTicks(end - start).TotalMilliseconds).ToString());
			Debug.Listeners["StandaloneSchemaLoading"].Flush();
		}

        /// <summary />
        public static void SelectDescendentsSpeed(object it)
		{
			int iterations = (int) it;

			long start;
			long end;
			XPathNavigator context = new XPathDocument(@"..\data.xml").CreateNavigator();
			XPathExpression expr = context.Compile("descendant-or-self::publishers");
			context.MoveToFirstChild();
           
			start = DateTime.Now.Ticks;
			for (int i = 0; i < iterations; i++)
			{
				context.Select(expr);
			}
			end = DateTime.Now.Ticks;
			//Console.WriteLine("Elapsed time for compiled expression select: {0}", TimeSpan.FromTicks(end - start).TotalMilliseconds);

			string ns = Schema.Namespace;

			start = DateTime.Now.Ticks;
			for (int i = 0; i < iterations; i++)
			{
				context.SelectDescendants("publisher", ns, true);
			}
			end = DateTime.Now.Ticks;
			//Console.WriteLine("Elapsed time for descendents select: {0}", TimeSpan.FromTicks(end - start).TotalMilliseconds);
		}

        /// <summary />
        public static void SelectChildrenSpeed(int it)
		{
			int iterations = (int) it;
			long start;
			long end;
			XPathNavigator context = new XPathDocument(@"..\data.xml").CreateNavigator();
			context.MoveToFirstChild();
			XPathExpression expr = context.Compile("child::sch:assert");
			XmlNamespaceManager mgr = new XmlNamespaceManager(context.NameTable);
			mgr.AddNamespace("dv", "deverest:schemas");
			mgr.AddNamespace("sch", Schema.Namespace);
			expr.SetContext(mgr);
           
			start = DateTime.Now.Ticks;
			for (int i = 0; i < iterations; i++)
			{
				context.Select(expr);
			}
			end = DateTime.Now.Ticks;
			//Console.WriteLine("Elapsed time for compiled expression select: {0}", TimeSpan.FromTicks(end - start).TotalMilliseconds);

			string ns = Schema.Namespace;

			start = DateTime.Now.Ticks;
			for (int i = 0; i < iterations; i++)
			{
				context.SelectChildren("assert", ns);
			}
			end = DateTime.Now.Ticks;
			//Console.WriteLine("Elapsed time for children select: {0}", TimeSpan.FromTicks(end - start).TotalMilliseconds);
		}
	}
}
