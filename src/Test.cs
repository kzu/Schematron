using System;
using System.Xml;
using System.Xml.XPath;
using System.Text.RegularExpressions;
using System.Collections;

namespace NMatrix.Schematron
{
	/// <summary>
	/// Base class for testing units of Schematron, such as Assert or Report elements.
	/// </summary>
    public abstract class Test : EvaluableExpression
	{
		/// <summary />
		protected string _msg;

		/// <summary />
        protected MatchCollection _names;

		/// <summary />
        protected XPathExpression[] _paths;

		/// <summary />
		/// <param name="test"></param>
		/// <param name="message"></param>
		public Test(string test, string message) : base(test)
		{
			if (ReturnType != XPathResultType.Boolean && 
				ReturnType != XPathResultType.NodeSet)
				throw new InvalidExpressionException("Test expression doesn't evaluate to a boolean or nodeset result: " + test);
			
			Message = Formatters.FormattingUtils.NormalizeString(message);
						
			// Save <name> tags in the message and their paths in their compiled form.
            // TODO: see if we can work with the XML in the message, instead of using RE.
			_names = TagExpressions.Name.Matches(Message);
			int nc = _names.Count;
			_paths = new XPathExpression[nc];

			for (int i = 0; i < nc; i++)
			{
				// Locate the path attribute and compile it with the DefaultNavigator.
				Match name = _names[i];
				int start = name.Value.IndexOf("path=");
				// Does it have a path attribute?
				if (start > 0)
				{
					// Skip the path=" string.
					start += 6;
					// If the namespace element is present, end the expression there.
					int end = name.Value.LastIndexOf("xmlns") - 2;
					if (end < 0)
						end = name.Value.LastIndexOf('"');
					string xpath = name.Value.Substring(start, end - start);
					_paths[i] = Config.DefaultNavigator.Compile(xpath);
				}
				else
					_paths[i] = null;
			}
		}

		/// <summary />
		public string Message
		{
			get { return _msg; }
			set { _msg = value; }
		}

		/// <summary />
		public MatchCollection NameExpressions
		{
			get { return _names; }
		}

		/// <summary />
		public XPathExpression[] NamePaths
		{
			get { return _paths; }
		}
	}
}
