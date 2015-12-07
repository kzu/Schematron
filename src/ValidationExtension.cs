using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Web.Services.Protocols;

namespace NMatrix.Schematron
{
	/// <summary>
	/// Extension that allows Schematron validation on webmethods.
	/// </summary>
	internal class ValidationExtension : SoapExtension
	{
		ValidationExtension.ExtensionInitializer initializer;

		public override object GetInitializer(Type serviceType)
		{
			throw new NotImplementedException("Schematron validation must be specified through the ValidationAttribute.");
		}

		public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
		{
			if (!(attribute is ValidationAttribute)) 
				throw new NotSupportedException("Schematron validation must be enabled through the ValidationAttribute.");

			return new ExtensionInitializer(methodInfo.DeclaringType.Assembly, (ValidationAttribute)attribute);
		}

		public override void Initialize(object initializer)
		{
			this.initializer = (ExtensionInitializer) initializer;
		}

		public override void ProcessMessage(SoapMessage message)
		{
			if (message is SoapClientMessage)
				throw new NotSupportedException("Schematron validation is only available on the server side.");

			if (message.Stage == SoapMessageStage.BeforeDeserialize)
			{
				Validator v = new Validator(initializer.Validation.Formatting);
				if (initializer.Formatter != null)
					v.Formatter = initializer.Formatter;

				// Add cached schemas.
				v.Schemas.AddRange(initializer.SchematronSchemas);
				v.XmlSchemas.Add(initializer.XmlSchemas);
				if (initializer.Validation.Phase != null)
					v.Phase = initializer.Validation.Phase;

				try
				{
					// Only validate body.
					XmlTextReader reader = new XmlTextReader(message.Stream);
					object body = reader.NameTable.Add("Body");
					reader.MoveToContent();
					// First read will get us into the envelope.
					while (reader.Read())
					{
						if ((object)reader.LocalName == body)
						{
							// Skip the webmethod wrapper element too.
							reader.Read();
							string inner = reader.ReadInnerXml();
							v.Validate(new StringReader(inner));
						}
					}
				}
				catch (ValidationException vex)
				{
					throw new SoapException("Incoming document is invalid.\n" + 
						vex.Message, SoapException.ClientFaultCode);
				}
				finally
				{
					// Reset stream position for normal processing to continue.
					message.Stream.Position = 0;
				}
			}
		}

		#region ExtensionInitializer

		private class ExtensionInitializer
		{
			public Assembly SchemaAssembly;
			public ValidationAttribute Validation;
			public SchemaCollection SchematronSchemas =  new SchemaCollection();
			public XmlSchemaCollection XmlSchemas = new XmlSchemaCollection();
			public Formatters.IFormatter Formatter;
			public ExtensionInitializer(Assembly schemaAssembly, ValidationAttribute validation)
			{
				this.SchemaAssembly = schemaAssembly;
				this.Validation = validation;

				// Use temporary validator to load schemas and validate them at the same time.
				Validator val = new Validator(OutputFormatting.Log);

				// Cache all schemas.
				foreach (string schema in validation.Schemas)
				{
					// First try full resource name.
					Stream memstream = schemaAssembly.GetManifestResourceStream(schema);
					if (memstream == null)
					{
						// Retry appending assembly name.
						string resourcebase = schemaAssembly.FullName.Substring(
							0, schemaAssembly.FullName.IndexOf(','));
						memstream = schemaAssembly.GetManifestResourceStream(
							resourcebase + "." + schema);
					}

					if (memstream == null) 
						throw new FileNotFoundException("Schema not found.", schema);
					
					XmlTextReader reader = new XmlTextReader(memstream);
					val.AddSchema(reader);
				}

				// Pull schemas out so that validator can be GC-collected;
				XmlSchema[] xsds = new XmlSchema[val.XmlSchemas.Count];
				val.XmlSchemas.CopyTo(xsds, 0);
				Schema[] schs = new Schema[val.Schemas.Count];
				val.Schemas.CopyTo(schs, 0);

				SchematronSchemas.AddRange(schs);
				foreach (XmlSchema xsd in xsds)
				{
					XmlSchemas.Add(xsd);
				}

				// Detemine custom formatter.
				if (validation.FormattingType != null)
				{
					if (!typeof(Formatters.IFormatter).IsAssignableFrom(validation.FormattingType))
					{
						throw new ArgumentException("Assigned formatter type " + validation.FormattingType.FullName + 
							" does not implement the required " + typeof(Formatters.IFormatter).FullName + " interface.");
					}
					// Try to retrieve empty constructor
					ConstructorInfo ctor = validation.FormattingType.GetConstructor(Type.EmptyTypes);
					if (ctor == null)
					{
						throw new ArgumentException("Assigned formatter type " + validation.FormattingType.FullName + 
							" doesn't have a parameterless constructor");
					}

					Formatter = (Formatters.IFormatter) Activator.CreateInstance(validation.FormattingType);
				}
			}
		}

		#endregion ExtensionInitializer
	}
}
