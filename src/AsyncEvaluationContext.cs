using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Collections;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using NMatrix.Schematron.Formatters;

namespace NMatrix.Schematron
{
	/// <summary>
	/// Evaluates <see cref="Assert"/> and <see cref="Report"/> elements asynchronously. 
	/// </summary>
	/// <remarks>
	/// See <see cref="EvaluationContextBase"/> for a description of the purpose and 
	/// of evaluation contexts, and where are they used.
	/// </remarks>
	/// <author ref="dcazzulino">
	/// Currently this class is not working properly. We're getting thread contention
	/// when the <see cref="WaitHandle.WaitAll"/> method is called. We have tried 
	/// almost every options there is to retrieve the handles. The one with the contention
	/// problem is the one using <see cref="ThreadPool.RegisterWaitForSingleObject"/> method
	/// to enqueue the method execution.
	/// <para>
	/// Another approach tried (and commented in the code) is asynchronous execution of the
	/// evaluation method using <see cref="AsyncAssertEvaluate"/> delegate. This produces
	/// and exception because apparently an invalid <see cref="WaitHandle"/> is returned.
	/// </para>
	/// So currently, only the <see cref="SyncEvaluationContext"/> is in use.
	/// </author>
	/// <progress amount="70" />
	internal class AsyncEvaluationContext : EvaluationContextBase
	{
		/// <summary>Creates the evaluation context</summary>
		public AsyncEvaluationContext()
		{
		}

		/// <summary>
		/// Starts the evaluation process.
		/// </summary>
		/// <remarks>
		/// When the process is finished, the results are placed 
		/// in the <see cref="EvaluationContextBase.Messages"/> property.
		/// </remarks>
		public override void Start()
		{
			Messages = String.Empty;
            
			// If no phase was received, try the default phase defined for the schema.
			// If no default phase is defined, all patterns will be tested (see the
			// Schema object comments.
			if (Phase == String.Empty)
			{
				if (Schema.DefaultPhase == String.Empty)
				{
					Phase = Schematron.Phase.All;
				}
				else
				{
					Phase = Schema.DefaultPhase;
				}
			}

			string res = Evaluate(Schema.Phases[Phase]);
			Messages = Formatter.Format(Schema, res, Source);
		}

		/// <summary>
		/// Evaluates the selected <see cref="Phase"/>.
		/// </summary>
		/// <remarks>
		/// Processing is synchronous, as patterns must 
		/// me evaluated in document order.
		/// <para>
		///	As most of the other evaluation methods, it repositions the 
		///	<see cref="EvaluationContextBase.Source"/> navigator on the root node.
		/// </para>
		/// </remarks>
		/// <param name="phase">The <see cref="Phase"/> to evaluate.</param>
		/// <returns>The messages accumulated by the evaluation of all the child
		/// <see cref="Pattern"/>, or <see cref="String.Empty"/> if there are no messages.</returns>
		private string Evaluate(Phase phase)
		{
			Source.MoveToRoot();
			StringBuilder sb = new StringBuilder();

			foreach (Pattern pt in phase.Patterns)
			{
				sb.Append(Evaluate(pt));
			}

			string res = sb.ToString();
			return Formatter.Format(phase, res, Source);
		}

		/// <summary>
		/// Evaluates the selected <see cref="Pattern"/>.
		/// </summary>
		/// <remarks>
		/// Processing is synchronous, as rules must me evaluated in document order.
		/// <para>
		///	As most of the other evaluation methods, it repositions the 
		///	<see cref="EvaluationContextBase.Source"/> navigator on the root node.
		/// </para>
		/// <para>
		/// Clears the <see cref="EvaluationContextBase.Matched"/> object before
		/// proceeding, as the restriction about node mathing (see <link ref="schematron" />)
		/// applies only inside a single pattern.
		/// </para>
		/// </remarks>
		/// <param name="pattern">The <see cref="Pattern"/> to evaluate.</param>
		/// <returns>The messages accumulated by the evaluation of all the child
		/// <see cref="Rule"/>, or <see cref="String.Empty"/> if there are no messages.</returns>
		private string Evaluate(Pattern pattern)
		{
			Source.MoveToRoot();
			StringBuilder sb = new StringBuilder();

			// Reset matched nodes, as across patters, nodes can be 
			// evaluated more than once.
			Matched.Clear();

			foreach (Rule rule in pattern.Rules)
			{
				sb.Append(Evaluate(rule));
			}

			string res = sb.ToString();
			return Formatter.Format(pattern, res, Source);
		}

