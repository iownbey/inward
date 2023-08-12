using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OutlinedSpriteController : MonoBehaviour
{
    public Color outlineColor = Color.white;
    public float outlineThickness = 0;

    private SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateOutline();
    }

    void Update()
    {
        UpdateOutline();
    }

    void UpdateOutline()
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetColor("_OutlineColor", outlineColor);
        mpb.SetFloat("_OutlineThickness", outlineThickness);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}