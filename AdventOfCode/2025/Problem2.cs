
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.TwentyFive
{
    public class Problem2 : StringProblem
    {
        public override void Solve(IEnumerable<string> testData)
        {
            var ranges = testData.Single().Split(',').Select(x => new Range(x));
            var maxDigitsLength = ranges.Select(x => x.End).Max().ToString().Length;
            var allInvalids = new List<long>();
            var allInvalids2 = new List<long>();
            for (var i = 1; i < Math.Pow(10, maxDigitsLength / 2); i++)
            {
                var timesItCanRepeat = maxDigitsLength / i.ToString().Length;
                allInvalids.Add(long.Parse(i.ToString() + i));
                for (var times = 2; times <= timesItCanRepeat; times++)
                {
                    allInvalids2.Add(long.Parse(string.Join("", Enumerable.Repeat(i, times))));
                }
            }

            allInvalids2 = allInvalids2.OrderBy(x => x).Distinct().ToList();

            PrintResult(ranges.Select(x => x.InvalidIdSum(allInvalids)).Sum());
            PrintResult(ranges.Select(x => x.InvalidIdSum(allInvalids2)).Sum());
        }

        private class Range
        {
            public long Start { get; set; }
            public long End { get; set; }
            public Range(string line)
            {
                var tokens = line.Split('-').Select(long.Parse).ToArray();
                Start = tokens[0];
                End = tokens[1];
            }

            public long InvalidIdSum(List<long> allInvalids)
            {
                return allInvalids.Where(x => x >= Start && x <= End).Sum();
            }
        }
    }

   
}
