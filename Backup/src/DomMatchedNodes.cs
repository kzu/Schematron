using System;
using System.Xml;
using System.Xml.XPath;

namespace NMatrix.Schematron
{
	/// <summary>
	/// Strategy class for matching and keeping references to nodes in an xml document.
	/// </summary>
	/// <remarks>
	/// When an <see cref="XPathNavigator"/> is created from an <see cref="XmlDocument"/>,
	/// it implements the <see cref="IHasXmlNode"/> interface, which is used to gain
	/// access to the underlying node.
	/// </remarks>
	/// <author ref="dcazzulino" />
	/// <progress amount="100" />
	class DomMatchedNodes : IMatchedNodes
	{
		/// <summary>
		/// We use an optimized collection for saving the hash codes.
		/// </summary>
		Int32Collection _matched = new Int32Collection();

		/// <summary>Initializes an instance of the class.</summary>
		public DomMatchedNodes()
		{
		}

		/// <summary>See <see cref="IMatchedNodes.IsMatched"/>.</summary>
		public bool IsMatched(XPathNavigator node)
		{
			return _matched.Contains(((IHasXmlNode)node).GetNode().GetHashCode());
		}

		/// <summary>See <see cref="IMatchedNodes.AddMatched"/>.</summary>
		public void AddMatched(XPathNavigator node)
		{
			_matched.Add(((IHasXmlNode)node).GetNode().GetHashCode());
		}

		/// <summary>See <see cref="IMatchedNodes.Clear"/>.</summary>
		public void Clear()
		{
			_matched.Clear();
		}
	}
}
