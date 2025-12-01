using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.TwentyFive
{
    public class Problem1 : StringProblem
    {
        public override void Solve(IEnumerable<string> testData)
        {
            var dial = new Dial();
            testData.ForEach(x => dial.Turn(x));
            PrintResult(dial.ExactNumberOfZeroPoints);
            PrintResult(dial.NumberOfZeroPoints);
        }

        private class Dial
        {
            private int current = 50;
            private int exactZeroPointCount;
            private int zeroPointCount;
            public void Turn(string instruction)
            {
                var multiplier = instruction[0] == 'L' ? -1 : 1;
                var steps = int.Parse(instruction[1..]);

                var stepsToGetToFirstZero = multiplier == -1 ? current : 100 - current;
                if (steps >= stepsToGetToFirstZero)
                {
                    if (stepsToGetToFirstZero != 0)
                    {
                        zeroPointCount++;
                    }

                    var stepsLeft = steps - stepsToGetToFirstZero;
                    zeroPointCount += (stepsLeft) / 100;
                }

                current = (current + (steps % 100) * multiplier + 100) % 100;

                if (current == 0)
                {
                    exactZeroPointCount++;
                }
            }

            public int ExactNumberOfZeroPoints => exactZeroPointCount;

            public int NumberOfZeroPoints => zeroPointCount;
        }
    }

   
}
