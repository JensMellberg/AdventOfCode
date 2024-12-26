using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Twenty
{
    public class Problem10 : IntegerProblem
    {
        public override void Solve(IEnumerable<int> testData)
        {
            var adapters = testData.OrderBy(x => x).ToList();
            adapters.Add(adapters.Last() + 3);
            var oneCounts = 0;
            var threeCounts = 0;
            var lastAdapter = 0;
            foreach (var adapter in adapters)
            {
                var diff = adapter - lastAdapter;
                if (diff == 1)
                {
                    oneCounts++;
                }
                else if (diff == 3)
                {
                    threeCounts++;
                }

                lastAdapter = adapter;
            }

            this.PrintResult(oneCounts * threeCounts);

            adapters.Insert(0, 0);
            this.PrintResult(GetPaths(adapters, 0, new Dictionary<int, long>()));
        }

        private long GetPaths(List<int> adapters, int index, Dictionary<int, long> pathsFromIndex)
        {
            if (pathsFromIndex.TryGetValue(index, out var paths))
            {
                return paths;
            }

            var current = adapters[index];
            var potentialTargets = new List<int>();
            var pointer = index + 1;
            while (pointer < adapters.Count && adapters[pointer] <= current + 3)
            {
                potentialTargets.Add(pointer);
                pointer++;
            }

            if (potentialTargets.Count == 0)
            {
                return 1;
            }

            paths = 0;
            foreach (var target in potentialTargets)
            {
                paths += GetPaths(adapters, target, pathsFromIndex);
            }

            pathsFromIndex[index] = paths;
            return paths;
        }
    }
}
