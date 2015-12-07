using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using System.Text;

namespace NMatrix.Schematron.Formatters
{
	/// <summary>
	/// Look at <see cref="IFormatter"/> documentation.
	/// </summary>
	public abstract class FormatterBase : IFormatter
	{
		/// <summary />
		public FormatterBase()
		{
		}

        /// <summary>
        /// Look at <see cref="IFormatter.Format(Test, XPathNavigator, StringBuilder)"/> documentation.
        /// </summary>
        public virtual void Format(Test source, XPathNavigator context, StringBuilder output)
        {
        }

        /// <summary>
        /// Look at <see cref="IFormatter.Format(Rule, XPathNavigator, StringBuilder)"/> documentation.
        /// </summary>
        public virtual void Format(Rule source, XPathNavigator context, StringBuilder output)
        {
        }

        /// <summary>
        /// Look at <see cref="IFormatter.Format(Pattern, XPathNavigator, StringBuilder)"/> documentation.
        /// </summary>
        public virtual void Format(Pattern source, XPathNavigator context, StringBuilder output)
        {
        }

        /// <summary>
        /// Look at <see cref="IFormatter.Format(Phase, XPathNavigator, StringBuilder)"/> documentation.
        /// </summary>
        public virtual void Format(Phase source, XPathNavigator context, StringBuilder output)
        {
        }

        /// <summary>
        /// Look at <see cref="IFormatter.Format(Schema, XPathNavigator, StringBuilder)"/> documentation.
        /// </summary>
        public virtual void Format(Schema source, XPathNavigator context, StringBuilder output)
        {
        }
			
        /// <summary>
        /// Look at <see cref="IFormatter.Format(XmlReader, StringBuilder)"/> documentation.
        /// </summary>
        public virtual void Format(XmlReader reader, StringBuilder output)
        {
        }
			
        /// <summary>
        /// Look at <see cref="IFormatter.Format(ValidationEventArgs, StringBuilder)"/> documentation.
        /// </summary>
        public virtual void Format(ValidationEventArgs source, StringBuilder output)
        {
        }

		/// <summary>
        /// Look at <see cref="IFormatter.Format(XmlSchemaSet, StringBuilder)"/> documentation.
		/// </summary>
		public virtual void Format(XmlSchemaSet schemas, StringBuilder output)
		{
		}

		/// <summary>
        /// Look at <see cref="IFormatter.Format(SchemaCollection, StringBuilder)"/> documentation.
		/// </summary>
		public virtual void Format(SchemaCollection schemas, StringBuilder output)
		{
		}
 
		/// <summary>
        /// Look at <see cref="IFormatter.Format(StringBuilder)"/> documentation.
        /// </summary>
        public virtual void Format(StringBuilder output)
        {
        }
    }
}