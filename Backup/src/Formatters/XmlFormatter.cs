using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;

namespace NMatrix.Schematron.Formatters
{
	/// <summary>
	/// Provides an Xml output from validation.
	/// </summary>
	public class XmlFormatter : FormatterBase
	{
		/// <summary />
		public XmlFormatter()
		{
		}

		/// <summary>
		/// Namespace of generated output.
		/// </summary>
		public const string OutputNamespace = "http://sourceforge.net/projects/dotnetopensrc/schematron";

		/// <summary>
		/// Look at <see cref="IFormatter.Format"/> documentation.
		/// </summary>
		public override void Format(Test source, XPathNavigator context, StringBuilder output)
		{
			string msg = source.Message;
			XmlTextWriter writer = new XmlTextWriter(new StringWriter(output));
			//Temporary disable namespace support.
			writer.Namespaces = false;
			StringBuilder sb = new StringBuilder();
			XPathExpression expr;

			// Start element declaration.
			writer.WriteStartElement("message");

			// As we move on, we have to append starting from the last point,
			// skipping the <name> expression itself: Substring(offset, name.Index - offset).
			int offset = 0;
			
			for (int i = 0; i < source.NameExpressions.Count; i++)
			{
				Match name = source.NameExpressions[i];
				expr = source.NamePaths[i];

				// Append the text without the expression.
				sb.Append(msg.Substring(offset, name.Index - offset));

				// Does the name element have a path attribute?
				if (expr != null)
				{
					expr.SetContext(source.GetContext());

					string result = null;
					if (expr.ReturnType == XPathResultType.NodeSet)
					{
						// It the result of the expression is a nodeset, we only get the element 
						// name of the first node, which is compatible with XSLT implementation.
						XPathNodeIterator nodes = (XPathNodeIterator)context.Evaluate(expr);
						if (nodes.MoveNext())
							result = nodes.Current.Name;
					}
					else
						result = context.Evaluate(expr) as string;

					if (result != null)
						sb.Append(result);
				}
				else
					sb.Append(context.Name);

				offset = name.Index + name.Length;
			}

			sb.Append(msg.Substring(offset));
			// Finally remove any non-name schematron tag in the message.
			string res = TagExpressions.AllSchematron.Replace(sb.ToString(), String.Empty);

			//Accumulate namespaces found during traversal of node for its position.
			Hashtable ns = new Hashtable();

            // Write <text> element.
			writer.WriteElementString("text", res);
			// Write <path> element.
			writer.WriteElementString("path", FormattingUtils.GetFullNodePosition(context.Clone(), String.Empty, source, ns));
			// Write <summary> element.
			//writer.WriteElementString("summary", FormattingUtils.GetNodeSummary(context, ns, String.Empty));
			writer.WriteStartElement("summary");
			writer.WriteRaw(FormattingUtils.GetNodeSummary(context, ns, String.Empty));
			writer.WriteEndElement();

			// Write <position> element.
			if (context is IXmlLineInfo)
			{
				writer.WriteStartElement("position");
				IXmlLineInfo info = (IXmlLineInfo) context;
				writer.WriteAttributeString("line", info.LineNumber.ToString());
				writer.WriteAttributeString("column", info.LinePosition.ToString());
				writer.WriteEndElement();
			}

			// Close <message> element.
			writer.WriteEndElement();
			writer.Flush();
		}

		/// <summary>
		/// Look at <see cref="IFormatter.Format"/> documentation.
		/// </summary>
		public override void Format(Rule source, XPathNavigator context, StringBuilder output)
		{
			string res = "<rule context=\"" + source.Context + "\" ";
			if (source.Id != String.Empty) res += "id=\"" + source.Id + "\" ";
			res += ">";

			output.Insert(0, res);
			output.Append("</rule>");
		}

		/// <summary>
		/// Look at <see cref="IFormatter.Format"/> documentation.
		/// </summary>
		public override void Format(Pattern source, XPathNavigator context, StringBuilder output)
		{
			string res = "<pattern name=\"" + source.Name + "\" ";
			if (source.Id != String.Empty) res += "id=\"" + source.Id + "\" ";
			res += ">";
            
			output.Insert(0, res);
			output.Append("</pattern>");
		}

