using System;
using System.Xml.XPath;
using NMatrix.Schematron;

namespace Test
{
	class Entry
	{
		[MTAThread]
		static void Main(string[] args)
		{
			try
			{
				Validator val = new Validator();
				val.AddSchema(@"..\Files\po.xsd");

				try
				{
					XPathDocument doc = (XPathDocument) 
						val.Validate(@"..\Files\po-bad.xml");
					// Continue processing valid document.
				}
				catch (ValidationException ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
			catch (Exception ex)
			{
				Console.Write(ex);
			}
		
			Console.WriteLine("Finished");
			Console.Read();
		}
	}
}