		/// <summary>
		/// Evaluates the selected <see cref="Rule"/>.
		/// </summary>
		/// <remarks>
		/// Here is where asynchronous becomes. <see cref="Assert"/> and
		/// <see cref="Report"/> are queued using the <see cref="ThreadPool"/> class.
		/// <para>
		/// Nodes matched by this <see cref="Rule"/> are added to the <see cref="EvaluationContextBase.Matched"/> list of 
		/// nodes to skip in the next rule, using the <see cref="IMatchedNodes.AddMatched"/> method.
		/// This object is a strategy object which implements different algorithms for matching and 
		/// saving node references, as the actual <see cref="XPathNavigator"/> implementation provides
		/// different methods for accessing the underlying source. 
		/// <para>
		/// This makes the implementation both performant and compliant with
		/// the restriction about node mathing (see <link ref="schematron" />) in the spec.
		/// </para>
		/// <para>
		///		<seealso cref="DomMatchedNodes"/>
		///		<seealso cref="XPathMatchedNodes"/>
		///		<seealso cref="GenericMatchedNodes"/>
		/// </para>
		///	As most of the other evaluation methods, it repositions the 
		///	<see cref="EvaluationContextBase.Source"/> navigator on the root node.
		/// </para>
		/// <para>Here is where the multithreading problems arise, which are not
		/// due to the schema design itself, but this specific evaluation process.
		/// The intent it to evaluate asserts and reports in parallel, to get the 
		/// most out of the CPU.
		/// </para>
		/// </remarks>
		/// <param name="rule">The <see cref="Rule"/> to evaluate.</param>
		/// <returns>The messages accumulated by the evaluation of all the child
		/// <see cref="Assert"/> and <see cref="Report"/>.</returns>
		/// <exception cref="InvalidOperationException">
		/// The rule to evaluate is abstract (see <see cref="Rule.IsAbstract"/>).
		/// </exception>
		private string Evaluate(Rule rule)
		{
			if (rule.IsAbstract)
				throw new InvalidOperationException("The Rule is abstract, so it can't be evaluated.");

			StringBuilder sb = new StringBuilder();
			Source.MoveToRoot();
			XPathNodeIterator nodes = Source.Select(rule.CompiledExpression);
			ArrayList evaluables = new ArrayList(nodes.Count);

			while (nodes.MoveNext())
			{
				if (!Matched.IsMatched(nodes.Current))
				{
					// Add the navigator to the list to be evaluated and to 
					// the list of pattern-level nodes matched so far.
					XPathNavigator curr = nodes.Current.Clone();
					evaluables.Add(curr);
					Matched.AddMatched(curr);
				}
			}

			ArrayList handles = new ArrayList(rule.Asserts.Count + rule.Reports.Count);
			
			foreach (Assert asr in rule.Asserts)
			{
				//AsyncAssertEvaluate eval = new AsyncAssertEvaluate(EvaluateAssert);
				foreach (XPathNavigator node in evaluables)
				{
					XPathNavigator ctx = node.Clone();
					
					WaitHandle wh = new AutoResetEvent(true);
					WaitOrTimerCallback cb = new WaitOrTimerCallback(OnAssertEvaluate);

					ThreadPool.RegisterWaitForSingleObject(wh, cb, 
						new AsyncAssertState(asr, sb, ctx), Timeout.Infinite, true);
                    
					handles.Add(wh);
					//handles.Add(eval.BeginInvoke(
					//	asr, ctx, new AsyncCallback(OnAssertCompleted), 
					//	new AsyncAssertState(asr, sb, ctx)).AsyncWaitHandle);
					
					//Synchronous evaluation
					//string str = EvaluateAssert(asr, node.Clone());
					//if (str != String.Empty)
					//	sb.Append(str).Append(System.Environment.NewLine);
				}
			}

			foreach (Report rpt in rule.Reports)
			{
				//AsyncReportEvaluate eval = new AsyncReportEvaluate(EvaluateReport);
				foreach (XPathNavigator node in evaluables)
				{
					XPathNavigator ctx = node.Clone();

					WaitHandle wh = new AutoResetEvent(true);
					WaitOrTimerCallback cb = new WaitOrTimerCallback(OnReportEvaluate);

					ThreadPool.RegisterWaitForSingleObject(wh, cb, 
						new AsyncReportState(rpt, sb, ctx), Timeout.Infinite, true);
                    
					handles.Add(wh);
					//handles.Add(eval.BeginInvoke(
					//	rpt, ctx, new AsyncCallback(OnReportCompleted), 
					//	new AsyncReportState(rpt, sb, ctx)).AsyncWaitHandle);

					//Synchronous evaluation
					//string str = EvaluateReport(rpt, node.Clone());
					//if (str != String.Empty)
					//	sb.Append(str).Append(System.Environment.NewLine);
				}
			}

			try
			{
				//TODO: I think we are getting contention here. Anyone can help?
				WaitHandle[] waithandles = new WaitHandle[handles.Count];
				handles.CopyTo(waithandles);
				WaitHandle.WaitAll(waithandles);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Fail(ex.ToString());
			}

			string res = sb.ToString();
			return Formatter.Format(rule, res, Source);
		}

