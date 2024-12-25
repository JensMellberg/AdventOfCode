using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Twenty
{
    public class Problem2 : PatternProblem<(int min, int max, char letter, string password)>
    {
        protected override string Pattern => "¤min¤-¤max¤ ¤letter¤: ¤password¤";

        public override void Solve(IEnumerable<(int min, int max, char letter, string password)> testData)
        {
            var validCount = 0;
            var validCount2 = 0;
            foreach (var (min, max, letter, password) in testData)
            {
                var occurrences = password.Count(x => x == letter);
                if (occurrences >= min && occurrences <= max)
                {
                    validCount++;
                }

                var occurrences2 = 0;
                if (password[min - 1] == letter)
                {
                    occurrences2++;
                }

                if (password[max - 1] == letter)
                {
                    occurrences2++;
                }

                validCount2 += occurrences2 == 1 ? 1 : 0;
            }

            this.PrintResult(validCount);
            this.PrintResult(validCount2);
        }
    }
}
