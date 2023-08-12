using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dampener : MonoBehaviour
{
    public float value;
    public float target;
    public float smoothTime;
    float velocity;

    public Dampener Init(float value, float smoothTime)
    {
        this.value = target = value;
        this.smoothTime = smoothTime;
        return this;
    }
    
    void Update()
    {
        value = Mathf.SmoothDamp(value, target, ref velocity, smoothTime);
    }

    public static implicit operator float(Dampener d) => d.value;
}

public class AngleDampener : MonoBehaviour
{
    public float value;
    public float target;
    public float smoothTime;
    float velocity;

    public AngleDampener Init(float value, float smoothTime)
    {
        this.value = target = value;
        this.smoothTime = smoothTime;
        return this;
    }

    void Update()
    {
        value = Mathf.SmoothDampAngle(value, target, ref velocity, smoothTime);
    }

    public static implicit operator float(AngleDampener d) => d.value;
}

public class DampenedVector2 : MonoBehaviour
{
    public Vector2 value;
    public Vector2 target;
    public float smoothTime;
    public Vector2 velocity;

    public DampenedVector2 Init(Vector2 value, float smoothTime)
    {
        this.value = target = value;
        this.smoothTime = smoothTime;
        return this;
    }

    void Update()
    {
        value = Vector2.SmoothDamp(value, target, ref velocity, smoothTime);
    }

    public static implicit operator Vector2(DampenedVector2 d) => d.value;
}