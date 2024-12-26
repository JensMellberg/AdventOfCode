using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AdventOfCode.Twenty
{
    public class Problem8 : PatternProblem<(string instr, int value)>
    {
        protected override string Pattern => "¤instr¤ ¤value¤";

        public override void Solve(IEnumerable<(string instr, int value)> testData)
        {
            var testList = testData.ToList();

            this.PrintResult(GetAccValue(testList, out var _));

            for (var i = 0; i < testList.Count; i++)
            {
                var newList = testList.ToList();
                if (testList[i].instr == "nop")
                {
                    newList[i] = ("jmp", testList[i].value);
                }
                else if (testList[i].instr == "jmp")
                {
                    newList[i] = ("nop", testList[i].value);
                }
                else
                {
                    continue;
                }

                var accValue = this.GetAccValue(newList, out var didFinish);
                if (didFinish)
                {
                    this.PrintResult(accValue);
                }
            }
        }

        private long GetAccValue(IList<(string instr, int value)> program, out bool didFinish)
        {
            long accValue = 0;
            var pointer = 0;
            
            var instructionsPerformed = new HashSet<int>();
            while (!instructionsPerformed.Contains(pointer) && pointer < program.Count)
            {
                instructionsPerformed.Add(pointer);
                (var instr, var value) = program[pointer];

                switch (instr)
                {
                    case "acc": accValue += value; pointer++; break;
                    case "nop": pointer++; break;
                    case "jmp": pointer += value; break;
                    default: throw new Exception();
                }
            }

            didFinish = pointer == program.Count;
            return accValue;
        }
    }
}
