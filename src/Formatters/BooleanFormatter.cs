using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;

namespace NMatrix.Schematron.Formatters
{
	/// <summary>
	/// Provides a simple failure message, without any details of specific validation errors.
	/// </summary>
	public class BooleanFormatter : FormatterBase
	{
		/// <summary />
		public BooleanFormatter()
		{
		}

        /// <summary>
        /// Look at <see cref="IFormatter.Format"/> documentation.
        /// </summary>
		public override void Format(Schema source, XPathNavigator context, StringBuilder output)
		{
            output.Append("Validation failed!");
		}
	}
}
