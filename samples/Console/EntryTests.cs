using System;
using System.Xml.XPath;
using NMatrix.Schematron;
using NMatrix.Schematron.Tests;
using System.Diagnostics;
using System.IO;
using System.Threading;

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
					XPathDocument doc = (XPathDocument) val.Validate(@"..\Files\po-bad.xml");
					// Continue processing valid document.
				}
				catch (ValidationException ex)
				{
					Console.WriteLine(ex.Message);
				}

				/*
				if (args.Length == 0)
					Driver.Run(@"..\Files\Tournament.xml", @"..\Files\tournament-schema.sch");
					//NMatrix.Schematron.Driver.Run(@"..\data.xml", @"..\Files\po.xsd");
				else if (args.Length == 1)
					Driver.Run(@"..\Files\Tournament.xml", @"..\Files\tournament-schema.sch", args[0]);
					//NMatrix.Schematron.Driver.Run(@"..\Files\po-bad.xml", @"..\Files\po-schema.sch", args[0]);
					//NMatrix.Schematron.Driver.Run(@"..\Files\po-bad.xml", @"..\Files\po.xsd", args[0]);
				else
					NMatrix.Schematron.Driver.Run(args[0], args[1], args[2]);

				//Force initialization of Schematron static members
				Config.Setup();

				top = Int32.Parse(args[0]);

				if (File.Exists(@"..\Files\EmbeddedSchemaExtractionAndLoading.txt"))
					File.Delete(@"..\Files\EmbeddedSchemaExtractionAndLoading.txt");

				if (File.Exists(@"..\Files\StandaloneSchemaLoading.txt"))
					File.Delete(@"..\Files\StandaloneSchemaLoading.txt");

				if (File.Exists(@"..\Files\SchematronSchemaExec.txt"))
					File.Delete(@"..\Files\SchematronSchemaExec.txt");

				Debug.Listeners.Add(new TextWriterTraceListener(@"..\Files\EmbeddedSchemaExtractionAndLoading.txt", "EmbeddedSchemaExtractionAndLoading"));
				Debug.Listeners["EmbeddedSchemaExtractionAndLoading"].WriteLine("Iterations;XslTransform;Schematron;MSXML");

				Debug.Listeners.Add(new TextWriterTraceListener(@"..\Files\StandaloneSchemaLoading.txt", "StandaloneSchemaLoading"));
				Debug.Listeners["StandaloneSchemaLoading"].WriteLine("Iterations;XslTransform;Schematron;MSXML");

				Debug.Listeners.Add(new TextWriterTraceListener(@"..\Files\SchematronSchemaExec.txt", "SchematronSchemaExec"));
				Debug.Listeners["SchematronSchemaExec"].WriteLine("Iterations;XslTransform;Schematron;MSXML");

				//Synchonous call.
				//StartEmbeddedSchemaExtractionAndLoading(null);
				StartSchematronSchemaExec(null);
				StartStandaloneSchemaLoading(null);
				*/

				/*
				//Explicit thread creation. Throws sometimes. (we removed the object state param for these calls)
				Thread t;
				t = new Thread(new ThreadStart(StartEmbeddedSchemaExtractionAndLoading));
				t.Start();
				t = new Thread(new ThreadStart(StartSchematronSchemaExec));
				t.Start();
				t = new Thread(new ThreadStart(StartStandaloneSchemaLoading));
				t.Start();
				*/

				/*
				//ThreadPool way...
				ThreadPool.QueueUserWorkItem(new WaitCallback(StartEmbeddedSchemaExtractionAndLoading));
				ThreadPool.QueueUserWorkItem(new WaitCallback(StartSchematronSchemaExec));
				ThreadPool.QueueUserWorkItem(new WaitCallback(StartStandaloneSchemaLoading));
				*/
			}
			catch (Exception ex)
			{
				Console.Write(ex);
			}
		
			Console.WriteLine("Finished");
			Console.Read();
		}

		static int top;

		private static void StartEmbeddedSchemaExtractionAndLoading(object state)
		{
			for (int i = 0; i <= top; i += 10)
			{
				try
				{
					Benchmarks.EmbeddedSchemaExtractionAndLoading(i);
					//ThreadPool.QueueUserWorkItem(new WaitCallback(Benchmarks.EmbeddedSchemaExtractionAndLoading), i);
				}
				catch {}
				// Force to avoid impact during time calculation.
				GC.Collect();
			}
			Console.WriteLine("--> Finished EmbeddedSchemaExtractionAndLoading");
		}

		private static void StartStandaloneSchemaLoading(object state)
		{
			for (int i = 0; i <= top; i += 10)
			{
				try
				{
					Benchmarks.StandaloneSchemaLoading(i);
					//ThreadPool.QueueUserWorkItem(new WaitCallback(Benchmarks.StandaloneSchemaLoading), i);
				}
				catch {}
				// Force to avoid impact during time calculation.
				GC.Collect();
			}
			Console.WriteLine("--> Finished StandaloneSchemaLoading");
		}

		private static void StartSchematronSchemaExec(object state)
		{
			for (int i = 0; i <= top; i += 10)
			{
				try
				{
					Benchmarks.SchematronSchemaExec(i);
					//ThreadPool.QueueUserWorkItem(new WaitCallback(Benchmarks.SchematronSchemaExec), i);
				} 
				catch {}
				// Force to avoid impact during time calculation.
				GC.Collect();
			}
			Console.WriteLine("--> Finished SchematronSchemaExec");
		}
	}
}
