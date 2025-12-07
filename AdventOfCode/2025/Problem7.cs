using System.Collections.Generic;

namespace AdventOfCode.TwentyFive
{
    public class Problem7 : StringProblem
    {
        public override void Solve(IEnumerable<string> testData)
        {
            var matrix = Matrix.FromTestInput<char>(testData);
            var beamsPoses = Matrix.InitWithStartValue(matrix.RowCount, matrix.ColumnCount, (Beam)null);
            var beams = new List<Beam>();
            var start = matrix.Find('S');
            beams.Add(new Beam(start));
            beams[0].ShouldMove = true;
            var hasFinished = false;
            var splits = 0;
            long quantumSplits = 1;
            while (!hasFinished)
            {
                for (var i = 0; i < beams.Count; i++)
                {
                    var beam = beams[i];
                    if (!beam.ShouldMove)
                    {
                        beam.ShouldMove = true;
                        continue;
                    }

                    beam.Y++;
                    if (beam.Y == matrix.RowCount - 1)
                    {
                        hasFinished = true;
                    }

                    if (beamsPoses[beam.X, beam.Y] != null)
                    {
                        beamsPoses[beam.X, beam.Y].MergeCount += beam.MergeCount;
                        beams.RemoveAt(i);
                        i--;
                    }
                    else if (matrix[beam.X, beam.Y] == '^')
                    {
                        splits++;
                        quantumSplits += beam.MergeCount;
                        beams.RemoveAt(i);
                        i--;
                        AddBeam((beam.X - 1, beam.Y));
                        AddBeam((beam.X + 1, beam.Y));
                    }
                    else
                    {
                        beamsPoses[beam.X, beam.Y] = beam;
                    }

                    void AddBeam((int x, int y) startPoint)
                    {
                        if (matrix.IsInBounds(startPoint.x, startPoint.y))
                        {
                            if (beamsPoses[startPoint.x, startPoint.y] != null)
                            {
                                beamsPoses[startPoint.x, startPoint.y].MergeCount += beam.MergeCount;
                                return;
                            }

                            var newBeam = new Beam(startPoint)
                            {
                                MergeCount = beam.MergeCount
                            };
                            beams.Add(newBeam);
                            beamsPoses[startPoint.x, startPoint.y] = newBeam;
                        }
                    }
                }
            }

            PrintResult(splits);
            PrintResult(quantumSplits);
        }

        private class Beam
        {
            public Beam((int x, int y) point)
            {
                X = point.x;
                Y = point.y;
            }

            public bool ShouldMove { get; set; }

            public int X { get; set; }

            public int Y { get; set; }

            public (int x, int y) Point => (X, Y);

            public long MergeCount { get; set; } = 1;
        }
    }
}
