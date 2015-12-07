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
        /// Look at <see cref="IFormatter.Format"/> documentation.
        /// </summary>
        public virtual void Format(Test source, XPathNavigator context, StringBuilder output)
        {
        }

        /// <summary>
        /// Look at <see cref="IFormatter.Format"/> documentation.
        /// </summary>
        public virtual void Format(Rule source, XPathNavigator context, StringBuilder output)
        {
        }

        /// <summary>
        /// Look at <see cref="IFormatter.Format"/> documentation.
        /// </summary>
        public virtual void Format(Pattern source, XPathNavigator context, StringBuilder output)
        {
        }

        /// <summary>
        /// Look at <see cref="IFormatter.Format"/> documentation.
        /// </summary>
        public virtual void Format(Phase source, XPathNavigator context, StringBuilder output)
        {
        }

        /// <summary>
        /// Look at <see cref="IFormatter.Format"/> documentation.
        /// </summary>
        public virtual void Format(Schema source, XPathNavigator context, StringBuilder output)
        {
        }
			
        /// <summary>
        /// Look at <see cref="IFormatter.Format"/> documentation.
        /// </summary>
        public virtual void Format(XmlValidatingReader reader, StringBuilder output)
        {
        }
			
        /// <summary>
        /// Look at <see cref="IFormatter.Format"/> documentation.
        /// </summary>
        public virtual void Format(ValidationEventArgs source, StringBuilder output)
        {
        }

		/// <summary>
		/// Look at <see cref="IFormatter.Format"/> documentation.
		/// </summary>
		public virtual void Format(XmlSchemaCollection schemas, StringBuilder output)
		{
		}

		/// <summary>
		/// Look at <see cref="IFormatter.Format"/> documentation.
		/// </summary>
		public virtual void Format(SchemaCollection schemas, StringBuilder output)
		{
		}
 
		/// <summary>
        /// Look at <see cref="IFormatter.Format"/> documentation.
        /// </summary>
        public virtual void Format(StringBuilder output)
        {
        }
    }
}