using System;
using System.Xml;
using System.Xml.XPath;
using System.Collections;

namespace NMatrix.Schematron
{
	/// <summary>
	/// Strategy class for matching and keeping references to nodes in an <see cref="XPathDocument"/>.
	/// </summary>
	/// <authorref id="dcazzulino" />
	/// <progress amount="100" />
	/// <remarks>
	/// When an <see cref="XPathNavigator"/> is created from an <see cref="XPathDocument"/>,
	/// it implements the <see cref="IXmlLineInfo"/> interface, which is used to gain
	/// access to the underlying node position.
	/// </remarks>
	class XPathMatchedNodes : IMatchedNodes
	{
		/// <summary>
		/// The table contains an item for each line, and the item value
		/// is an instance of our <see cref="Int32Collection"/> class for
		/// optimized value types storage.
		/// </summary>
		Hashtable _matched = new Hashtable();

		/// <summary>Initializes an instance of the class.</summary>
		public XPathMatchedNodes()
		{
		}

		/// <summary>See <see cref="IMatchedNodes.IsMatched"/>.</summary>
		public bool IsMatched(System.Xml.XPath.XPathNavigator node)
		{
			IXmlLineInfo info = (IXmlLineInfo)node;

			if (!_matched.ContainsKey(info.LineNumber))
				return false;

			Int32Collection pos = (Int32Collection) _matched[info.LineNumber];
			
			if (!pos.Contains(info.LinePosition))
				return false;

			return true;
		}

		/// <summary>See <see cref="IMatchedNodes.AddMatched"/>.</summary>
		public void AddMatched(System.Xml.XPath.XPathNavigator node)
		{
			IXmlLineInfo info = (IXmlLineInfo)node;
			Int32Collection pos;

			if (!_matched.ContainsKey(info.LineNumber))
			{
				pos = new Int32Collection();
				_matched.Add(info.LineNumber, pos);
			}
			else
			{
				pos = (Int32Collection)_matched[info.LineNumber];
			}

			pos.Add(info.LinePosition);
		}

		/// <summary>See <see cref="IMatchedNodes.Clear"/>.</summary>
		public void Clear()
		{
			_matched.Clear();
		}
	}
}
