﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
	public abstract class IntegerProblem : Problem<int>
	{
		protected override int ParseDataLine(string line)
		{
			return int.Parse(line);
		}
	}
}