		/// <summary>
		/// Look at <see cref="IFormatter.Format"/> documentation.
		/// </summary>
		public override void Format(Phase source, XPathNavigator context, StringBuilder output)
		{
			output.Insert(0, "<phase id=\"" + source.Id + "\">");
			output.Append("</phase>");
		}

		/// <summary>
		/// Look at <see cref="IFormatter.Format"/> documentation.
		/// </summary>
		public override void Format(Schema source, XPathNavigator context, StringBuilder output)
		{
			StringBuilder sb = new StringBuilder();
			XmlTextWriter writer = new XmlTextWriter(new StringWriter(sb));
			writer.WriteStartElement("schema ");

			foreach (string prefix in source.NsManager)
			{
				if (!prefix.StartsWith("xml"))
					writer.WriteAttributeString("xmlns",  prefix, null, 
						source.NsManager.LookupNamespace(source.NsManager.NameTable.Get(prefix)));
			}

			if (source.Title != String.Empty) writer.WriteAttributeString("title", source.Title);

			writer.WriteRaw(output.ToString());
			writer.WriteEndElement();
			writer.Flush();
			output.Remove(0, output.Length);
			output.Append(sb.ToString());
		}

		/// <summary>
		/// Look at <see cref="IFormatter.Format"/> documentation.
		/// </summary>
		public override void Format(ValidationEventArgs source, StringBuilder output)
		{
			XmlTextWriter writer = new XmlTextWriter(new StringWriter(output));
			// Start element declaration.
			writer.WriteStartElement("message");

			// Write <text> element.
			writer.WriteElementString("text", FormattingUtils.XmlErrorPosition.Replace(source.Message, String.Empty));

			// Write <position> element.
			writer.WriteStartElement("position");
			writer.WriteAttributeString("line", source.Exception.LineNumber.ToString());
			writer.WriteAttributeString("column", source.Exception.LinePosition.ToString());
			writer.WriteEndElement();

			// Close <message> element.
			writer.WriteEndElement();
			writer.Flush();
		}

		/// <summary>
		/// Look at <see cref="IFormatter.Format"/> documentation.
		/// </summary>
		public override void Format(XmlSchemaCollection schemas, StringBuilder output)
		{
			StringBuilder sb = new StringBuilder();
			XmlTextWriter writer = new XmlTextWriter(new StringWriter(sb));

			foreach (XmlSchema sch in schemas)
			{
				writer.WriteStartElement("xmlSchema");
				writer.WriteAttributeString("id", sch.Id);
				writer.WriteAttributeString("version", sch.Version);
				writer.WriteAttributeString("targetNamespace", sch.TargetNamespace);
				writer.WriteEndElement();
			}
			writer.Flush();
			output.Insert(0, sb.ToString());
		}

		/// <summary>
		/// Look at <see cref="IFormatter.Format"/> documentation.
		/// </summary>
		public override void Format(SchemaCollection schemas, StringBuilder output)
		{
			// Enclose putput in an <schematron> element.
			output.Insert(0, "<schematron>");
			output.Append("</schematron>");
		}

		/// <summary>
		/// Look at <see cref="IFormatter.Format"/> documentation.
		/// </summary>
		public override void Format(XmlValidatingReader reader, StringBuilder output)
		{
			// Enclose messages in an <xml> element.
			output.Insert(0, "<xml>");
			output.Append("</xml>");
		}

		/// <summary>
		/// Look at <see cref="IFormatter.Format"/> documentation.
		/// </summary>
		public override void Format(StringBuilder output)
		{
			StringBuilder sb = new StringBuilder();
			XmlTextWriter writer = new XmlTextWriter(new StringWriter(sb));

			writer.WriteStartElement("output", OutputNamespace);
            writer.WriteRaw(output.ToString());
			writer.WriteEndElement();
			writer.Flush();

			// Clean output. 
			output.Remove(0, output.Length);

			// Create indented output.
			writer = new XmlTextWriter(new StringWriter(output));
			writer.Formatting = Formatting.Indented;
			writer.WriteStartDocument();
			writer.WriteNode(new XmlTextReader(new StringReader(sb.ToString())), false);
			writer.WriteEndDocument();
			writer.Flush();
		}
	}
}
