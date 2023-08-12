using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer: MonoBehaviour
{
    public event System.Action OnFinish;

    public float time = 0;

    public bool IsFinished { get => time == 0; }

    public void Update()
    {
        if (time == 0) return;
        time = Mathf.Max(time - Time.deltaTime, 0);
        if (time == 0) OnFinish?.Invoke();
    }

    public void Set(float time) { this.time = time; }
}
