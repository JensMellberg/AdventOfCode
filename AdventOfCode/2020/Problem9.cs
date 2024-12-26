using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Twenty
{
    public class Problem9 : LongProblem
    {
        public override void Solve(IEnumerable<long> testData)
        {
            var testList = testData.ToList();
            var numberList = testData.Take(25).ToList();
            long breakNumber = 0;
            foreach (var number in testData.Skip(25))
            {
                var isValid = false;
                for (var i = 0; i <  numberList.Count; i++)
                {
                    for (var f = i + 1; f < numberList.Count; f++)
                    {
                        if (number == numberList[f] + numberList[i])
                        {
                            isValid = true;
                            break;
                        }
                    }
                }

                if (!isValid)
                {
                    this.PrintResult(number);
                    breakNumber = number;
                    break;
                }

                numberList.RemoveAt(0);
                numberList.Add(number);
            }

            for (var i = 0; i < testList.Count; i++)
            {
                long sum = testList[i];
                var fullSet = new List<long>
                {
                    testList[i]
                };

                for (var f = i + 1; f < testList.Count; f++)
                {
                    fullSet.Add(testList[f]);
                    sum += testList[f];
                    if (sum == breakNumber)
                    {
                        this.PrintResult(fullSet.Min() + fullSet.Max());
                        return;
                    }
                    else if (sum > breakNumber)
                    {
                        break;
                    }
                }
            }
        }
    }
}
