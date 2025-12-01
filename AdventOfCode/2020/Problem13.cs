using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Twenty
{
    public class Problem13 : StringProblem
    {
        public override void Solve(IEnumerable<string> testData)
        {
            var time = int.Parse(testData.First());
            var buses = testData.Skip(1).First().Split(',');
            var busesInService = buses.Where(x => x != "x").Select(int.Parse);
            var earliest = (0, int.MaxValue);
            foreach (var bus in busesInService)
            {
                var bussesPassed = time / bus;
                var nextBusTime = (bussesPassed + 1) * bus;
                var waitTime = nextBusTime - time;
                if (waitTime < earliest.Item2)
                {
                    earliest = (bus, waitTime);
                }
            }

            this.PrintResult(earliest.Item1 * earliest.Item2);

            var busList = buses.ToList();
            var busIndexes = busList.Select((x, i) => (x, i)).Where(b => b.x != "x").Select(b => (int.Parse(b.x), b.i)).OrderByDescending(b => b.Item1).ToList();
            var largest = busIndexes.FindMax(b => b.i);
            long timeStamp = -largest.i + 100000000000000;
            /*var mayAns = ParsableUtils.LowestCommonMultiple(busIndexes.Select(b => (long)b.Item1 + b.i));
            this.PrintResult(mayAns);
            return;*/
            while (true)
            {
                timeStamp += largest.Item1;
                if (busIndexes.All(b => (timeStamp + b.i) % b.Item1 == 0))
                {
                    this.PrintResult(timeStamp);
                    return;
                }
            }
        }
    }
}
