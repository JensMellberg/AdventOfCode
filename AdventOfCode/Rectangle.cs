namespace AdventOfCode
{
	public class Rectangle
	{
		public Point TopLeft { get; set; }

        public Point TopRight { get; set; }

        public Point BottomLeft { get; set; }

        public Point BottomRight { get; set; }

		public Rectangle(Point p1, Point p2)
		{
			if (p1.Y <= p2.Y)
			{
				if (p1.X <= p2.X)
				{
					TopLeft = p1;
                    TopRight = new Point(p2.X, p1.Y);
                    BottomLeft = new Point(p1.X, p2.Y);
					BottomRight = p2;
				}
				else
				{
                    TopLeft = new Point(p2.X, p1.Y);
					TopRight = p1;
                    BottomLeft = p2;
                    BottomRight = new Point(p1.X, p2.Y);
                }
			} else
			{
				var temp = new Rectangle(p2, p1);
				TopLeft = temp.TopLeft;
                TopRight = temp.TopRight;
                BottomLeft = temp.BottomLeft;
                BottomRight = temp.BottomRight;
            }
		}

		public long Area => (long)(TopLeft.YDistance(BottomRight, true) + 1) * (TopLeft.XDistance(BottomRight, true) + 1);
	}
}
