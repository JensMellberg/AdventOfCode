using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.TwentyFive
{
    public class Problem5 : StringProblem
    {
        protected override EmptyStringBehavior EmptyStringBehavior => EmptyStringBehavior.Keep;
        public override void Solve(IEnumerable<string> testData)
        {
            var ranges = new List<(long start, long end)>();
            var testDataList = testData.ToList();
            var current = 0;
            while (testDataList[current] != "")
            {
                var tokens = testDataList[current].Split('-').Select(long.Parse).ToArray();
                ranges.Add((tokens[0], tokens[1]));
                current++;
            }

            var ids = testDataList.SkipLast(1).TakeLast(testDataList.Count - current - 2).Select(long.Parse);

            var freshCount = ids.Count(x => ranges.Any(r => RangeContains(r, x)));
            PrintResult(freshCount);

            for (var i = 0; i < ranges.Count; i++)
            { 
                for (var x = i + 1; x < ranges.Count; x++)
                {
                    var range = ranges[i];
                    var range2 = ranges[x];
                    var combined = TryCombine(range, range2) ?? TryCombine(range2, range);
                    if (combined != null)
                    {
                        ranges.RemoveAt(x);
                        ranges[i] = combined.Value;
                        x = i;
                    }
                }
            }

            PrintResult(ranges.Sum(CountRange));
        }

        private long CountRange((long start, long end) range) => range.end - range.start + 1;

        private (long start, long end)? TryCombine((long start, long end) range1, (long start, long end) range2)
        {
            if (range1.start <= range2.start && range1.end >= range2.start)
            {
                return (range1.start, Math.Max(range1.end, range2.end));
            }

            return null;
        }

        private bool RangeContains((long start, long end) range, long value) => value >= range.start && value <= range.end;
    }
}
