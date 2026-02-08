using System;
using System.Collections.Generic;
using System.Linq;

class DFAMinimizer
{
    public List<string> States;
    public string StartState;
    public List<string> FinalStates;
    public string[] Alphabet;
    public Dictionary<(string, string), string> Transitions;

    public DFAMinimizer()
    {
        States = new List<string>();
        FinalStates = new List<string>();
        Transitions = new Dictionary<(string, string), string>();
    }

    public void ReadDFA()
    {
        int n = int.Parse(Console.ReadLine()); 
        States = Console.ReadLine().Split().ToList();
        StartState = States[0];

        int z = int.Parse(Console.ReadLine()); 
        Alphabet = Console.ReadLine().Split();

        int m = int.Parse(Console.ReadLine()); 
        FinalStates = Console.ReadLine().Split().ToList();

        int k = int.Parse(Console.ReadLine()); 
        for (int i = 0; i < k; i++)
        {
            var parts = Console.ReadLine().Split(',');
            string from = parts[0];
            string symbol = parts[1];
            string to = parts[2];
            Transitions[(from, symbol)] = to;
        }
    }

    // Minimize DFA
    public void Minimize()
    {
        // Initial partition: final and non-final
        var nonFinal = States.Except(FinalStates).ToList();
        var eqClasses = new List<List<string>>();
        if (nonFinal.Count > 0) eqClasses.Add(nonFinal);
        if (FinalStates.Count > 0) eqClasses.Add(FinalStates);

        bool changed;
        do
        {
            changed = false;
            var newClasses = new List<List<string>>();

            foreach (var cls in eqClasses)
            {
                var tempGroups = new List<List<string>>();

                foreach (var state in cls)
                {
                    bool placed = false;
                    foreach (var grp in tempGroups)
                    {
                        if (grp.Count == 0) continue;

                        bool same = true;
                        foreach (var sym in Alphabet)
                        {
                            Transitions.TryGetValue((state, sym), out var to1);
                            Transitions.TryGetValue((grp[0], sym), out var to2);
                            if (to1 != to2)
                            {
                                same = false;
                                break;
                            }
                        }
                        if (same)
                        {
                            grp.Add(state);
                            placed = true;
                            break;
                        }
                    }
                    if (!placed)
                    {
                        tempGroups.Add(new List<string> { state });
                    }
                }

                newClasses.AddRange(tempGroups);
            }

            if (newClasses.Count != eqClasses.Count)
                changed = true;

            eqClasses = newClasses;

        } while (changed);

        // Map old states to new states
        var stateMapping = new Dictionary<string, string>();
        for (int i = 0; i < eqClasses.Count; i++)
        {
            string newName = $"Q{i}";
            foreach (var s in eqClasses[i])
                stateMapping[s] = newName;
        }

        // Generate minimized DFA transitions
        var minTransitions = new Dictionary<(string, string), string>();
        foreach (var kvp in Transitions)
        {
            string from = stateMapping[kvp.Key.Item1];
            string symbol = kvp.Key.Item2;
            string to = stateMapping[kvp.Value];
            minTransitions[(from, symbol)] = to;
        }

        // Minimized DFA final states
        var minFinals = new HashSet<string>();
        foreach (var f in FinalStates)
            minFinals.Add(stateMapping[f]);

    
        Console.WriteLine("Number of states in minimized DFA: " + eqClasses.Count);
        Console.WriteLine("States: " + string.Join(", ", stateMapping.Values.Distinct()));
        Console.WriteLine("Start state: " + stateMapping[StartState]);
        Console.WriteLine("Final states: " + string.Join(", ", minFinals));
        Console.WriteLine("Transitions:");
        foreach (var t in minTransitions)
        {
            Console.WriteLine($"{t.Key.Item1},{t.Key.Item2},{t.Value}");
        }
    }

    static void Main()
    {
        var dfa = new DFAMinimizer();
        dfa.ReadDFA();
        dfa.Minimize();
    }
}