using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace AdventOfCode.TwentyFour
{
    public class Problem23 : PatternProblem<(string comp1, string comp2)>
    {
        protected override string Pattern => "¤comp1¤-¤comp2¤";

        public override void Solve(IEnumerable<(string comp1, string comp2)> testInput)
        {
            var allComputers = new Dictionary<string, Computer>();
            foreach (var pair in testInput)
            {
                var comp1 = GetComputer(pair.comp1);
                var comp2 = GetComputer(pair.comp2);
                comp1.Connections.Add(comp2);
                comp2.Connections.Add(comp1);

                Computer GetComputer(string name)
                {
                    if (!allComputers.TryGetValue(name, out var comp))
                    {
                        comp = new Computer
                        {
                            Name = name
                        };

                        allComputers.Add(name, comp);
                    }

                    return comp;
                }
            }

            allComputers.Values.ForEach(x => x.ConnectedNames = x.Connections.Select(x => x.Name).ToHashSet());

            var circleCount = 0;
            var usedCircles = new HashSet<string>();
            foreach (var c in allComputers.Where(x => x.Key[0] == 't'))
            {
                var comp = c.Value;
                for (var c1 = 0; c1 < comp.Connections.Count; c1++)
                {
                    for (var c2 = c1 + 1; c2 < comp.Connections.Count; c2++)
                    {
                        var pair1 = comp.Connections[c1];
                        var pair2 = comp.Connections[c2];
                        var circleId = string.Join("", new[] { comp.Name, pair1.Name, pair2.Name }.OrderBy(x => x));
                        if (pair1.Connections.Contains(pair2) && usedCircles.Add(circleId))
                        {
                            circleCount++;
                        }
                    }
                }
            }

            this.PrintResult(circleCount);

            foreach (var c in allComputers.Values)
            {
                var consToCheck = c.Connections.ToList();
                var counts = c.Connections.ToDictionary(x => x.Name, x => 0);
                foreach (var con in consToCheck)
                {
                    con.ConnectedNames.Where(x => counts.ContainsKey(x)).ForEach(x => counts[x]++);
                }

                var bigConnectionGroup = counts.Where(x => x.Value > 10).Select(x => x.Key).ToList();
                if (bigConnectionGroup.Count > 11)
                {
                    bigConnectionGroup.Add(c.Name);
                    this.PrintResult(string.Join(",", bigConnectionGroup.OrderBy(x => x)));
                    break;
                }
            }
        }

        private class Computer
        {
            public string Name { get; set; }

            public List<Computer> Connections = new List<Computer>();

            public HashSet<string> ConnectedNames;
        }
    }
}
