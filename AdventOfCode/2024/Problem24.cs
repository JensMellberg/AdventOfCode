using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace AdventOfCode.TwentyFour
{
    public class Problem24 : SplitProblem<ConstantGate, LogicGate>
    {
        protected override TabBehavior TabBehavior => TabBehavior.Reject;
        protected override string FirstPattern => "¤Name¤: ¤Value¤";

        protected override string SecondPattern => "¤Gate1¤ ¤GateType¤ ¤Gate2¤ -> ¤Name¤";

        protected override void Solve(IEnumerable<ConstantGate> constantGates, IEnumerable<LogicGate> logicGates)
        {
            var gates = new Gates(constantGates.OfType<Gate>().Concat(logicGates));
            var zGates = gates.GetZGateValues();
            this.Print(gates.Stringify());
            this.PrintResult(Convert.ToInt64(zGates, 2));
            this.PrintResult(GetSwitches(gates));
        }

        private string GetSwitches(Gates gates)
        {
            var switches = new List<string>();
            LogicGate previousMainXor = null;
            LogicGate previousAnd = null;
            LogicGate previousFullAnd = null;
            var computerLength = gates.GetZGateValues().Length;
            for (int index = 0; index < computerLength; index++)
            {
                var crntGate = (LogicGate)gates.Get("z" + PadNumber(index));
                if (index == 0)
                {
                    if (!IsMainXor(crntGate, index))
                    {
                        throw new Exception("Computer is invalid.");
                    }

                    previousMainXor = crntGate;
                    continue;
                }

                var gate1 = gates.Get(crntGate.Gate1) as LogicGate;
                var gate2 = gates.Get(crntGate.Gate2) as LogicGate;
                if (index == 1)
                {
                    if (IsMainXor(gate1, index) && IsMainAnd(gate2, index))
                    {
                        previousMainXor = gate1;
                        previousFullAnd = gate2;
                    }
                    else if (IsMainXor(gate2, index) && IsMainAnd(gate1, index))
                    {
                        previousMainXor = gate2;
                        previousFullAnd = gate1;
                    }
                    else
                    {
                        throw new Exception("Computer is invalid.");
                    }

                    continue;
                }

                LogicGate othGate = null;
                LogicGate thisMainXor = null;
                LogicGate thisMainAnd = null;

                if (index == computerLength - 1)
                {
                    if (IsMainAnd(gate1, index))
                    {
                        othGate = gate2;
                    }
                    else if (IsMainAnd(gate2, index))
                    {
                        othGate = gate1;
                    }
                    else
                    {
                        throw new Exception("Computer is invalid.");
                    }

                    if (!ArrayEquals(new[] { othGate.Gate1, othGate.Gate2 }, new[] { previousFullAnd.Name, previousMainXor.Name }))
                    {
                        throw new Exception("Computer is invalid.");
                    }

                    continue;
                }


                if (gate1 == null || gate2 == null || crntGate.GateType != "XOR")
                {
                    var actualMainXor = gates.gates.Values.OfType<LogicGate>().Where(x => IsMainXor(x, index)).First();
                    var switcher = gates.gates.Values.OfType<LogicGate>().Where(x => (x.Gate1 == actualMainXor.Name || x.Gate2 == actualMainXor.Name) && x.GateType == "XOR").First();
                    SwitchGates(switcher, crntGate);
                    
                    crntGate = switcher;
                    gate1 = gates.Get(crntGate.Gate1) as LogicGate;
                    gate2 = gates.Get(crntGate.Gate2) as LogicGate;
                }

                if (IsMainXor(gate1, index))
                {
                    thisMainXor = gate1;
                    othGate = gate2;
                }
                else if (IsMainXor(gate2, index)) {
                    thisMainXor = gate2;
                    othGate = gate1;
                }
                else
                {
                    LogicGate switcher;
                    if (gates.Get(gate1?.Gate1) is LogicGate convGate1 && gates.Get(gate1?.Gate2) is LogicGate convGate2
                        && (IsMainAnd(convGate1, index) || IsMainAnd(convGate2, index)))
                    {
                        switcher = gate2;
                        othGate = gate1;
                    }
                    else
                    {
                        switcher = gate1;
                        othGate = gate2;
                    }

                    var actualMainXor = gates.gates.Values.OfType<LogicGate>().Where(x => IsMainXor(x, index)).First();
                    SwitchGates(actualMainXor, switcher);

                    thisMainXor = actualMainXor;
                }

                thisMainAnd = othGate;
                gate1 = gates.Get(othGate.Gate1) as LogicGate;
                gate2 = gates.Get(othGate.Gate2) as LogicGate;
                if (IsMainAnd(gate1, index))
                {
                    previousAnd = gate1;
                    othGate = gate2;
                }
                else if (IsMainAnd(gate2, index))
                {
                    previousAnd = gate2;
                    othGate = gate1;
                }
                else
                {
                    throw new Exception("Computer is invalid.");
                }

                if (!ArrayEquals(new[] { othGate.Gate1, othGate.Gate2 }, new[] { previousFullAnd.Name, previousMainXor.Name}))
                {
                    throw new Exception("Computer is invalid.");
                }

                previousFullAnd = thisMainAnd;
                previousMainXor = thisMainXor;
            }

            if (switches.Count != 8)
            {
                throw new Exception("Computer is invalid.");
            }

            return string.Join(",", switches.OrderBy(x => x));

            void SwitchGates(Gate gate1, Gate gate2)
            {
                switches.Add(gate1.Name);
                switches.Add(gate2.Name);

                var gate1Name = gate1.Name;
                var gate2Name = gate2.Name;

                gate1.Name = gate2.Name;
                gate2.Name = gate1Name;

                gates.gates[gate1Name] = gate2;
                gates.gates[gate2Name] = gate1;
            }

            bool IsMainXor(LogicGate gate, int index) => gate != null && gate.GateType == "XOR" && 
                ArrayEquals(new[] { gate.Gate1, gate.Gate2 }, new[] { "x" + PadNumber(index), "y" + PadNumber(index) });

            bool IsMainAnd(LogicGate gate, int index) => gate != null && gate.GateType == "AND" &&
                ArrayEquals(new[] { gate.Gate1, gate.Gate2 }, new[] { "x" + PadNumber(index - 1), "y" + PadNumber(index - 1) });

            string PadNumber(int number) => number < 10 ? "0" + number : number.ToString();

            bool ArrayEquals(string[] a1, string[] a2)
            {
                var a11 = a1.OrderBy(x => x).ToArray();
                var a22 = a2.OrderBy(x => x).ToArray();
                return a11[0] == a22[0] && a11[1] == a22[1];
            }
        }
    }

    public class Gates
    {
        public Dictionary<string, Gate> gates;
        public Gates(IEnumerable<Gate> allGates)
        {
            this.gates = allGates.ToDictionary(x => x.Name, x => x);
        }

        public Gate Get(string name) => name != null && this.gates.ContainsKey(name) ? this.gates[name] : null;

        public int GetGateValue(string gate)
        {
            return this.gates[gate].GetValue(this);
        }

        public string GetGateValues(char letter) => string.Join("", this.gates.Keys.Where(x => x[0] == letter).OrderByDescending(x => int.Parse(x.Substring(1, x.Length - 1))).Select(x => this.gates[x].GetValue(this)));

        public string GetZGateValues() => this.GetGateValues('z');

        public string Stringify()
        {
            var res = "";
            foreach (var z in this.gates.Keys.Where(x => x[0] == 'z').OrderBy(x => int.Parse(x.Substring(1, x.Length - 1))))
            {
                var gate = this.gates[z];
                res += gate.ToString(this) + '\n';
            }

            return res;
        }
    }

    public interface Gate
    {
        string Name { get; set; }

        int GetValue(Gates gates);

        string ToString(Gates gates);
    }

    public class ConstantGate : Gate
    {
        public string Name { get; set; }

        public int Value { get; set; }

        public int GetValue(Gates gates) => this.Value;

        public string ToString(Gates gates) => this.Name;
    }

    public class LogicGate : Gate
    {
        public string Name { get; set; }

        public string Gate1 { get; set; }

        public string Gate2 { get; set; }

        public string GateType { get; set; }

        public int GetValue(Gates gates)
        {
            switch (GateType)
            {
                case "OR": return gates.GetGateValue(this.Gate1) | gates.GetGateValue(this.Gate2);
                case "AND": return gates.GetGateValue(this.Gate1) & gates.GetGateValue(this.Gate2);
                case "XOR": return gates.GetGateValue(this.Gate1) ^ gates.GetGateValue(this.Gate2);
                default: throw new Exception();
            }
        }

        public string ToString(Gates gates)
        {
            return $"<{this.Name}>{gates.Get(Gate1).ToString(gates)} {GateType} {gates.Get(Gate2).ToString(gates)}</{this.Name}>";
        }
    }
}
