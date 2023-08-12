using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ColorController : MonoBehaviour
{
    SpriteRenderer r;

    [HideInInspector]
    public Color baseColor;
    float lerpValue;
    Color lerpFrom;
    Color lerpTo;
    public Color multiply = Color.white;
    float lerpSpeed;

    void Start()
    {
        r = GetComponent<SpriteRenderer>();
        baseColor = lerpFrom = lerpTo = r.color;
    }

    private void Update()
    {
        lerpValue = Mathf.Lerp(lerpValue, 1, Mathf.Clamp01(lerpSpeed * Time.deltaTime));
        r.color = Color.Lerp(lerpFrom, lerpTo, lerpValue) * multiply;
    }

    public void Flash(Color c, float outSpeed)
    {
        print("Flash");
        lerpFrom = c;
        lerpSpeed = outSpeed;
        lerpValue = 0;
        lerpTo = baseColor;
    }
}
