using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.TwentyFive
{
    public class Problem3 : StringProblem
    {
        public override void Solve(IEnumerable<string> testData)
        {
            var testDataList = testData.ToList();

            PrintResult(testDataList.Sum(x => GetMaxJoltage(x, 2)));
            PrintResult(testDataList.Sum(x => GetMaxJoltage(x, 12)));
        }

        private long GetMaxJoltage(string bank, int digits)
        {
            var digitsList = new List<int>();
            var highestPreviousIndex = -1;
            for (var crntDigits = digits; crntDigits > 0; crntDigits--)
            {
                var highest = 0;
                for (var i = highestPreviousIndex + 1; i < bank.Length - crntDigits + 1; i++)
                {
                    var crnt = bank[i] - '0';
                    if (crnt > highest)
                    {
                        highest = crnt;
                        highestPreviousIndex = i;

                        if (crnt == '9')
                        {
                            break;
                        }
                    }
                }

                digitsList.Add(highest);
            }

            return long.Parse(string.Join("", digitsList));
        }
    }
}
