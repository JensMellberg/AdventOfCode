using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Twenty
{
    public class Problem11 : StringProblem
    {
        public override void Solve(IEnumerable<string> testData)
        {
            var seats = Matrix.FromTestInput<char>(testData);
            
            this.PrintResult(GetSeatCount(seats, x => true, 4));
            this.PrintResult(GetSeatCount(seats, x => x != '.', 5));
        }

        private int GetSeatCount(Matrix<char> seats, Func<char, bool> predicate, int seatBreakPoint)
        {
            Matrix<char> previousSeats = null;
            while (!seats.Equals(previousSeats))
            {
                var copy = seats.Copy();
                for (var x = 0; x < seats.ColumnCount; x++)
                {
                    for (var y = 0; y < seats.RowCount; y++)
                    {
                        if (seats[x, y] == '.')
                        {
                            continue;
                        }

                        var totalChecked = 0;
                        var occupiedSeen = 0;
                        foreach ((int x, int y) delta in new[] { (-1, 0), (1, 0), (0, -1), (0, 1), (1, 1), (-1, 1), (-1, -1), (1, -1)})
                        {
                            (int x, int y) pos = (x + delta.x, y + delta.y);
                            while (seats.IsInBounds(pos.x, pos.y) && !predicate(seats[pos.x, pos.y]))
                            {
                                pos.x += delta.x;
                                pos.y += delta.y;
                            }

                            if (seats.IsInBounds(pos.x, pos.y))
                            {
                                var seen = seats[pos.x, pos.y];
                                totalChecked++;
                                if (seen == '#')
                                {
                                    occupiedSeen++;
                                }

                                if (seen == '#' && seats[x, y] == 'L')
                                {
                                    break;
                                }

                                if (seats[x, y] == '#' && occupiedSeen >= seatBreakPoint)
                                {
                                    copy[x, y] = 'L';
                                    break;
                                }
                                else if (seen == '#' && 8 - totalChecked + occupiedSeen < seatBreakPoint)
                                {
                                    break;
                                }
                            }
                        }

                        if (seats[x, y] == 'L' && occupiedSeen == 0)
                        {
                            copy[x, y] = '#';
                        }
                    }
                }

                previousSeats = seats;
                seats = copy;
            }

            return seats.AllValues().Count(x => x == '#');
        }
    }
}
