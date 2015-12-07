using System;
using System.Xml.XPath;

namespace NMatrix.Schematron
{
	/// <summary>
	/// Private delegate used in <see cref="AsyncEvaluationContext"/> to call
	/// evaluation asynchronously against an <see cref="Assert"/> object.
	/// </summary>
	/// <example>
	/// This in an example of the delegate creation and asynchronous execution.
	/// <code>AsyncAssertEvaluate eval = new AsyncAssertEvaluate(EvaluateAssert);
	/// eval.BeginInvoke(
	///		assert, 
	///		context, 
	///		new AsyncCallback(OnAssertCompleted), 
	///		state);
	/// </code>
	/// </example>
	/// <author ref="dcazzulino" />
	/// <progress amount="100" />
	internal delegate string AsyncAssertEvaluate(Assert assert, XPathNavigator context);
}
