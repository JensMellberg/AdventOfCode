using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Twenty
{
    public class Problem12 : StringProblem
    {
        public override void Solve(IEnumerable<string> testData)
        {
            var currentDir = Direction.Right;
            (long x, long y) currentPos = (0, 0);
            (long x, long y) currentPos2 = (0, 0);
            (long x, long y) wayPointDiff = (10, -1);
            foreach (var instr in testData)
            {
                var type = instr[0];
                var value = int.Parse(instr.Substring(1));
                switch(type)
                {
                    case 'N': 
                        currentPos.y -= value;
                        wayPointDiff.y -= value;
                        break;
                    case 'S': 
                        currentPos.y += value;
                        wayPointDiff.y += value;
                        break;
                    case 'W': 
                        currentPos.x -= value;
                        wayPointDiff.x -= value;
                        break;
                    case 'E': 
                        currentPos.x += value;
                        wayPointDiff.x += value;
                        break;
                    case 'L': 
                        Turn(360 - value);
                        break;
                    case 'R': 
                        Turn(value); 
                        break;
                    case 'F': 
                        currentPos.x += currentDir.GetDelta().x * value; 
                        currentPos.y += currentDir.GetDelta().y * value;
                        currentPos2.x += wayPointDiff.x * value;
                        currentPos2.y += wayPointDiff.y * value;
                        break;
                }
            }

            this.PrintResult(Math.Abs(currentPos.x) + Math.Abs(currentPos.y));
            this.PrintResult(Math.Abs(currentPos2.x) + Math.Abs(currentPos2.y));
            void Turn(int degrees)
            {
                if (degrees == 90)
                {
                    currentDir = currentDir.TurnClockwise();
                    TurnWayPoint(1);
                }
                else if (degrees == 180)
                {
                    currentDir = currentDir.Reverse();
                    TurnWayPoint(2);
                }
                else if (degrees == 270)
                {
                    currentDir = currentDir.Reverse().TurnClockwise();
                    TurnWayPoint(3);
                }

                void TurnWayPoint(int times)
                {
                    for (var i = 0; i < times; i++)
                    {
                        wayPointDiff = (-wayPointDiff.y, wayPointDiff.x);
                    }
                }
            }
        }
    }
}
