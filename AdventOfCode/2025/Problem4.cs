using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.TwentyFive
{
    public class Problem4 : StringProblem
    {
        public override void Solve(IEnumerable<string> testData)
        {
            var matrix = Matrix.FromTestInput<bool>(testData, x => x == '@');
            var (newMatrix, removeCount) = RemoveRolls(matrix);
            PrintResult(removeCount);
            var totalRemoveCount = removeCount;
            while (removeCount > 0)
            {
                (newMatrix, removeCount) = RemoveRolls(newMatrix);
                totalRemoveCount += removeCount;
            }

            PrintResult(totalRemoveCount);
        }

        private (Matrix<bool> newMatrix, int removeCount) RemoveRolls(Matrix<bool> matrix)
        {
            var accessibleRolls = 0;
            var copy = matrix.Copy();
            for (var x = 0; x < matrix.ColumnCount; x++)
            {
                for (var y = 0; y < matrix.RowCount; y++)
                {
                    if (!matrix[x, y])
                    {
                        continue;
                    }

                    var adjacents = matrix.GetAdjacentCoordinatesDiagonally(x, y).Select(v => matrix[v.x, v.y]);
                    if (adjacents.Count(x => x) < 4)
                    {
                        accessibleRolls++;
                        copy[x, y] = false;
                    }
                }
            }

            return (copy, accessibleRolls);
        }
    }
}
