using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;

namespace AdventOfCode.TwentyFour
{
    public class Problem21 : StringProblem
    {
        public static Dictionary<char, (int x, int y)> keyPadDict = new Dictionary<char, (int x, int y)>()
        {
            {'^', (1, 0) },
            {'A', (2, 0) },
            {'<', (0, 1) },
            {'v', (1, 1) },
            {'>', (2, 1) }
        };

        public static Dictionary<char, (int x, int y)> numberPadDict = new Dictionary<char, (int x, int y)>()
        {
            {'7', (0, 0) },
            {'8', (1, 0) },
            {'9', (2, 0) },
            {'4', (0, 1) },
            {'5', (1, 1) },
            {'6', (2, 1) },
            {'1', (0, 2) },
            {'2', (1, 2) },
            {'3', (2, 2) },
            {'0', (1, 3) },
            {'A', (2, 3) },
        };

        public static Dictionary<(int x, int y, string part), (string res, int x, int y)> ResultDict = new Dictionary<(int x, int y, string part), (string res, int x, int y)>();

        public override void Solve(IEnumerable<string> testInput)
        {
            var numberPad = this.CreateNumberPad(2);
            var numberPad2 = this.CreateNumberPad(25);

            long result = 0;
            long result2 = 0;

            foreach (var code in testInput)
            {
                var length1 = numberPad.GetFromMatrix(code);
                var length2 = numberPad2.GetFromMatrix(code);
                var numericCode = int.Parse(code[..3]);
                result += length1 * numericCode;
                result2 += length2 * numericCode;
            }

            this.PrintResult(result);
            this.PrintResult(result2);
        }

        private KeyPad CreateNumberPad(int robotCount)
        {
            KeyPad previousRobot = null;
            for (var i = 0; i < robotCount; i++)
            {
                var robot = new KeyPad(keyPadDict, previousRobot);
                previousRobot = robot;
            }

            return new KeyPad(numberPadDict, previousRobot);
        }

        private class KeyPad
        {
            private Dictionary<char, (int x, int y)> keypad;

            private HashSet<(int x, int y)> allowedPoses;

            private KeyPad wrapped;

            public KeyPad(Dictionary<char, (int x, int y)> keypad, KeyPad wrapped)
            {
                this.keypad = keypad;
                this.wrapped = wrapped;
                this.allowedPoses = keypad.Values.ToHashSet();
            }

            public long GetFromMatrix(string result)
            {
                var moveMatrix = this.CreateMoveMatrix();
                var lastChar = 'A';
                long totalDist = 0;
                foreach (var c in result)
                {
                    totalDist += moveMatrix[(lastChar, c)];
                    lastChar = c;
                }

                return totalDist;
            }

            public Dictionary<(char from, char to), long> CreateMoveMatrix()
            {
                var parentMatrix = this.wrapped?.CreateMoveMatrix() ?? new Dictionary<(char from, char to), long>();
                var dict = new Dictionary<(char from, char to), long>();
                var allDirs = this.keypad.Keys;
                foreach (var fromChar in allDirs)
                {
                    foreach (var toChar in allDirs)
                    {
                        var from = this.keypad[fromChar];
                        var to = this.keypad[toChar];
                        var (deltaX, deltaY) = (to.x - from.x, to.y - from.y);
                        var minLength = long.MaxValue;
                        var horizontalDirs = Enumerable.Repeat(deltaX > 0 ? Direction.Right : Direction.Left, Math.Abs(deltaX)).ToList();
                        var verticalDirs = Enumerable.Repeat(deltaY > 0 ? Direction.Down : Direction.Up, Math.Abs(deltaY)).ToList();
                        var resultDirs = new List<Direction>();

                        if (verticalDirs.Any() && horizontalDirs.Any())
                        {
                            var horzCopy = horizontalDirs.ToList();
                            var vertsCopy = verticalDirs.ToList();
                            var horizontalDelta = horizontalDirs[0].GetDelta();
                            var verticalDelta = verticalDirs[0].GetDelta();
                            var tempDirs = new List<Direction>();
                            var tempFrom = from;
                            AddHorizontals();
                            AddVerticals();
                            AddHorizontals();

                            minLength = GetDirsLength(tempDirs);
                            tempDirs.Clear();
                            horzCopy = horizontalDirs.ToList();
                            vertsCopy = verticalDirs.ToList();
                            tempFrom = from;

                            AddVerticals();
                            AddHorizontals();
                            AddVerticals();

                            minLength = Math.Min(GetDirsLength(tempDirs), minLength);

                            void AddVerticals()
                            {
                                AddToTempDirs(vertsCopy, verticalDelta);
                            }

                            void AddHorizontals()
                            {
                                AddToTempDirs(horzCopy, horizontalDelta);
                            }

                            void AddToTempDirs(List<Direction> sourceList, (int x, int y) delta)
                            {
                                while (sourceList.Any() && allowedPoses.Contains((tempFrom.x + delta.x, tempFrom.y + delta.y)))
                                {
                                    tempDirs.Add(sourceList[0]);
                                    sourceList.RemoveAt(0);
                                    tempFrom.x += delta.x;
                                    tempFrom.y += delta.y;
                                }
                            }
                        }
                        else if (horizontalDirs.Any())
                        {
                            minLength = GetDirsLength(horizontalDirs);
                        }
                        else
                        {
                            minLength = GetDirsLength(verticalDirs);
                        }

                        dict[(fromChar, toChar)] = minLength;
                        long GetDirsLength(List<Direction> dirs)
                        {
                            var fromChar = 'A';
                            long total = 0;
                            var chars = dirs.Select(x => DirectionToChar(x)).ToList();
                            chars.Add('A');
                            foreach (var toChar in chars)
                            {
                                if (!parentMatrix.TryGetValue((fromChar, toChar), out var length))
                                {
                                    length = 1;
                                }

                                total += length;
                                fromChar = toChar;
                            }

                            return total;
                        }
                    }
                }

                return dict;
            }

            private static char DirectionToChar(Direction d) => d switch
            {
                Direction.Left => '<',
                Direction.Right => '>',
                Direction.Down => 'v',
                Direction.Up => '^',
                _ => throw new Exception(),
            };
        }
    }
}
