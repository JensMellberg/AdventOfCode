using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Twenty
{
    public class Problem7 : StringProblem
    {
        public override void Solve(IEnumerable<string> testData)
        {
            var connections = new Dictionary<string, IList<(string target, int count)>>();
            var reverseConnections = new Dictionary<string, List<string>>();
            foreach (var line in testData)
            {
                var parser = new TokenParser(line);
                var bagPart = string.Join("", parser.PopUntil("bags"));
                parser.Pop();
                var localConnections = new List<(string target, int count)>();
                while (!parser.IsFinished)
                {
                    var first = parser.Pop();
                    if (ParsableUtils.IsNumber(first[0]))
                    {
                        var bagPartCon = string.Join("", new[] { parser.Pop(), parser.Pop() });
                        parser.Pop();
                        localConnections.Add((bagPartCon, first[0] - '0'));
                    }
                    else
                    {
                        break;
                    }
                }

                if (localConnections.Any())
                {
                    connections[bagPart] = localConnections;
                }

                foreach (var con in localConnections)
                {
                    if (!reverseConnections.TryGetValue(con.target, out var list))
                    {
                        list = new List<string>();
                        reverseConnections[con.target] = list;
                    }

                    list.Add(bagPart);
                }
            }

            var bagAlternatives = 0;
            var bagQueue = new Queue<string>();
            var addedBags = new HashSet<string>();
            bagQueue.Enqueue("shinygold");
            while (bagQueue.Any())
            {
                var bag = bagQueue.Dequeue();
                if (!reverseConnections.ContainsKey(bag))
                {
                    continue;
                }

                reverseConnections[bag].Where(x => !addedBags.Contains(x)).ForEach(x => 
                {
                    bagQueue.Enqueue(x);
                    addedBags.Add(x);
                    bagAlternatives++;
                });
            }

            this.PrintResult(bagAlternatives);

            var bagCount = 0;
            var newBagQueue = new Queue<(string bag, int count)>();
            newBagQueue.Enqueue(("shinygold", 1));
            while (newBagQueue.Any())
            {
                (var bag, var count) = newBagQueue.Dequeue();
                bagCount += count;

                if (!connections.ContainsKey(bag))
                {
                    continue;
                }

                connections[bag].ForEach(x =>
                {
                    newBagQueue.Enqueue((x.target, x.count * count));
                });
            }

            this.PrintResult(bagCount - 1);
        }
    }
}
