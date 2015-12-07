using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Collections;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using NMatrix.Schematron.Formatters;

namespace NMatrix.Schematron
{
	/// <summary>
	/// Performs validation of Schematron elements and schemas.
	/// </summary>
	/// <remarks>
	/// Can handle either standalone or embedded schematron schemas. If the schematron
	/// is embedded in an XML Schema, the input document is validated against both at
	/// the same time.
	/// </remarks>
	public class Validator
	{
		#region Fields & Ctors

		XmlSchemaCollection _xmlschemas = new XmlSchemaCollection();
		SchemaCollection _schematrons = new SchemaCollection();
		EvaluationContextBase _evaluationctx;
		NavigableType _navtype = NavigableType.XPathDocument;

		StringBuilder _errors;
		bool _haserrors;

		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		public Validator()
		{
			_evaluationctx = CreateContext();
			_evaluationctx.Formatter = Config.DefaultFormatter;
		}

		/// <summary>
		/// Initializes a new instance of the class, using the specified output format for error messages.
		/// </summary>
		/// <param name="format">Output format of error messages.</param>
		public Validator(OutputFormatting format) : this()
		{
			InitValidator(format, NavigableType.Default);
		}

		/// <summary>
		/// Initializes a new instance of the class, using the specified return type.
		/// </summary>
		/// <param name="type">The <see cref="IXPathNavigable"/> type to use for validation and return type.</param>
		public Validator(NavigableType type) : this()
		{
			InitValidator(OutputFormatting.Default, type);
		}

		/// <summary>
		/// Initializes a new instance of the class, using the specified options.
		/// </summary>
		/// <param name="format">Output format of error messages.</param>
		/// <param name="type">The <see cref="IXPathNavigable"/> type to use for validation and return type.</param>
		public Validator(OutputFormatting format, NavigableType type) : this()
		{
			InitValidator(format, type);
		}

		#endregion Fields & Ctors

		/// <summary>
		/// Initializes the validator with the options received from the constructor overloads.
		/// </summary>
		/// <param name="format">Output format of error messages.</param>
		/// <param name="type">The <see cref="IXPathNavigable"/> type to use for validation and return type.</param>
		private void InitValidator(OutputFormatting format, NavigableType type)
		{
			if (!Enum.IsDefined(typeof(OutputFormatting), format)) 
				throw new ArgumentException("Invalid type.", "type"); 

			switch (format)
			{
				case OutputFormatting.Boolean:
					_evaluationctx.Formatter = new BooleanFormatter();
					break;
				case OutputFormatting.Log:
				case OutputFormatting.Default:
					_evaluationctx.Formatter = new LogFormatter();
					break;
				case OutputFormatting.Simple:
					_evaluationctx.Formatter = new SimpleFormatter();
					break;
				case OutputFormatting.XML:
					_evaluationctx.Formatter = new XmlFormatter();
					break;
			}

			if (!Enum.IsDefined(typeof(NavigableType), type)) 
				throw new ArgumentException("Invalid type.", "type"); 

			// If type is Default, set it to XPathDocument.
			_navtype = (type != NavigableType.Default) ? type : NavigableType.XPathDocument;
		}

		#region Overridable Factory Methods
		/// <summary>Creates the evaluation context to use.</summary>
		/// <remarks>
		/// Inheritors can override this method should they want to 
		/// use a different strategy for node traversal and evaluation
		/// against the source file.
		/// </remarks>
		protected virtual EvaluationContextBase CreateContext()
		{
			return new SyncEvaluationContext();
		}
		#endregion

		#region Properties
		/// <summary />
		public EvaluationContextBase Context
		{
			get { return _evaluationctx; }
			set { _evaluationctx = value; }
		}

		/// <summary />
		public IFormatter Formatter
		{
			get { return _evaluationctx.Formatter; }
			set { _evaluationctx.Formatter = value; }
		}

		/// <summary />
		public NavigableType ReturnType
		{
			get { return _navtype; }
			set 
			{ 
				if (!Enum.IsDefined(typeof(NavigableType), value))
					throw new ArgumentException("NavigableType value is not defined.");
				_navtype = value; 
			}
		}

		/// <summary />
		public string Phase
		{
			get { return _evaluationctx.Phase; }
			set { _evaluationctx.Phase = value; }
		}

		/// <summary>
		/// Exposes the schematron schemas to use for validation.
		/// </summary>
		public SchemaCollection Schemas
		{
			get { return _schematrons; }
		}

		/// <summary>
		/// Exposes the XML schemas to use for validation.
		/// </summary>
		public XmlSchemaCollection XmlSchemas
		{
			get { return _xmlschemas; }
		}

		#endregion

		#region AddSchema overloads
		/// <summary>
		/// Adds an XML Schema to the collection to use for validation.
		/// </summary>
		public void AddSchema(XmlSchema schema)
		{
			_xmlschemas.Add(schema);
		}

