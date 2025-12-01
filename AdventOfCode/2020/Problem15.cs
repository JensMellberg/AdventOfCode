using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Twenty
{
    public class Problem15 : ObjectProblem<ListParsable<int>>
    {
        public override void Solve(IEnumerable<ListParsable<int>> testData)
        {
            var numbers = testData.First().Values;
            var turn = numbers.Count;
            var history = new Dictionary<int, int>();
            var lastNumber = numbers.Last();
            numbers.Take(numbers.Count - 1).Select((x, i) => (x, i)).ForEach(p => history.Add(p.x, p.i));
            while (turn < 30000000)
            {
                if (turn == 2020)
                {
                    this.PrintResult(lastNumber);
                }

                var previousLastNumber = lastNumber;
                if (!history.ContainsKey(lastNumber))
                {
                    lastNumber = 0;
                }
                else
                {
                    lastNumber = turn - 1 - history[lastNumber];
                }

                history[previousLastNumber] = turn - 1;
                turn++;
            }

            this.PrintResult(lastNumber);
        }
    }
}
