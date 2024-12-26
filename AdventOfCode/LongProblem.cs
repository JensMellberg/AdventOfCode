using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
	public abstract class LongProblem : Problem<long>
	{
		protected override long ParseDataLine(string line)
		{
			return long.Parse(line);
		}
	}
}
