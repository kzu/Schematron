using System;
using NMatrix.Schematron;

namespace Test
{
	/// <summary />
	public class ExtSchema : Schema
	{
		public ExtSchema() : base()
		{
		}

		public override Phase CreatePhase(string id)
		{
			Console.WriteLine("Creating ext phase");
			return new ExtPhase(id);
		}
	}
}
