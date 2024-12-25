using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace AdventOfCode.Twenty
{
    public class Problem4 : GroupedObjectProblem<Passport, PassportLine>
    {
        public override void Solve(IEnumerable<Passport> testData)
        {
            var validCount = 0;
            var validCount2 = 0;
            foreach (var passport in testData)
            {
                if (passport.HasAllFields()) {
                    validCount++;
                }

                if (passport.IsValid())
                {
                    validCount2++;
                }
            }

            this.PrintResult(validCount);
            this.PrintResult(validCount2);
        }
    }

    public class Passport : ParsableGroup<PassportLine>
    {
        public Dictionary<string, string> GetAllFields() => this.Contents.SelectMany(x => x.GetPairs()).ToDictionary(x => x.key, x => x.value);

        public bool HasAllFields() => this.GetAllFields().Where(x => x.Key != "cid").Count() == 7;

        public bool IsValid()
        {
            if (!HasAllFields())
            {
                return false;
            }

            var dict = this.GetAllFields();
            if (int.Parse(dict["byr"]) < 1920 || int.Parse(dict["byr"]) > 2002)
            {
                return false;
            }

            if (int.Parse(dict["iyr"]) < 2010 || int.Parse(dict["iyr"]) > 2020)
            {
                return false;
            }

            if (int.Parse(dict["eyr"]) < 2020 || int.Parse(dict["eyr"]) > 2030)
            {
                return false;
            }

            var height = dict["hgt"];
            if (height.Length < 3)
            {
                return false;
            }

            var end = height[^2..];
            var number = int.Parse(height.Substring(0, height.Length - 2));
            if (end == "cm")
            {
                if (number < 150 || number > 193)
                {
                    return false;
                }
            }
            else if (end == "in")
            {
                if (number < 59 || number > 76)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            if (!new Regex("^#[a-fA-F0-9]{6}$").IsMatch(dict["hcl"]))
            {
                return false;
            }

            if (!"amb blu brn gry grn hzl oth".Split(' ').Contains(dict["ecl"]))
            {
                return false;
            }

            var pid = dict["pid"];
            if (pid.Length != 9 || !pid.All(x => ParsableUtils.IsNumber(x)))
            {
                return false;
            }

            return true;
        }
    }

    public class PassportLine : ListParsable<string>
    {
        protected override string Separator => " ";

        public IEnumerable<(string key, string value)> GetPairs() => this.Values.Select(x => x.Split(':')).Select(x => (x[0], x[1]));
    }
}
