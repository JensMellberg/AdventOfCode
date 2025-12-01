using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;

namespace AdventOfCode.Twenty
{
    public class Problem17 : StringProblem
    {
        public override void Solve(IEnumerable<string> testData)
        {
            var inMatrix = Matrix.FromTestInput<char>(testData);
            var activePositions = new HashSet<(int x, int y, int z)>();
            var activePositions4D = new HashSet<(int x, int y, int z, int w)>();
            for (var x = 0; x < inMatrix.ColumnCount; x++)
            {
                for (var y = 0; y < inMatrix.RowCount; y++)
                {
                    if (inMatrix[x, y] == '#')
                    {
                        activePositions.Add((x, y, 0));
                        activePositions4D.Add((x, y, 0, 0));
                    }
                }
            }
            

            this.PrintResult(SimulateRounds(activePositions, GetConnectedPoints));
            this.PrintResult(SimulateRounds(activePositions4D, GetConnectedPoints4D));
        }

        private int SimulateRounds<T>(HashSet<T> activePositions, Func<T, IEnumerable<T>> getConnectedPoints)
        {
            for (var i = 0; i < 6; i++)
            {
                var newPositions = activePositions.ToHashSet();
                var checkedInactives = new HashSet<T>();
                foreach (var cube in activePositions)
                {
                    var activeCount = 0;
                    foreach (var position in getConnectedPoints(cube))
                    {
                        if (activePositions.Contains(position))
                        {
                            activeCount++;
                        }
                        else if (checkedInactives.Add(position))
                        {
                            var activeCount2 = 0;
                            foreach (var position2 in getConnectedPoints(position).Where(p => activePositions.Contains(p)))
                            {
                                activeCount2++;
                                if (activeCount2 > 3)
                                {
                                    break;
                                }
                            }

                            if (activeCount2 == 3)
                            {
                                newPositions.Add(position);
                            }
                        }
                    }

                    if (activeCount < 2 || activeCount > 3)
                    {
                        newPositions.Remove(cube);
                    }
                }

                activePositions = newPositions;
            }

            return activePositions.Count;
        }

        private static IEnumerable<(int x, int y, int z)> GetConnectedPoints((int x, int y, int z) point)
        {
            var (x, y, z) = point;
            for (var xD = -1; xD < 2; xD++)
            {
                for (var yD = -1; yD < 2; yD++)
                {
                    for (var zD = -1; zD < 2; zD++)
                    {
                        var newPoint = (x + xD, y + yD, z + zD);
                        if (!newPoint.Equals(point))
                        {
                            yield return newPoint;
                        }
                    }
                }
            }
        }

        private static IEnumerable<(int x, int y, int z, int w)> GetConnectedPoints4D((int x, int y, int z, int w) point)
        {
            var (x, y, z, w) = point;
            for (var xD = -1; xD < 2; xD++)
            {
                for (var yD = -1; yD < 2; yD++)
                {
                    for (var zD = -1; zD < 2; zD++)
                    {
                        for (var wD = -1; wD < 2; wD++)
                        {
                            var newPoint = (x + xD, y + yD, z + zD, w + wD);
                            if (!newPoint.Equals(point))
                            {
                                yield return newPoint;
                            }
                        }
                    }
                }
            }
        }
    }
}
