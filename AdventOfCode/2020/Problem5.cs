using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Twenty
{
    public class Problem5 : StringProblem
    {
        public override void Solve(IEnumerable<string> testData)
        {
            var highestId = 0;
            var lowestId = int.MaxValue;
            var takenSeats = new HashSet<int>();
            foreach (var input in testData)
            {
                (int lower, int higher) range = (0, 127);
                var rowInfo = input.Substring(0, 7);
                foreach (var r in rowInfo)
                {
                    range = r == 'F' ? KeepLower(range) : KeepHigher(range);
                }

                var row = range.lower;
                range = (0, 7);
                foreach (var r in input.Substring(7))
                {
                    range = r == 'L' ? KeepLower(range) : KeepHigher(range);
                }

                var col = range.lower;
                var seatId = row * 8 + col;
                highestId = Math.Max(highestId, seatId);
                lowestId = Math.Min(lowestId, seatId);
                takenSeats.Add(seatId);
            }

            this.PrintResult(highestId);
            for (var f = lowestId; f < highestId; f++)
            {
                if (!takenSeats.Contains(f))
                {
                    this.PrintResult(f);
                    return;
                }
            }


            (int lower, int higher) KeepLower((int lower, int higher) range) => (range.lower, range.higher - (range.higher - range.lower) / 2 - 1);
            (int lower, int higher) KeepHigher((int lower, int higher) range) => (range.lower + ((range.higher - range.lower) / 2 + 1), range.higher);
        }
    }
}
