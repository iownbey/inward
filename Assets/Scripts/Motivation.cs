using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motivation
{
    public event System.Action OnMotivated;
    public event System.Action OnUnmotivated;

    readonly HashSet<MonoBehaviour> reasons = new HashSet<MonoBehaviour>();

    public void AddReason(MonoBehaviour proposer)
    {
        bool unique = reasons.Add(proposer);
        if (unique && reasons.Count == 1)
        {
            OnMotivated?.Invoke();
        }
    }

    public void RemoveReason(MonoBehaviour proposer)
    {
        bool removed = reasons.Remove(proposer);
        if (removed && reasons.Count == 0)
        {
            OnUnmotivated?.Invoke();
        }
    }

    public void Reset()
    {
        reasons.Clear();
    }
}
