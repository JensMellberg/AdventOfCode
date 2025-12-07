using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.TwentyFive
{
    public class Problem6 : StringProblem
    {
        public override void Solve(IEnumerable<string> testData)
        {
            var testDataList = testData.ToList();
            var columns = testDataList.Take(testDataList.Count - 1).Select(
                x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToList());
            var operators = testDataList.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            ulong sum = 0;

            for (var i = 0; i < operators.Length; i++)
            {
                var seed = operators[i] == "*" ? 1 : 0;
                var result = columns.Aggregate((ulong)seed, (v, c) => DoOperation(c[i], v, operators[i]));
                sum += result;
            }

            PrintResult(sum);

            Part2(testDataList.Take(testDataList.Count - 1).ToList(), testDataList.Last());
        }

        private void Part2(List<string> data, string operators)
        {
            var index = 0;
            string op = operators[0].ToString();
            var numbers = new List<ulong>();
            ulong sum = 0;
            while (index < data[0].Length)
            {
                var numberSb = new StringBuilder();
                for (var y = 0; y < data.Count; y++)
                {
                    var charAtPos = data[y][index];
                    if (charAtPos != ' ')
                    {
                        numberSb.Append(charAtPos);
                    }
                }

                var numberString = numberSb.ToString();
                if (string.IsNullOrEmpty(numberString))
                {
                    var result = numbers.Aggregate((v, c) => DoOperation(c, v, op));
                    sum += result;
                    numbers.Clear();
                }
                else
                {
                    if (numbers.Count == 0)
                    {
                        op = operators[index].ToString();
                    }
                    numbers.Add(ulong.Parse(numberString));
                }

                index++;
            }

            sum += numbers.Aggregate((v, c) => DoOperation(c, v, op));

            PrintResult(sum);
        }

        private ulong DoOperation(ulong nbr1, ulong nbr2, string op) => op == "*" ? nbr1 * nbr2 : nbr1 + nbr2;
    }
}
