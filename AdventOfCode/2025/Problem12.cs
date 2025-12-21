using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.TwentyFive
{
    public class Problem12 : StringProblem
    {
        protected override EmptyStringBehavior EmptyStringBehavior => EmptyStringBehavior.Keep;
        public override void Solve(IEnumerable<string> testData)
        {
            var testDataList = testData.ToList();
            var index = 0;
            var presents = new List<Present>();
            var spacesToFill = new List<SpaceToFill>();
            while (testDataList[index][1] == ':')
            {
                index++;
                var presentLines = testDataList.Skip(index).TakeWhile(line =>
                {
                    index++;
                    return line != "";
                });
                presents.Add(new Present(Matrix.FromTestInput<char>(presentLines)));
            }

            while (index < testDataList.Count - 1)
            {
                var tokens = testDataList[index].Split(' ');
                var sizes = tokens[0].Replace(":", "").Split('x').Select(int.Parse).ToArray();
                spacesToFill.Add(new SpaceToFill
                {
                    Width = sizes[0],
                    Height = sizes[1],
                    PresentCounts = tokens[1..].Select(int.Parse).ToArray()
                });
                index++;
            }

            var b = 4;
        }

        private class SpaceToFill
        {
            public int Width { get; set; }

            public int Height { get; set; }

            public int[] PresentCounts { get; set; }
        }

        private class Present
        {
            private Matrix<char> shape;
            public Present(Matrix<char> shape)
            {
                this.shape = shape;
            }
        }
    }
}
