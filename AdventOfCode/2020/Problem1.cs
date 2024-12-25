using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Twenty
{
    public class Problem1 : IntegerProblem
    {

        public override void Solve(IEnumerable<int> testData)
        {
            var dataList = testData.ToList();
            for (var i1 = 0; i1 < dataList.Count; i1++)
            {
                for (var i2 = i1 + 1; i2 < dataList.Count; i2++)
                {
                    if (dataList[i1] + dataList[i2] == 2020)
                    {
                        this.PrintResult(dataList[i1] * dataList[i2]);
                        i1 = dataList.Count;
                        break;
                    }
                }
            }

            for (var i1 = 0; i1 < dataList.Count; i1++)
            {
                for (var i2 = i1 + 1; i2 < dataList.Count; i2++)
                {
                    for (var i3 = i2 + 1; i3 < dataList.Count; i3++)
                    {
                        if (dataList[i1] + dataList[i2] + dataList[i3] == 2020)
                        {
                            this.PrintResult(dataList[i1] * dataList[i2] * dataList[i3]);
                            i1 = dataList.Count;
                            i2 = dataList.Count;
                            break;
                        }
                    }
                }
            }
        }
    }
}
