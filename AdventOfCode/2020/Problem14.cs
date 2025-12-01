using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Twenty
{
    public class Problem14 : StringProblem
    {
        public override void Solve(IEnumerable<string> testData)
        {
            var parser = new PatternParser("mem$¤address¤] = ¤value¤");
            ulong mask0s = 0;
            ulong mask1s = 0;
            string currentMask = "";
            var memory = new Dictionary<long, ulong>();
            var memory2 = new Dictionary<ulong, long>();

            foreach (var line in testData)
            {
                if (line.StartsWith("mask"))
                {
                    SetMask(line.Split(' ').Last());
                    continue;
                }

                var (address, value) = parser.ParseObject<(long address, long value)>(line);
                var valueAfterMask = ((ulong)value & mask0s) | mask1s;
                memory[address] = valueAfterMask;

                var addressAfterMask = (ulong)address | mask1s;
                var addressBinary = Convert.ToString((long)addressAfterMask, 2);
                if (addressBinary.Length < currentMask.Length)
                {
                    addressBinary = string.Join("", Enumerable.Repeat('0', currentMask.Length - addressBinary.Length)) + addressBinary;
                }

                var allAddresses = GetAddresses(addressBinary, currentMask.Length - 1);
                var decimalAddresses = allAddresses.Select(x => Convert.ToUInt64(string.Join("", x), 2));
                decimalAddresses.ForEach(x => memory2[x] = value);
            }

            ulong result = 0;
            long result2 = 0;
            foreach (var val in memory.Values)
            {
                result += val;
            }

            foreach (var val in memory2.Values)
            {
                result2 += val;
            }

            this.PrintResult(result);
            this.PrintResult(result2);

            void SetMask(string mask)
            {
                currentMask = mask;
                mask0s = Convert.ToUInt64(string.Join("", mask.Select(x => x == '0' ? '0' : '1')), 2);
                mask1s = Convert.ToUInt64(string.Join("", mask.Select(x => x == '1' ? '1' : '0')), 2);
            }

            IEnumerable<string> GetAddresses(string address, int index)
            {
                while (index >= 0)
                {
                    if (currentMask[index] == 'X')
                    {
                        var withOne = address.ReplaceAtIndex(index, '1');
                        var withZero = address.ReplaceAtIndex(index, '0');
                        return GetAddresses(withOne, index - 1).Concat(GetAddresses(withZero, index - 1));
                    }

                    index--;
                }

                return new[] { address };
            }
        }
    }
}
