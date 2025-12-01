using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Twenty
{
    public class Problem19 : SplitProblem<(int nbr, string ruleText), StringParsable>
    {
        protected override string FirstPattern => "¤nbr¤: ¤ruleText¤";
        protected override void Solve(IEnumerable<(int nbr, string ruleText)> ruleInput, IEnumerable<StringParsable> input)
        {
            var rules = new Dictionary<int, IRule>();
            foreach (var ruleTuple in ruleInput)
            {
                var text = ruleTuple.ruleText;
                if (text.Contains('|'))
                {
                    var tokens = text.Split('|');
                    rules.Add(ruleTuple.nbr, new EitherRule(ParseCombinedRule(tokens[0]), ParseCombinedRule(tokens[1])));
                }
                else if (text.Contains('"'))
                {
                    rules.Add(ruleTuple.nbr, new LetterRule(text[1]));
                }
                else
                {
                    rules.Add(ruleTuple.nbr, ParseCombinedRule(text));
                }
            }

            var allPossible = rules[0].GetMatchesWithCache(rules).ToHashSet();
            this.PrintResult(input.Count(x => allPossible.Contains(x.Value)));
            IRule ParseCombinedRule(string line)
            {
                var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                var second = tokens.Length > 1 ? tokens[1] : -1;
                return new CombinedRule(tokens[0], second);
            }
        }

        private abstract class IRule
        {
            private IEnumerable<string> cache;
            protected abstract IEnumerable<string> GetMatches(Dictionary<int, IRule> rules);

            public IEnumerable<string> GetMatchesWithCache(Dictionary<int, IRule> rules)
            {
                if (this.cache == null)
                {
                    this.cache = this.GetMatches(rules);
                }

                return this.cache;
            }
        }

        private class EitherRule : IRule
        {
            private IRule rule1;
            private IRule rule2;
            public EitherRule(IRule rule1, IRule rule2)
            {
                this.rule1 = rule1;
                this.rule2 = rule2;
            }

            protected override IEnumerable<string> GetMatches(Dictionary<int, IRule> rules)
            {
                return this.rule1.GetMatchesWithCache(rules).Concat(this.rule2.GetMatchesWithCache(rules));
            }
        }

        private class CombinedRule : IRule
        {
            private int rule1;
            private int rule2;
            public CombinedRule(int rule1, int rule2)
            {
                this.rule1 = rule1;
                this.rule2 = rule2;
            }

            protected override IEnumerable<string> GetMatches(Dictionary<int, IRule> rules)
            {
                var rule1Matches = rules[this.rule1].GetMatchesWithCache(rules);
                var rule2Matches = this.rule2 == -1 ? new[] { "" } : rules[this.rule2].GetMatchesWithCache(rules);

                foreach (var rule in rule1Matches)
                {
                    foreach (var rule2 in rule2Matches)
                    {
                        yield return rule + rule2;
                    }
                }
            }
        }

        private class LetterRule : IRule
        {
            private char letter;
            public LetterRule(char letter)
            {
                this.letter = letter;
            }

            protected override IEnumerable<string> GetMatches(Dictionary<int, IRule> rules) => new[] { this.letter.ToString() };
        }
    }
}
