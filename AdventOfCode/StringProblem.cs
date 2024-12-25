using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
	public abstract class StringProblem : Problem<string>
	{
		protected override string ParseDataLine(string line)
		{
			return line;
		}
	}
}
