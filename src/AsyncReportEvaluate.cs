using System;
using System.Xml.XPath;

namespace NMatrix.Schematron
{
	/// <summary>
	/// Private delegate used in <see cref="AsyncEvaluationContext"/> to call
	/// evaluation asynchronously against an <see cref="Report"/> object.
	/// </summary>
	/// <example>
	/// This in an example of the delegate creation and asynchronous execution.
	/// <code>AsyncReportEvaluate eval = new AsyncReportEvaluate(EvaluateReport);
	/// eval.BeginInvoke(
	///		report, 
	///		context, 
	///		new AsyncCallback(OnReportCompleted), 
	///		state);
	/// </code>
	/// </example>
	/// <author ref="dcazzulino" />
	/// <progress amount="100" />
	internal delegate string AsyncReportEvaluate(Report report, XPathNavigator context);
}