		/// <summary>
		/// Adds a Schematron schema to the collection to use for validation.
		/// </summary>
		public void AddSchema(Schema schema)
		{
			_schematrons.Add(schema);
		}

		/// <summary>
		/// Adds a set of XML Schemas to the collection to use for validation.
		/// </summary>
		public void AddSchemas(XmlSchemaCollection schemas)
		{
			_xmlschemas.Add(schemas);
		}

		/// <summary>
		/// Adds a set of Schematron schemas to the collection to use for validation.
		/// </summary>
		public void AddSchemas(SchemaCollection schemas)
		{
			_schematrons.AddRange(schemas);
		}

		/// <summary>
		/// Adds a schema to the collection to use for validation from the specified URL.
		/// </summary>
		public void AddSchema(string uri)
		{
			using (FileStream fs = new FileStream(uri, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				AddSchema(new XmlTextReader(fs));
			}
		}

		/// <summary>
		/// Adds a schema to the collection to use for validation.
		/// </summary>
		public void AddSchema(TextReader reader)
		{
			AddSchema(new XmlTextReader(reader));
		}

		/// <summary>
		/// Adds a schema to the collection to use for validation.
		/// </summary>
		public void AddSchema(Stream input)
		{
			AddSchema(new XmlTextReader(input));
		}
	
		/// <summary>
		/// Adds a schema to the collection to use for validation.
		/// </summary>
		/// <remarks>Processing takes place here.</remarks>
		public void AddSchema(XmlReader reader)
		{
			if (reader.MoveToContent() == XmlNodeType.None) throw new BadSchemaException("No information found to read");

			// Determine type of schema received.
			bool standalone = (reader.NamespaceURI == Schema.Namespace);
			bool wxs = (reader.NamespaceURI == XmlSchema.Namespace);

			// The whole schema must be read first to preserve the state for later.
			string state = reader.ReadOuterXml();
			StringReader r = new StringReader(state);

			if (wxs)
			{
				_haserrors = false;
				_errors = new StringBuilder();
				XmlSchema xs = XmlSchema.Read(new XmlTextReader(r, reader.NameTable), new ValidationEventHandler(OnValidation));
				if (!xs.IsCompiled) xs.Compile(new ValidationEventHandler(OnValidation));
				if (_haserrors) throw new BadSchemaException(_errors.ToString());

				_xmlschemas.Add(xs);
			}

			//Schemas wouldn't be too big, so they are loaded in an XmlDocument for Schematron validation, so that
			//inner XML elements in messages, etc. are available. So we commented the following lines.
			//r = new StringReader(state);
			//XPathNavigator nav = new XPathDocument(new XmlTextReader(r, reader.NameTable)).CreateNavigator();
			XmlDocument doc = new XmlDocument(reader.NameTable);
			doc.LoadXml(state);
			XPathNavigator nav = doc.CreateNavigator();
			_evaluationctx.Source = nav;

			if (standalone) 
				PerformValidation(Config.FullSchematron);
			else 
				PerformValidation(Config.EmbeddedSchematron);

			if (_evaluationctx.HasErrors)
				throw new BadSchemaException(_evaluationctx.Messages.ToString());

			Schema sch = new Schema();
			sch.Load(nav);
			_schematrons.Add(sch);
			_errors = null;
		}
		#endregion

		#region Validation Methods
		/// <summary>
		/// Performs Schematron-only validation.
		/// </summary>
		/// <remarks>
		/// Even when <see cref="XmlDocument"/> implements IXPathNavigable, WXS
		/// validation can't be performed once it has been loaded becasue a 
		/// validating reader has to be used.
		/// </remarks>
		/// <exception cref="ValidationException">
		/// The document is invalid with respect to the loaded schemas.
		/// </exception>
		public void ValidateSchematron(IXPathNavigable source)
		{
			ValidateSchematron(source.CreateNavigator());
		}

		/// <summary>
		/// Performs Schematron-only validation.
		/// </summary>
		/// <exception cref="ValidationException">
		/// The document is invalid with respect to the loaded schemas.
		/// </exception>
		public void ValidateSchematron(XPathNavigator file)
		{
			_errors = new StringBuilder();
			_evaluationctx.Source = file;

			foreach (Schema sch in _schematrons)
			{
				PerformValidation(sch);
				if (_evaluationctx.HasErrors)
				{
					_haserrors = true;
					_errors.Append(_evaluationctx.Messages.ToString());
				}
			}

			if (_haserrors) 
			{
				_evaluationctx.Formatter.Format(_errors);
				throw new ValidationException(_errors.ToString());
			}

			if (_haserrors) throw new ValidationException(_errors.ToString());
		}

		/// <summary>Performs validation of the document at the specified URI.</summary>
		/// <param name="uri">The document location.</param>
		/// <exception cref="ValidationException">
		/// The document is invalid with respect to the loaded schemas.
		/// </exception>
		/// <returns>The loaded <see cref="IXPathNavigable"/> instance.</returns>
		public IXPathNavigable Validate(string uri)
		{
			using (FileStream fs = new FileStream(uri, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				return Validate(new XmlTextReader(fs));
			}			
		}

		/// <summary>Performs validation of the document using the specified reader.</summary>
		/// <param name="reader">The reader pointing to the document to validate.</param>
		/// <returns>The loaded <see cref="IXPathNavigable"/> instance.</returns>
		/// <exception cref="ValidationException">
		/// The document is invalid with respect to the loaded schemas.
		/// </exception>
		/// <returns>The loaded <see cref="IXPathNavigable"/> instance.</returns>
		public IXPathNavigable Validate(TextReader reader)
		{
			return Validate(new XmlTextReader(reader));
		}

		/// <summary>Performs validation of the document using the specified stream.</summary>
		/// <param name="input">The stream with the document to validate.</param>
		/// <exception cref="ValidationException">
		/// The document is invalid with respect to the loaded schemas.
		/// </exception>
		/// <returns>The loaded <see cref="IXPathNavigable"/> instance.</returns>
		public IXPathNavigable Validate(Stream input)
		{
			return Validate(new XmlTextReader(input));
		}

		/// <summary>Performs validation of the document using the received reader.</summary>
		/// <remarks>Where the actual work takes place</remarks>
		/// <param name="reader">The reader pointing to the document to validate.</param>
		/// <exception cref="ValidationException">
		/// The document is invalid with respect to the loaded schemas.
		/// </exception>
		/// <returns>The loaded <see cref="IXPathNavigable"/> instance.</returns>
		public IXPathNavigable Validate(XmlReader reader)
		{
			try
			{
				_errors = new StringBuilder();

				// Temporary variables to hold error flags and messages.
				bool hasxml = false;
				StringBuilder xmlerrors = null;
				bool hassch = false;
				StringBuilder scherrors = null;

				XmlValidatingReader r = new XmlValidatingReader(reader);
				foreach (XmlSchema xsd in _xmlschemas)
				{
					r.Schemas.Add(xsd);
				}
				r.ValidationEventHandler += new ValidationEventHandler(OnValidation);
				
				IXPathNavigable navdoc;
				XPathNavigator nav;

				if (_navtype == NavigableType.XmlDocument)
				{
					navdoc = new XmlDocument(r.NameTable);
					((XmlDocument)navdoc).Load(r);
				}
				else
				{
					navdoc = new XPathDocument(r);
				}

				nav = navdoc.CreateNavigator();

				if (_haserrors)
				{
                    _evaluationctx.Formatter.Format(r.Schemas, _errors);
					_evaluationctx.Formatter.Format(r, _errors);
					hasxml = true;
					xmlerrors = _errors;
				}

				_evaluationctx.Source = nav;

				// Reset shared variables
				_haserrors = false;
				_errors = new StringBuilder();
				
				foreach (Schema sch in _schematrons)
				{
					PerformValidation(sch);
					if (_evaluationctx.HasErrors)
					{
						_haserrors = true;
						_errors.Append(_evaluationctx.Messages.ToString());
					}
				}

				if (_haserrors) 
				{
					_evaluationctx.Formatter.Format(_schematrons, _errors);
					hassch = true;
					scherrors = _errors;
				}

				_errors = new StringBuilder();
				if (hasxml) _errors.Append(xmlerrors.ToString());
				if (hassch) _errors.Append(scherrors.ToString());

				if (hasxml || hassch) 
				{
					_evaluationctx.Formatter.Format(_errors);
					throw new ValidationException(_errors.ToString());
				}

				return navdoc;
			}
			catch
			{
				// Rethrow.
				throw;
			}
			finally
			{
				reader.Close();
			}
		}
		#endregion
	
		private void PerformValidation(Schema schema)
		{
			try
			{
				_evaluationctx.Schema = schema;
				//_evaluationctx.Start();
				StartDelegate st = new StartDelegate(_evaluationctx.Start);
				WaitHandle wh = st.BeginInvoke(new AsyncCallback(EndValidation), 
					_evaluationctx).AsyncWaitHandle;
				wh.WaitOne();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Fail(ex.Message, ex.ToString());
				throw ex;
			}
		}

		private void EndValidation(IAsyncResult result)
		{
			try
			{
				AsyncResult ar = (AsyncResult) result;
				StartDelegate st = (StartDelegate) ar.AsyncDelegate;
				st.EndInvoke(ar);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Fail(ex.Message, ex.ToString());
				throw ex;
			}
		}

		private void OnValidation(object sender, ValidationEventArgs e)
		{
			if (!_haserrors) _haserrors = true;
			_evaluationctx.Formatter.Format(e, _errors);
		}
	}
}
