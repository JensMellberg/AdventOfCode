﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
	public class ListParsable<T> : Parsable
	{
		public List<T> Values;

		protected virtual string Separator => ",";

		public override void ParseFromLine(string line)
		{
			this.Values = line.Split(this.Separator).Select(ConvertValue).OfType<T>().ToList();
			originalLine = line;
		}

		protected virtual object ConvertValue(string value)
		{
			if (typeof(T) == typeof(int))
			{
				return int.Parse(value);
			}
			else if (typeof(T) == typeof(long))
			{
				return long.Parse(value);
			}

			return value;
		}
	}
}
