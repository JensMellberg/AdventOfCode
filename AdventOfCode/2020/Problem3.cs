using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Twenty
{
    public class Problem3 : StringProblem
    {
        public override void Solve(IEnumerable<string> testData)
        {
            var matrix = Matrix.FromTestInput<char>(testData);
            var isFirst = true;
            long total = 1;
            foreach (var slope in new[] { (3, 1), (1, 1), (5, 1), (7, 1), (1, 2)})
            {
                var result = this.CountTrees(matrix, slope);
                if (isFirst)
                {
                    this.PrintResult(result);
                    isFirst = false;
                }

                total *= result;
            }

            this.PrintResult(total);
        }

        private int CountTrees(Matrix<char> matrix, (int x, int y) slope)
        {
            (int x, int y) pos = (0, 0);
            var treeCount = 0;
            while (pos.y < matrix.RowCount)
            {
                if (matrix[pos.x, pos.y] == '#')
                {
                    treeCount++;
                }

                pos.x = (pos.x + slope.x) % matrix.ColumnCount;
                pos.y += slope.y;
            }

            return treeCount;
        }
    }
}
