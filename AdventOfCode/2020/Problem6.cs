using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Twenty
{
    public class Problem6 : GroupedObjectProblem<StringGroup, StringParsable>
    {
        public override void Solve(IEnumerable<StringGroup> testData)
        {
            this.PrintResult(testData.Sum(x => x.Contents.SelectMany(x => x.Value).Distinct().Count()));
            this.PrintResult(testData.Sum(x => x.Contents.SelectMany(x => x.Value).GroupBy(x => x).Where(g => g.Count() == x.Contents.Count).Count()));
        }
    }
}
