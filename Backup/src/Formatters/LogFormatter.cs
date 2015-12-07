using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;

namespace NMatrix.Schematron.Formatters
{
	/// <summary>
	/// Provides a complete log of validation errors in text format.
	/// </summary>
	public class LogFormatter : FormatterBase
	{
		/// <summary />
		public LogFormatter()
		{
		}

        /// <summary>
        /// Look at <see cref="IFormatter.Format"/> documentation.
        /// </summary>
        public override void Format(Test source, XPathNavigator context, StringBuilder output)
		{
			string msg = source.Message;
			StringBuilder sb = new StringBuilder();
			XPathExpression expr;

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
			sb = new StringBuilder();
			if (source is Assert)
			{
				sb.Append("\tAssert fails: ");
			}
			else
			{
				sb.Append("\tReport: ");
			}
			sb.Append(res);

			//Accumulate namespaces found during traversal of node for its position.
			Hashtable ns = new Hashtable();

            sb.Append("\r\n\tAt: ").Append(FormattingUtils.GetFullNodePosition(context.Clone(), String.Empty, source, ns));
			sb.Append(FormattingUtils.GetNodeSummary(context, ns, "\r\n\t    "));

			res = FormattingUtils.GetPositionInFile(context, "\r\n\t    ");
			if (res != String.Empty) sb.Append(res);

			res = FormattingUtils.GetNamespaceSummary(context, ns, "\r\n\t    ");
			if (res != string.Empty) sb.Append(res);
			sb.Append("\r\n");
			output.Append(sb.ToString());
		}

        /// <summary>
        /// Look at <see cref="IFormatter.Format"/> documentation.
        /// </summary>
        public override void Format(Pattern source, XPathNavigator context, StringBuilder output)
		{
			output.Insert(0, "    From pattern \"" + source.Name + "\"\r\n");
			output.Append("\r\n");
		}

        /// <summary>
        /// Look at <see cref="IFormatter.Format"/> documentation.
        /// </summary>
        public override void Format(Schema source, XPathNavigator context, StringBuilder output)
		{
			if (source.Title != String.Empty)
				output.Insert(0, source.Title + "\r\n");
            else
                output.Insert(0, "Results from Schematron validation\r\n");

			output.Append("\r\n");
		}

        /// <summary>
        /// Look at <see cref="IFormatter.Format"/> documentation.
        /// </summary>
        public override void Format(ValidationEventArgs source, StringBuilder output)
		{
			output.Append("  Error: ");
			output.Append(FormattingUtils.XmlErrorPosition.Replace(source.Message, String.Empty));
			output.Append("\r\n  At: (Line: ").Append(source.Exception.LineNumber);
			output.Append(", Column: ").Append(source.Exception.LinePosition).Append(")\r\n");
		}

		/// <summary>
		/// Look at <see cref="IFormatter.Format"/> documentation.
		/// </summary>
		public override void Format(XmlSchemaCollection schemas, StringBuilder output)
		{
			output.Insert(0, "Results from XML Schema validation:\r\n");
			output.Append("\r\n");
		}

		/// <summary>
		/// Look at <see cref="IFormatter.Format"/> documentation.
		/// </summary>
		public override void Format(SchemaCollection schemas, StringBuilder output)
		{
			output.Insert(0, "Results from Schematron validation:\r\n");
			output.Append("\r\n");
		}
	}
}
