using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class LightCookiePicker : MonoBehaviour
{
    new HardLight2D light;

    public Sprite cookie;

    private void Update()
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            GetComponent<HardLight2D>().Range = 5 * transform.localScale.x;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.x, 1);
        }
#endif

        MeshRenderer r = GetComponent<MeshRenderer>();
        MaterialPropertyBlock block = new MaterialPropertyBlock();

        r.GetPropertyBlock(block);
        if (cookie != null)block.SetTexture("_MainTex", cookie.texture);
        r.SetPropertyBlock(block);
    }
}
