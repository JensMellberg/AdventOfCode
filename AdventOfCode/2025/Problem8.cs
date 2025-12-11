using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.TwentyFive
{
    public class Problem8 : PatternProblem<(int x, int y, int z)>
    {
        protected override string Pattern => "¤x¤,¤y¤,¤z¤";

        public override void Solve(IEnumerable<(int x, int y, int z)> testData)
        {
            var junctionBoxes = testData.Select(x => new JunctionBox(x)).ToList();
            var distances = new List<DistancePair>();
            for (var i = 0; i < junctionBoxes.Count; i++)
            {
                var p1 = junctionBoxes[i];
                for (var x = i + 1; x < junctionBoxes.Count; x++)
                {
                    var p2 = junctionBoxes[x];
                    var dist = DistanceBetween(p1.Point, p2.Point);
                    distances.Add(new DistancePair { P1 = p1, P2 = p2, Distance = dist });
                }
            }

            var boxesConnected = 0;
            var circuits = new List<Circuit>();
            var distancesAdded = 0;
            foreach (var pair in distances.OrderBy(x => x.Distance))
            {
                if (pair.P1.Circuit == null && pair.P2.Circuit == null)
                {
                    var boxes = new List<JunctionBox>
                    {
                        pair.P1,
                        pair.P2
                    };
                    var circuit = new Circuit { Boxes = boxes };
                    circuits.Add(circuit);
                    pair.P1.Circuit = circuit;
                    pair.P2.Circuit = circuit;
                    boxesConnected += 2;
                }
                else if (pair.P1.Circuit != null && pair.P2.Circuit == null)
                {
                    pair.P1.Circuit.Boxes.Add(pair.P2);
                    pair.P2.Circuit = pair.P1.Circuit;
                    boxesConnected++;
                }
                else if (pair.P2.Circuit != null && pair.P1.Circuit == null)
                {
                    pair.P2.Circuit.Boxes.Add(pair.P1);
                    pair.P1.Circuit = pair.P2.Circuit;
                    boxesConnected++;
                }
                else
                {
                    var tail = pair.P1.Circuit.GetTail;
                    var testTail = pair.P2.Circuit.GetTail;
                    if (tail != testTail)
                    {
                        var head = pair.P2.Circuit.GetHead;
                        tail.Tail = head;
                        head.Head = tail;
                    }
                }

                if (boxesConnected == junctionBoxes.Count)
                {
                    // Part 2 answer. Feels kinda lucky, i just check if all boxes has been connected to a circuit,
                    // not that they are all in the same circuit. But it works!
                    PrintResult((long)pair.P1.Point.x * pair.P2.Point.x);
                    boxesConnected++;
                }

                distancesAdded++;
                // Part 1 answer.
                if (distancesAdded == 1000)
                {
                    var filteredCircuits = new List<(Circuit c, int count)>();
                    foreach (var c in circuits.Where(x => x.IsHead))
                    {
                        filteredCircuits.Add((c, c.GetJunctionCount()));
                    }

                    var sum = filteredCircuits.OrderByDescending(x => x.count).Take(3).Aggregate(1, (v, n) => v * n.count);
                    PrintResult(sum);
                }
            }
        }

        private static double DistanceBetween((int x, int y, int z) p1, (int x, int y, int z) p2)
        {
            long dx = p1.x - p2.x;
            long dy = p1.y - p2.y;
            long dz = p1.z - p2.z;

            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        private class JunctionBox
        {
            public (int x, int y, int z) Point { get; set; }

            public Circuit Circuit { get; set; }

            public JunctionBox ((int x, int y, int z) point)
            {
                Point = point;
            }
        }

        private class Circuit
        {
            public List<JunctionBox> Boxes { get; set; } = new List<JunctionBox>();

            public Circuit Tail { get; set; }

            public Circuit Head { get; set; }

            public Circuit GetTail => Tail != null ? Tail.GetTail : this;

            public Circuit GetHead => Head != null ? Head.GetHead : this;

            public bool HasCounted { get; set; }

            public bool IsHead => Head == null;

            public int GetJunctionCount()
            {
                if (HasCounted)
                {
                    return -1;
                }

                HasCounted = true;
                var thisCount = Boxes.Count;
                return Tail != null ? thisCount + Tail.GetJunctionCount() : thisCount;
            }
        }

        private class DistancePair
        {
            public JunctionBox P1 { get; set; }

            public JunctionBox P2 { get; set; }

            public double Distance { get; set; }
        }
    }
}
