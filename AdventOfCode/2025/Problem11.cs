using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.TwentyFive
{
    public class Problem11 : PatternProblem<(string from, string to)>
    {
        protected override string Pattern => "¤from¤: ¤to¤";

        public override void Solve(IEnumerable<(string from, string to)> testData)
        {
            var deviceConnections = testData.ToDictionary(x => x.from, x => x.to.Split(' ').ToList());
            PrintResult(GetPathsTo(deviceConnections, "you", "out"));

            var dacToOut = GetPathsTo(deviceConnections, "dac", "out");
            var fftToDac = GetPathsTo(deviceConnections, "fft", "dac");
            var svrToFft = GetPathsTo(deviceConnections, "svr", "fft");
            PrintResult(svrToFft * fftToDac * dacToOut);
        }

        private long GetPathsTo(Dictionary<string, List<string>> deviceConnections, string start, string goal)
        {
            var allTargets = deviceConnections.Values.SelectMany(x => x).Distinct();
            var reverseConnections = allTargets.ToDictionary(x => x, v => deviceConnections.Where(x => x.Value.Contains(v)).Select(x => x.Key).ToList());
            var pathsToGoal = new Dictionary<string, long>();
            var queue = new Queue<string>();
            queue.Enqueue(goal);
            pathsToGoal[goal] = 1;

            while (queue.Count > 0)
            {
                var device = queue.Dequeue();
                var lengthToCurrent = pathsToGoal[device];

                if (!reverseConnections.ContainsKey(device))
                {
                    continue;
                }

                var devicesThatCanReach = reverseConnections[device];
                foreach (var d in devicesThatCanReach)
                {
                    if (!pathsToGoal.ContainsKey(d))
                    {
                        GetPathsToGoal(d);
                        queue.Enqueue(d);
                    }
                }
            }

            return pathsToGoal[start];

            long GetPathsToGoal(string device)
            {
                if (pathsToGoal.TryGetValue(device, out var paths))
                {
                    return paths;
                }

                if (!deviceConnections.ContainsKey(device))
                {
                    paths = 0;
                }
                else
                {
                    paths = deviceConnections[device].Sum(GetPathsToGoal);
                }

                pathsToGoal[device] = paths;
                return paths;
            }
        }
    }
}
