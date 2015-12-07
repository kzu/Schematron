using System;
using System.Text.RegularExpressions;

namespace NMatrix.Schematron
{
	/// <summary />
	internal class TagExpressions
	{
		/// <summary>
		/// The compiled regular expression to replace the special <c>name</c> tag inside a message.
		/// </summary>
		/// <remarks>
		/// Replaces each instance of <c>name</c> tags with the value un the current context element.
		/// </remarks>
		public static Regex Name;

		public static Regex Emph;
		public static Regex Dir;
		public static Regex Span;
		public static Regex Para;
		public static Regex Any;
		public static Regex AllSchematron;

		/// <summary />
		static TagExpressions()
		{
			// The element declarations can contain the namespace if expanded in a loaded document.
			Name = new Regex(@"<[^\s>]*\bname\b[^>]*/>", RegexOptions.Compiled); 
			Emph = new Regex(@"<[^\s>]*\bemph\b[^>]*>", RegexOptions.Compiled); 
			Dir = new Regex(@"<[^\s]*\bdir\b[^>]*>", RegexOptions.Compiled); 
			Span = new Regex(@"<[^\s]*\bspan\b[^>]*>", RegexOptions.Compiled); 
			Para = new Regex(@"<[^\s]*\bp\b[^>]*>", RegexOptions.Compiled); 
			Any = new Regex(@"<[^\s]*[^>]*>", RegexOptions.Compiled); 
			// Closing elements don't have an expanded xmlns so they will be matched too.
			// TODO: improve this to avoid removing non-schematron closing elements.
			AllSchematron = new Regex(@"<.*\bxmlns\b[^\s]*" + Schema.Namespace + "[^>]*>|</[^>]*>", RegexOptions.Compiled);
		}

		private TagExpressions()
		{
		}	
	}
}
