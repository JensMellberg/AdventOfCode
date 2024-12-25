using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace AdventOfCode.TwentyFour
{
    public class Problem22 : IntegerProblem
    {
        public override void Solve(IEnumerable<int> testData)
        {
            this.PrintResult(testData.Sum(x => GetXthNumber(x, 2000)));
            var sequenceGains = new Dictionary<string, int>();
            foreach (var nbr in testData)
            {
                var usedSequences = new HashSet<string>();
                var prices = GetPrices(nbr);
                var currentSequence = new List<int>();
                var isFirst = true;
                foreach (var p in prices)
                {
                    if (currentSequence.Count == 4)
                    {
                        currentSequence.RemoveAt(0);
                    }

                    if (!isFirst)
                    {
                        currentSequence.Add(p.diff);
                    }

                    isFirst = false;
                    var seqString = string.Join("", currentSequence);
                    if (currentSequence.Count == 4 && !usedSequences.Contains(seqString))
                    {
                        if (sequenceGains.ContainsKey(seqString))
                        {
                            sequenceGains[seqString] += p.price;
                        }
                        else
                        {
                            sequenceGains.Add(seqString, p.price);
                        }

                        usedSequences.Add(seqString);
                    }
                }
            }

            this.PrintResult(sequenceGains.Values.Max());
        }

        private static long GetXthNumber(long number, int times)
        {
            for (var i = 0; i < times; i++)
            {
                number = GetNextNumber(number);
            }

            return number;
        }

        private static IEnumerable<(int price, int diff)> GetPrices(long number)
        {
            var prevNumber = (int)(number % 10);
            yield return (prevNumber, 0);
            for (var i = 0; i < 2000; i++)
            {
                number = GetNextNumber(number);
                var newNumber = (int)(number % 10);
                yield return (newNumber, newNumber - prevNumber);
                prevNumber = newNumber;
            }
        }
        
        private static long GetNextNumber(long number)
        {
            var mult = number * 64;
            number = MixNumber(number, mult);
            number = PruneNumber(number);

            var div = number / 32;
            number = MixNumber(number, div);
            number = PruneNumber(number);

            var mult2 = number * 2048;
            number = MixNumber(number, mult2);
            number = PruneNumber(number);

            return number;
        }

        private static long MixNumber(long number, long mix) => number ^ mix;

        private static long PruneNumber(long number) => number % 16777216;
    }
}
