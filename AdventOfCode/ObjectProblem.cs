using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
	public abstract class ObjectProblem<T> : Problem<T>
		where T : Parsable, new()
	{
		protected override T ParseDataLine(string line)
		{
			return ParsableUtils.CreateFromLine<T>(line);
		}
	}
}