		/// <summary>
		/// Asynchronous evaluation of an <see cref="Assert"/>
		/// </summary>
		/// <remarks>
		/// This method is used as the <see cref="WaitOrTimerCallback"/> delegate for the 
		/// <see cref="ThreadPool.RegisterWaitForSingleObject"/> version of the asynchonous call.
		/// <para>
		/// This just retrieves the state, casts it to <see cref="AsyncAssertState"/> and executes the 
		/// <see cref="EvaluateAssert"/> method, passing the values received. It locks on the received
		/// <see cref="StringBuilder"/> to append the messages from evaluation.
		/// </para>
		/// </remarks>
		/// <param name="state">State for the execution.</param>
		/// <param name="timedOut">If the <see cref="WaitHandle"/> timed out.</param>
		private void OnAssertEvaluate(object state, bool timedOut)
		{
			AsyncAssertState st = (AsyncAssertState) state;
			System.Diagnostics.Debug.WriteLine("Executing assert on thread: " +
				Thread.CurrentThread.GetHashCode());
			
			lock(st.Builder)
			{
				st.Builder.Append(EvaluateAssert(st.Assert, st.Context));
			}
		}

		/// <summary>
		/// Asynchronous evaluation of a <see cref="Report"/>
		/// </summary>
		/// <remarks>
		/// This method is used as the <see cref="WaitOrTimerCallback"/> delegate for the 
		/// <see cref="ThreadPool.RegisterWaitForSingleObject"/> version of the asynchonous call.
		/// <para>
		/// This just retrieves the state, casts it to <see cref="AsyncReportState"/> and executes the 
		/// <see cref="EvaluateReport"/> method, passing the values received. It locks on the received
		/// <see cref="StringBuilder"/> to append the messages from evaluation.
		/// </para>
		/// </remarks>
		/// <param name="state">State for the execution.</param>
		/// <param name="timedOut">If the <see cref="WaitHandle"/> timed out.</param>
		private void OnReportEvaluate(object state, bool timedOut)
		{
			AsyncReportState st = (AsyncReportState) state;
			System.Diagnostics.Debug.WriteLine("Executing report on thread: " +
				Thread.CurrentThread.GetHashCode());
			
			lock(st.Builder)
			{
				st.Builder.Append(EvaluateReport(st.Report, st.Context));
			}
		}

		/// <summary>
		/// Performs the evaluation of the <see cref="Assert"/>.
		/// </summary>
		/// <remarks>
		/// This is where the actual assert expression is evaluated. 
		/// Is called synchonously from the <see cref="OnAssertEvaluate"/> or 
		/// using the <see cref="AsyncAssertEvaluate"/> delegate asynchronously, with 
		/// BeginInvoke and EndInvoke. If the <see cref="EvaluableExpression.Expression"/> 
		/// returns false, a formated message is generated 
		/// from the <see cref="Test.Message"/> property.
		/// </remarks>
		/// <param name="assert">The <see cref="Assert"/> to evaluate.</param>
		/// <param name="context">The context node for the execution.</param>
		/// <returns>The formatted message for a failing <see cref="Assert"/>, or 
		/// <see cref="String.Empty"/>.
		/// </returns>
		private string EvaluateAssert(Assert assert, XPathNavigator context)
		{
			object eval = context.Evaluate(assert.CompiledExpression);
			bool result = true;

			if (assert.ReturnType == XPathResultType.Boolean)
				result = (bool)eval;
			else if (assert.ReturnType == XPathResultType.NodeSet &&
				((XPathNodeIterator)eval).Count == 0)
				result = false;
			
			if (!result)
				return Formatter.Format(assert, context);

			return String.Empty;
		}

