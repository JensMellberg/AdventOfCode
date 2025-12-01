using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AdventOfCode.Twenty
{
    public class Problem16 : StringProblem
    {
        protected override EmptyStringBehavior EmptyStringBehavior => EmptyStringBehavior.Keep;

        public override void Solve(IEnumerable<string> testData)
        {
            var ruleParser = new PatternParser("¤field¤: ¤range1¤ or ¤range2¤");
            var rules = new Dictionary<string, Range>();
            var testList = testData.ToList();
            var index = 0;
            while (testList[index] != string.Empty)
            {
                var rule = ruleParser.ParseObject<(string field, string range1, string range2)>(testList[index]);
                rules.Add(rule.field, new Range(rule.range1, rule.range2));
                index++;
            }

            index += 2;
            var myTicket = ParseTicket(testList[index]);
            index += 3;
            var otherTickets = new List<int[]>();
            var validTickets = new List<int[]>();
            while (index < testList.Count && testList[index] != string.Empty)
            {
                otherTickets.Add(ParseTicket(testList[index]));
                index++;
            }

            var errorRate = 0;
            foreach (var t in otherTickets)
            {
                var hasError = false;
                foreach (var value in t)
                {
                    if (!rules.Values.Any(x => x.IsInRange(value))) {
                        errorRate += value;
                        hasError = true;
                    }
                }

                if (!hasError)
                {
                    validTickets.Add(t);
                }
            }

            this.PrintResult(errorRate);

            var potentialFieldsPerIndex = new List<string>[myTicket.Length];
            var actualFieldIndexes = new string[myTicket.Length];
            for (index = 0; index < potentialFieldsPerIndex.Length; index++)
            {
                var alts = rules.Keys.ToList();
                foreach (var t in validTickets)
                {
                    for (var i = 0; i < alts.Count; i++)
                    {
                        if (!rules[alts[i]].IsInRange(t[index]))
                        {
                            alts.RemoveAt(i);
                            i--;
                        }
                    }
                }

                potentialFieldsPerIndex[index] = alts;
            }

            var entrysAdded = 0;
            while (entrysAdded < myTicket.Length)
            {
                foreach (var l in potentialFieldsPerIndex.Select((list, i) => (list, i)).Where(x => x.list.Count == 1))
                {
                    var entry = l.list.Single();
                    actualFieldIndexes[l.i] = l.list.Single();
                    potentialFieldsPerIndex.ForEach(x => x.Remove(entry));
                    entrysAdded++;
                    break;
                }
            }

            long result = 1;
            for (var i = 0; i < actualFieldIndexes.Length; i++)
            {
                if (actualFieldIndexes[i].StartsWith("departure"))
                {
                    result *= myTicket[i];
                }
            }

            this.PrintResult(result);
            int[] ParseTicket(string ticket) => ticket.Split(',').Select(int.Parse).ToArray();
        }

        private class Range
        {
            private (int from, int to) range1;
            private (int from, int to) range2;
            public Range(string range1, string range2)
            {
                this.range1 = ParseRange(range1);
                this.range2 = ParseRange(range2);
                (int, int) ParseRange(string range)
                {
                    var tokens = range.Split('-');
                    return (int.Parse(tokens[0]), int.Parse(tokens[1]));
                }
            }

            public bool IsInRange(int value) => value >= range1.from && value <= range1.to || value >= range2.from && value <= range2.to;
        }
    }
}
