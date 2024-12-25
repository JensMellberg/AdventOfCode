using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
	public abstract class Parsable
	{
		public string originalLine;
		public Parsable() { }

		public virtual void ParseFromLine(string line)
		{
			originalLine = line;
		}

		public Exception Exception => new Exception($"Error at object parsed from line {{{this.originalLine}}}");
	}
}
