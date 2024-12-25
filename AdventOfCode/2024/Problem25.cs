using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace AdventOfCode.TwentyFour
{
    public class Problem25 : GroupedObjectProblem<KeyOrLock, StringParsable>
    {

        public override void Solve(IEnumerable<KeyOrLock> testInput)
        {
            var height = testInput.First().GetMatrix().ColumnCount;
            var locks = new List<int[]>();
            var keys = new List<int[]>();
            foreach (var keyOrLock in testInput)
            {
                var targetList = keyOrLock.IsLock ? locks : keys;
                targetList.Add(keyOrLock.GetMatrix().GetColumns().Select(x => x.Count(c => c == '#')).ToArray());
            }

            var fits = 0;
            foreach (var key in keys)
            {
                foreach (var lockk in locks)
                {
                    if (KeyFitsLock(key, lockk))
                    {
                        fits++;
                    }
                }
            }

            this.PrintResult(fits);

            bool KeyFitsLock(int[] keyVector, int[] lockVector) => keyVector.Select((x, i) => (x, i)).All(p => p.x + lockVector[p.i] <= height);
        }
    }

    public class KeyOrLock : ParsableGroup<StringParsable>
    {
        public bool IsLock => this.Contents[0].Value.All(x => x == '#');

        public Matrix<char> GetMatrix()
        {
            return Matrix.FromTestInput<char>(this.Contents.Skip(1).Take(this.Contents.Count - 2).Select(x => x.Value));
        }
    }
}
