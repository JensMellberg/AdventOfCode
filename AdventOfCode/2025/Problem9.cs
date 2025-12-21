using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace AdventOfCode.TwentyFive
{
    public class Problem9 : ObjectProblem<Point>
    {
        public override void Solve(IEnumerable<Point> testData)
        {
            long maxArea = 0;
            long restrictedMaxArea = 0;
            var points = testData.Select(x => new PointWithLines(x)).ToList();
            var vertLines = new List<VerticalLine>();
            var horiLines = new List<HorizontalLine>();
            VerticalLine previousVertLine = null;
            HorizontalLine previousHoriLine = null;
            for (var i = 0; i < points.Count; i++)
            {
                var prevPoint = i == 0 ? points.Last() : points[i - 1];
                var point = points[i];
                if (prevPoint.Y != point.Y)
                {
                    VerticalLine line;
                    if (prevPoint.Y < point.Y)
                    {
                        line = new VerticalLine
                        {
                            Top = prevPoint,
                            Bottom = point,
                            TopLine = previousHoriLine
                        };
                    } else
                    {
                        line = new VerticalLine
                        {
                            Top = point,
                            Bottom = prevPoint,
                            BottomLine = previousHoriLine
                        };
                    }
                    vertLines.Add(line);
                    previousVertLine = line;
                    point.Lines.Add(line);
                    prevPoint.Lines.Add(line);

                    if (previousHoriLine != null)
                    {
                        previousHoriLine.RightLine ??= line;
                        previousHoriLine.LeftLine ??= line;
                    }
                } else
                {
                    HorizontalLine line;
                    if (prevPoint.X < point.X)
                    {
                        line = new HorizontalLine
                        {
                            Left = prevPoint,
                            Right = point,
                            LeftLine = previousVertLine
                        };
                    }
                    else
                    {
                        line = new HorizontalLine
                        {
                            Left = point,
                            Right = prevPoint,
                            RightLine = previousVertLine
                        };
                    }

                    horiLines.Add(line);
                    previousHoriLine = line;
                    point.Lines.Add(line);
                    prevPoint.Lines.Add(line);
                    if (previousVertLine != null)
                    {
                        previousVertLine.TopLine ??= line;
                        previousVertLine.BottomLine ??= line;
                    }
                }
            }

            vertLines.Last().BottomLine = horiLines.First();
            var huh = vertLines.Where(x => x.TopLine == null || x.BottomLine == null);
            var hu2h = horiLines.Where(x => x.LeftLine == null || x.RightLine == null);
            if (hu2h.Any())
            {
                horiLines.Last().RightLine = vertLines.First();
            }
            var topmostLine = horiLines.FindMin(x => x.Y);
            ILine previousLine = topmostLine;
            topmostLine.IsGreenUnder = true;
            ILine crntLine = topmostLine.RightLine;
            do
            {
                if (crntLine is VerticalLine vertLine)
                {
                    var prevHor = previousLine as HorizontalLine;
                    if (vertLine.TopLine == prevHor)
                    {
                        if (prevHor.RightLine == vertLine)
                        {
                            vertLine.IsGreenLeft = prevHor.IsGreenUnder;
                            vertLine.IsGreenRight = !vertLine.IsGreenLeft;
                        } else
                        {
                            vertLine.IsGreenRight = prevHor.IsGreenUnder;
                            vertLine.IsGreenLeft = !vertLine.IsGreenRight;
                        }
                        crntLine = vertLine.BottomLine;
                        previousLine = vertLine;
                    }
                    else
                    {
                        if (prevHor.RightLine == vertLine)
                        {
                            vertLine.IsGreenLeft = prevHor.IsGreenOver;
                            vertLine.IsGreenRight = !vertLine.IsGreenLeft;
                        }
                        else
                        {
                            vertLine.IsGreenRight = prevHor.IsGreenOver;
                            vertLine.IsGreenLeft = !vertLine.IsGreenRight;
                        }
                        crntLine = vertLine.TopLine;
                        previousLine = vertLine;
                    }
                } else
                {
                    var horLine = crntLine as HorizontalLine;
                    var prevVert = previousLine as VerticalLine;
                    if (horLine.LeftLine == prevVert)
                    {
                        if (prevVert.TopLine == horLine)
                        {
                            horLine.IsGreenUnder = prevVert.IsGreenRight;
                            horLine.IsGreenOver = !horLine.IsGreenUnder;
                        }
                        else
                        {
                            horLine.IsGreenOver = prevVert.IsGreenRight;
                            horLine.IsGreenUnder = !horLine.IsGreenOver;
                        }
                        crntLine = horLine.RightLine;
                        previousLine = horLine;
                    }
                    else
                    {
                        if (prevVert.TopLine == horLine)
                        {
                            horLine.IsGreenUnder = prevVert.IsGreenLeft;
                            horLine.IsGreenOver = !horLine.IsGreenUnder;
                        }
                        else
                        {
                            horLine.IsGreenOver = prevVert.IsGreenLeft;
                            horLine.IsGreenUnder = !horLine.IsGreenOver;
                        }
                        crntLine = horLine.LeftLine;
                        previousLine = horLine;
                    }
                }
            } while (crntLine != topmostLine);

            var pointsX = horiLines.SelectMany(x => new long[] { x.Left.X, x.Right.X }).Distinct().OrderBy(x => x).ToList();
            var diffs = new List<long>();
            for (var i = 1; i < pointsX.Count; i++)
            {
                diffs.Add(pointsX[i] - pointsX[i - 1]);
                if (diffs.Last() == 1)
                {
                    var p = 5;
                }
            }


            var ordDiff = diffs.OrderBy(x => x);
            var pointsY = vertLines.SelectMany(x => new long[] { x.Top.Y, x.Bottom.Y }).Distinct().OrderBy(x => x).ToList();
            var diffs2 = new List<long>();
            for (var i = 1; i < pointsY.Count; i++)
            {
                diffs2.Add(pointsY[i] - pointsY[i - 1]);
            }


            var ordDiff2 = diffs2.OrderBy(x => x);

            for (var i = 0; i < vertLines.Count; i++)
            {
                for (var x = i +1; x < vertLines.Count; x++)
                {
                    var line = vertLines[i];
                    var line2 = vertLines[x];
                    if (Math.Abs(line.X - line2.X) < 3)
                    {
                        var p = 5;
                    }
                }
            }

            for (var i = 0; i < horiLines.Count; i++)
            {
                for (var x = i + 1; x < horiLines.Count; x++)
                {
                    var line = horiLines[i];
                    var line2 = horiLines[x];
                    if (Math.Abs(line.Y - line2.Y) < 3)
                    {
                        var p = 5;
                    }
                }
            }

            for (var i = 0; i < points.Count; i++)
            {
                for (var x = i + 1; x < points.Count; x++)
                {
                    if (points[i].X == points[x].X || points[i].Y == points[x].Y)
                    {
                        continue;
                    }

                    var rect = new Rectangle(points[i], points[x]);
                    maxArea = Math.Max(maxArea, rect.Area);
                    if (rect.Area == 24)
                    {
                        var gg = 5;
                    }
                    
                    /*if (horiLines.Any(x => x.Left.X <= rect.TopLeft.X && x.Right.X >= rect.TopRight.X && x.Left.Y <= rect.TopLeft.Y))
                    {
                        if (horiLines.Any(x => x.Left.X <= rect.BottomLeft.X && x.Right.X >= rect.BottomRight.X && x.Left.Y >= rect.BottomRight.Y))
                        {
                            if (vertLines.Any(x => x.Top.Y <= rect.TopLeft.Y && x.Bottom.Y >= rect.BottomLeft.Y && x.Top.X <= rect.BottomLeft.X))
                            {
                                if (vertLines.Any(x => x.Top.Y <= rect.TopRight.Y && x.Bottom.Y >= rect.BottomRight.Y && x.Top.X <= rect.TopRight.X))
                                {
                                    restrictedMaxArea = Math.Max(restrictedMaxArea, rect.Area);
                                }
                            }
                        }
                    }*/

                    var topLine = new HorizontalLine { Left = new PointWithLines(rect.TopLeft), Right = new PointWithLines(rect.TopRight) };
                    var bottomLine = new HorizontalLine { Left = new PointWithLines(rect.BottomLeft), Right = new PointWithLines(rect.BottomRight) };
                    var leftLine = new VerticalLine { Top = new PointWithLines(rect.TopLeft), Bottom= new PointWithLines(rect.BottomLeft) };
                    var rightLine = new VerticalLine { Top = new PointWithLines(rect.TopRight), Bottom = new PointWithLines(rect.BottomRight) };
                    if (topLine.Left.Lines.Concat(topLine.Right.Lines).OfType<HorizontalLine>().All(x => x.IsGreenUnder)
                        && bottomLine.Left.Lines.Concat(bottomLine.Right.Lines).OfType<HorizontalLine>().All(x => x.IsGreenOver)
                        && leftLine.Top.Lines.Concat(leftLine.Bottom.Lines).OfType<VerticalLine>().All(x => x.IsGreenRight)
                        && rightLine.Top.Lines.Concat(rightLine.Bottom.Lines).OfType<VerticalLine>().All(x => x.IsGreenLeft))
                    {
                        if (!CrossesHorizontalLine(leftLine, true) && !CrossesHorizontalLine(rightLine, false)
                             && !CrossesVerticalLine(topLine, true) && !CrossesVerticalLine(bottomLine, false))
                        {
                            restrictedMaxArea = Math.Max(restrictedMaxArea, rect.Area);
                        }
                    }
                }
            }

            PrintResult(maxArea);
            PrintResult(restrictedMaxArea);

            bool CrossesVerticalLine(HorizontalLine line, bool checkTop)
            {
                foreach (var vertLine in vertLines)
                {
                    if (vertLine.Top.Y < line.Y && vertLine.Bottom.Y > line.Y && vertLine.X > line.Left.X && vertLine.X < line.Right.X)
                    {
                        return true;
                    }

                    else if (vertLine.Top.Y <= line.Y && vertLine.Bottom.Y >= line.Y && vertLine.X > line.Left.X && vertLine.X < line.Right.X) {
                        if (vertLine.Top.Y == line.Y && checkTop)
                        {
                            return true;
                        } else if (vertLine.Bottom.Y == line.Y && !checkTop)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            bool CrossesHorizontalLine(VerticalLine line, bool checkLeft)
            {
                foreach (var horLine in horiLines)
                {
                    if (horLine.Left.X < line.X && horLine.Right.X > line.X && horLine.Y > line.Top.Y && horLine.Y < line.Bottom.Y)
                    {
                        return true;
                    }
                    else if (horLine.Left.X <= line.X && horLine.Right.X >= line.X && horLine.Y > line.Top.Y && horLine.Y < line.Bottom.Y)
                    {
                        if (horLine.Left.X == line.X && checkLeft)
                        {
                            return true;
                        }
                        else if (horLine.Right.X == line.X && !checkLeft)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        private class PointWithLines : Point
        {
            public PointWithLines(Point p)
            {
                X = p.X;
                Y = p.Y;
                if (p is PointWithLines pWithLines)
                {
                    Lines = pWithLines.Lines;
                }
            }

            public List<ILine> Lines = new List<ILine>();
        }

        private class ILine
        {

        }

        private class VerticalLine : ILine
        {
            public PointWithLines Top { get; set; }

            public PointWithLines Bottom { get; set; }

            public long X => Top.X;

            public HorizontalLine TopLine { get; set; }

            public HorizontalLine BottomLine { get; set; }

            public bool IsGreenLeft { get; set; }

            public bool IsGreenRight { get; set; }
        }

        private class HorizontalLine : ILine
        {
            public PointWithLines Left { get; set; }

            public PointWithLines Right { get; set; }

            public VerticalLine LeftLine { get; set; }

            public VerticalLine RightLine { get; set; }

            public bool IsGreenUnder { get; set; }

            public bool IsGreenOver { get; set; }

            public long Y => Left.Y;
        }
    }
}
