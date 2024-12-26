using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Twenty
{
    public class Problem18 : StringProblem
    {
        protected override TabBehavior TabBehavior => TabBehavior.Reject;
        public override void Solve(IEnumerable<string> testData)
        {
            this.PrintResult(testData.Sum(x => Parse(x, false)));
            this.PrintResult(testData.Sum(x => Parse(x, true)));
        }

        private long Parse(string s, bool usePrecedence)
        {
            var _ = 0;
            var rr = Parse(s, ref _, usePrecedence);
            return rr;
        }

        private long Parse(string expString, ref int index, bool usePrecedence)
        {
            long wholeValue = 0;
            char lastOperator = '+';
            while (index < expString.Length)
            {
                var currentChar = expString[index];
                long value;
                if (currentChar == ' ')
                {
                    index++;
                    continue;
                }

                if (currentChar == '(')
                {
                    index++;
                    value = Parse(expString, ref index, usePrecedence);
                }
                else if (currentChar == ')')
                {
                    return wholeValue;
                }
                else if (ParsableUtils.IsNumber(currentChar))
                {
                    value = currentChar - '0';
                }
                else
                {
                    lastOperator = currentChar;
                    index++;

                    if (usePrecedence && currentChar == '*')
                    {
                        value = Parse(expString, ref index, usePrecedence);
                        if (index < expString.Length && expString[index] == ')')
                        {
                            index--;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                wholeValue = lastOperator == '+' ? wholeValue + value : wholeValue * value;
                index++;
            }

            return wholeValue;
        }
    }
}
