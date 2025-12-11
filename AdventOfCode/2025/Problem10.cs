using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.TwentyFive
{
    public class Problem10 : StringProblem
    {
        public override void Solve(IEnumerable<string> testData)
        {
            var boolMachineConfigs = new List<MachineConfig>();
            var intMachineConfigs = new List<MachineConfig>();
            foreach (var line in testData)
            {
                var tokens = line.Split(' ');
                var lightConfigToken = tokens[0][1..^1];
                var boolLightConfig = new BoolLightConfig { Config = new int[lightConfigToken.Length] };
                var intLightConfig = new IntLightConfig { Config = new int[lightConfigToken.Length] };
                var wantedLightConfig = new BoolLightConfig { Config = lightConfigToken.Select(x => x == '#' ? 1 : 0).ToArray() };

                var buttons = tokens[1..^1]
                    .Select(x => x[1..^1].Split(',').Select(int.Parse))
                    .Select(x => new Button { AffectedLights = x.ToList() })
                    .ToList();

                var joltageRequirements = new IntLightConfig
                {
                    Config = tokens.Last()[1..^1].Split(',').Select(int.Parse).ToArray()
                };

                boolMachineConfigs.Add(new MachineConfig
                {
                    lightConfig = boolLightConfig,
                    wantedLightConfig = wantedLightConfig,
                    buttons = buttons
                });

                intMachineConfigs.Add(new MachineConfig
                {
                    lightConfig = intLightConfig,
                    wantedLightConfig = joltageRequirements,
                    buttons = buttons
                });
            }

            //PrintResult(boolMachineConfigs.Sum(x => x.ClicksUntilDone()));
            PrintResult(intMachineConfigs.Sum(x => x.ClicksUntilDone()));
        }

        private class Button
        {
            public List<int> AffectedLights { get; set; }
        }

        private abstract class LightConfig
        {
            public int[] Config;

            public abstract void Click(Button button);

            public abstract LightConfig Clone();

            public abstract bool HasPassed(LightConfig other);

            public override bool Equals(object other)
            {
                if (other is LightConfig otherConfig)
                {
                    return Config.SequenceEqual(otherConfig.Config);
                }

                return false;
            }

            public override int GetHashCode()
            {
                var hash = 17;
                foreach (var l in Config)
                {
                    hash = hash * 31 + l;
                }
                   
                return hash;
            }
        }

        private class BoolLightConfig : LightConfig
        {
            public override void Click(Button button)
            {
                foreach (var i in button.AffectedLights)
                {
                    Config[i] = Config[i] == 1 ? 0 : 1;
                }
            }

            public override LightConfig Clone() => new BoolLightConfig { Config = Config.ToArray() };

            public override bool HasPassed(LightConfig other) => false;
        }

        private class IntLightConfig : LightConfig
        {
            public override void Click(Button button)
            {
                foreach (var i in button.AffectedLights)
                {
                    Config[i]++;
                }
            }

            public override LightConfig Clone() => new IntLightConfig { Config = Config.ToArray() };

            public override bool HasPassed(LightConfig other)
            {
                for (var i = 0; i < Config.Length; i++)
                {
                    if (Config[i] > other.Config[i])
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private class MachineConfig
        {
            public LightConfig lightConfig;
            public LightConfig wantedLightConfig;
            public List<Button> buttons;
            private int clicks;

            public int ClicksUntilDone()
            {
                var queue = new Queue<MachineConfig>();
                var seenConfigs = new HashSet<LightConfig>();

                Enqueue(queue, seenConfigs);
                while (queue.Any())
                {
                    var item = queue.Dequeue();
                    if (item.IsDone)
                    {
                        Console.WriteLine("Done");
                        return item.clicks;
                    }

                    item.Enqueue(queue, seenConfigs);
                }

                return -1;
            }

            private bool IsDone => lightConfig.Equals(wantedLightConfig);

            private void Enqueue(Queue<MachineConfig> queue, HashSet<LightConfig> seenConfigs)
            {
                if (seenConfigs.Contains(lightConfig) || lightConfig.HasPassed(wantedLightConfig))
                {
                    return;
                }

                seenConfigs.Add(lightConfig);
                foreach (var button in buttons)
                {
                    var clone = Clone();
                    clone.Click(button);
                    queue.Enqueue(clone);
                }
            }

            private void Click(Button button)
            {
                lightConfig.Click(button);
                clicks++;
            }

            private MachineConfig Clone() => new MachineConfig
            {
                buttons = buttons,
                wantedLightConfig = wantedLightConfig,
                lightConfig = lightConfig.Clone(),
                clicks = clicks
            };
        }
    }
}
