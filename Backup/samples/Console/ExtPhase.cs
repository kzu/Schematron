using System;
using NMatrix.Schematron;

namespace Test
{
	/// <summary />
	public class ExtPhase : Phase
	{
		/// <summary />
		internal protected ExtPhase(string id): base(id)
		{
		}

		/// <summary />
		internal protected ExtPhase(): base()
		{
		}

		public override Pattern CreatePattern(string name, string id)
		{
			Console.WriteLine("Creating ext pattern");
			return base.CreatePattern(name, id);
		}

		/// <summary />
		/// <param name="name"></param>
		public override Pattern CreatePattern(string name)
		{
			Console.WriteLine("Creating ext pattern");
			return base.CreatePattern(name);
		}
	}
}
