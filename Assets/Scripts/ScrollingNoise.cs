using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingNoise : MonoBehaviour
{
    public float scrollSpeed = 1;
    public float Value { get => Mathf.Clamp01(Mathf.PerlinNoise(point.x, point.y)); }
    Vector2 point;
    Vector2 speedDelta;

    void Start()
    {
        speedDelta = Helper.RandomPointOnUnitCircle();
        point = speedDelta * Random.Range(-10f,10f);
    }

    void Update()
    {
        point += speedDelta * Time.deltaTime * scrollSpeed;
    }

    public float GetSignedValue()
    {
        return (Value * 2) - 1;
    }

    public static implicit operator float(ScrollingNoise s)
    {
        return s.Value;
    }
}
