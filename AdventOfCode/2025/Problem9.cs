using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.TwentyFive
{
    public class Problem9 : ObjectProblem<Point>
    {
        public override void Solve(IEnumerable<Point> testData)
        {
            long maxArea = 0;
            long restrictedMaxArea = 0;
            var points = testData.ToList();
            var vertLines = new List<VerticalLine>();
            var horiLines = new List<HorizontalLine>();
            for (var i = 0; i < points.Count; i++)
            {
                var prevPoint = i == 0 ? points.Last() : points[i - 1];
                var point = points[i];
                if (prevPoint.Y != point.Y)
                {
                    var top = prevPoint.Y < point.Y ? prevPoint : point;
                    var bottom = prevPoint.Y < point.Y ? point : prevPoint;
                    vertLines.Add(new VerticalLine { Top = top, Bottom = bottom });
                } else
                {
                    var left = prevPoint.X < point.X ? prevPoint : point;
                    var right = prevPoint.X < point.X ? point : prevPoint;
                    horiLines.Add(new HorizontalLine { Left = left, Right = right });
                }
            }

            for (var i = 0; i < points.Count; i++)
            {
                for (var x = i + 1; x < points.Count; x++)
                {
                    var rect = new Rectangle(points[i], points[x]);
                    maxArea = Math.Max(maxArea, rect.Area);
                    
                    if (horiLines.Any(x => x.Left.X <= rect.TopLeft.X && x.Right.X >= rect.TopRight.X && x.Left.Y <= rect.TopLeft.Y))
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
                    }
                }
            }

            PrintResult(maxArea);
            PrintResult(restrictedMaxArea);
        }

        private class VerticalLine
        {
            public Point Top { get; set; }

            public Point Bottom { get; set; }
        }

        private class HorizontalLine
        {
            public Point Left { get; set; }

            public Point Right { get; set; }
        }
    }
}
