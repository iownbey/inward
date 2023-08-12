using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class RandomGetter<T> where T : Object
{
    public List<T> ts;
    T lastT;

    public T Get()
    {
        List<T> from = ts.Where((a) => (a != lastT)).ToList();
        return lastT = from[Random.Range(0, from.Count)];
    }
}

[System.Serializable]
public class AudioGetter : RandomGetter<AudioClip>
{

}
