using System.Collections.Generic;
using UnityEngine;

public class Match
{
    private List<Matchable> matchables;
    public List<Matchable> Matchables
    {
        get
        {
            return matchables;
        }
    }
    public int Count
    {
        get
        {
            return matchables.Count;
        }
    }
    public Match()
    {
        matchables = new List<Matchable>(5);
    }
    public Match(Matchable original) : this()
    {
        AddMatchable(original);
    }
    public void Merge(Match toMerge)
    {
        matchables.AddRange(toMerge.matchables);
    }
    public void AddMatchable(Matchable toAdd)
    {
        matchables.Add(toAdd);
    }

}
