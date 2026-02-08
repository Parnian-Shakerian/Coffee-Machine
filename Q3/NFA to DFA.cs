using System;
using System.Collections.Generic;
using System.Linq;

class NFAtoDFA
{
    static Dictionary<string, Dictionary<string, HashSet<string>>> NFA;
    static Dictionary<string, HashSet<string>> EpsilonClosure;

    // Convert a set of NFA states to a string (DFA state)
    static string StatesToString(HashSet<string> states)
    {
        return string.Join("-", states.OrderBy(s => s));
    }

    // Convert string to set of states
    static HashSet<string> StringToStates(string str)
    {
        return new HashSet<string>(str.Split('-'));
    }

    // Compute full epsilon closure for all NFA states
    static void ComputeEpsilonClosure()
    {
        EpsilonClosure = new Dictionary<string, HashSet<string>>();

        foreach (var state in NFA.Keys)
        {
            var closure = new HashSet<string>();
            var stack = new Stack<string>();
            closure.Add(state);
            stack.Push(state);

            while (stack.Count > 0)
            {
                var s = stack.Pop();
                if (NFA[s].ContainsKey("$")) // $ = epsilon
                {
                    foreach (var t in NFA[s]["$"])
                    {
                        if (closure.Add(t))
                            stack.Push(t);
                    }
                }
            }

            EpsilonClosure[state] = closure;
        }
    }

    // Get next DFA state from a set of NFA states and an input symbol
    static string GetNextDFAState(HashSet<string> nfaStates, string symbol, out bool trap)
    {
        trap = false;
        var reachable = new HashSet<string>();

        foreach (var state in nfaStates)
        {
            foreach (var eState in EpsilonClosure[state])
            {
                if (NFA[eState].TryGetValue(symbol, out var nextStates))
                    reachable.UnionWith(nextStates);
            }
        }

        if (reachable.Count == 0)
        {
            trap = true;
            return null;
        }

        // Add epsilon closure of reachable states
        var fullReachable = new HashSet<string>();
        foreach (var st in reachable)
            fullReachable.UnionWith(EpsilonClosure[st]);

        return StatesToString(fullReachable);
    }

    // Main NFA -> DFA conversion
    static void ConvertNFAtoDFA(string startState, string[] alphabet, HashSet<string> nfaFinalStates)
    {
        var dfaStates = new HashSet<string>();
        var queue = new Queue<string>();
        var dfaTransitions = new Dictionary<(string, string), string>();
        var dfaFinalStates = new HashSet<string>();
        string trapState = "TRAP";

        var startDFAState = StatesToString(EpsilonClosure[startState]);
        dfaStates.Add(startDFAState);
        queue.Enqueue(startDFAState);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var nfaSet = StringToStates(current);

            foreach (var symbol in alphabet)
            {
                string nextState = GetNextDFAState(nfaSet, symbol, out bool isTrap);

                if (isTrap)
                {
                    dfaTransitions[(current, symbol)] = trapState;
                    dfaStates.Add(trapState);
                }
                else
                {
                    dfaTransitions[(current, symbol)] = nextState;
                    if (!dfaStates.Contains(nextState))
                    {
                        dfaStates.Add(nextState);
                        queue.Enqueue(nextState);
                    }
                }
            }

            if (nfaSet.Any(s => nfaFinalStates.Contains(s)))
                dfaFinalStates.Add(current);
        }

        // Add trap state
        if (dfaStates.Contains(trapState))
        {
            foreach (var symbol in alphabet)
                dfaTransitions[(trapState, symbol)] = trapState;
        }

        Console.WriteLine("DFA States: " + string.Join(", ", dfaStates));
        Console.WriteLine("Start State: " + startDFAState);
        Console.WriteLine("Final States: " + string.Join(", ", dfaFinalStates));
        Console.WriteLine("DFA Transitions:");
        foreach (var t in dfaTransitions)
            Console.WriteLine($"{t.Key.Item1},{t.Key.Item2},{t.Value}");
    }

    static void Main()
    {
        int stateCount = int.Parse(Console.ReadLine());
        string[] states = Console.ReadLine().Split();
        int alphabetSize = int.Parse(Console.ReadLine());
        string[] alphabet = Console.ReadLine().Split();
        int finalStateCount = int.Parse(Console.ReadLine());
        HashSet<string> finalStates = new HashSet<string>(Console.ReadLine().Split());
        int transitionCount = int.Parse(Console.ReadLine());

        NFA = new Dictionary<string, Dictionary<string, HashSet<string>>>();
        foreach (var state in states)
            NFA[state] = new Dictionary<string, HashSet<string>>();

        for (int i = 0; i < transitionCount; i++)
        {
            var parts = Console.ReadLine().Split(',');
            if (!NFA[parts[0]].ContainsKey(parts[1]))
                NFA[parts[0]][parts[1]] = new HashSet<string>();
            NFA[parts[0]][parts[1]].Add(parts[2]);
        }

        ComputeEpsilonClosure();
        ConvertNFAtoDFA(states[0], alphabet, finalStates);
    }
}