		/// <summary>
		/// Performs the evaluation of the <see cref="Report"/>.
		/// </summary>
		/// <remarks>
		/// This is where the actual report expression is evaluated. 
		/// Is called synchonously from the <see cref="OnReportEvaluate"/> or 
		/// using the <see cref="AsyncReportEvaluate"/> delegate asynchronously, with 
		/// BeginInvoke and EndInvoke. If
		/// the <see cref="EvaluableExpression.Expression"/> returns true, 
		/// a formated message is generated from the <see cref="Test.Message"/> property.
		/// </remarks>
		/// <param name="report">The <see cref="Report"/> to evaluate.</param>
		/// <param name="context">The context node for the execution.</param>
		/// <returns>The formatted message for a succesful <see cref="Report"/>, or 
		/// <see cref="String.Empty"/>.
		/// </returns>
		private string EvaluateReport(Report report, XPathNavigator context)
		{
			object eval = context.Evaluate(report.CompiledExpression);
			bool result = false;

			if (report.ReturnType == XPathResultType.Boolean)
				result = (bool)eval;
			else if (report.ReturnType == XPathResultType.NodeSet &&
				((XPathNodeIterator)eval).Count != 0)
				result = true;

			if (result) 
				return Formatter.Format(report, context);

			return String.Empty;
		}

		/// <summary>
		/// Callback for asynchonous delegate execution.
		/// </summary>
		/// <remarks>
		/// This is the  <see cref="AsyncCallback"/> to use when executing
		/// asynchronously the <see cref="EvaluateAssert"/> method.
		/// It completes the call by calling EndInvoke, retrieving the results
		/// and appending the messages to the <see cref="StringBuilder"/> received
		/// in the <see cref="IAsyncResult.AsyncState"/> property.
		/// </remarks>
		/// <example>
		/// This in an example of the delegate creation and asynchronous execution.
		/// <code>AsyncAssertEvaluate eval = new AsyncAssertEvaluate(EvaluateAssert);
		/// eval.BeginInvoke(asr, ctx, 
		///		new AsyncCallback(OnAssertCompleted), sb);
		/// </code>
		/// </example>
		/// <param name="result">The object to extract state information from.</param>
		private void OnAssertCompleted(IAsyncResult result)
		{
			try
			{
				AsyncResult ar = (AsyncResult) result;
				AsyncAssertState st = (AsyncAssertState) ar.AsyncState;
				AsyncAssertEvaluate eval = (AsyncAssertEvaluate) ar.AsyncDelegate;
				string res =  eval.EndInvoke(ar);
				if (res != String.Empty)
				{
					lock(st.Builder)
					{
						st.Builder.Append(res).Append(System.Environment.NewLine);
						System.Diagnostics.Debug.WriteLine(res);
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Fail(ex.ToString());
				throw ex;
			}
		}

		/// <summary>
		/// Callback for asynchonous delegate execution.
		/// </summary>
		/// <remarks>
		/// This is the  <see cref="AsyncCallback"/> to use when executing
		/// asynchronously the <see cref="EvaluateReport"/> method.
		/// It completes the call by calling EndInvoke, retrieving the results
		/// and appending the messages to the <see cref="StringBuilder"/> received
		/// in the <see cref="IAsyncResult.AsyncState"/> property.
		/// </remarks>
		/// <example>
		/// This in an example of the delegate creation and asynchronous execution.
		/// <code>AsyncReportEvaluate eval = new AsyncReportEvaluate(EvaluateReport);
		/// eval.BeginInvoke(rpt, ctx, new AsyncCallback(OnReportCompleted), sb);
		/// </code>
		/// </example>
		/// <param name="result">The object to extract state information from.</param>
		private void OnReportCompleted(IAsyncResult result)
		{
			try
			{
				AsyncResult ar = (AsyncResult) result;
				AsyncReportState st = (AsyncReportState) ar.AsyncState;
				AsyncReportEvaluate eval = (AsyncReportEvaluate) ar.AsyncDelegate;
				string res =  eval.EndInvoke(ar);
				if (res != String.Empty)
				{
					lock(st.Builder)
					{
						st.Builder.Append(res).Append(System.Environment.NewLine);
						System.Diagnostics.Debug.WriteLine(res);
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Fail(ex.ToString());
				throw ex;
			}
		}

		/// <summary>
		/// Structure to pass state around for asynchonous execution.
		/// </summary>
		struct AsyncAssertState
		{
			public Assert Assert;
			public StringBuilder Builder;
			public XPathNavigator Context;
			
			public AsyncAssertState(Assert assert, StringBuilder builder, XPathNavigator context)
			{
				Assert = assert;
				Builder = builder;
				Context = context;
			}
		}

		/// <summary>
		/// Structure to pass state around for asynchonous execution.
		/// </summary>
		struct AsyncReportState
		{
			public Report Report;
			public StringBuilder Builder;
			public XPathNavigator Context;

			public AsyncReportState(Report report, StringBuilder builder, XPathNavigator context)
			{
				Report = report;
				Builder = builder;
				Context = context;
			}
		}
	}
}
