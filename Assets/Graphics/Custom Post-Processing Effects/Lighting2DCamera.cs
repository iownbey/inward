using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting2DCamera : MonoBehaviour
{
    [SerializeField]
    RenderTexture rt;
    private void Awake()
    {
        rt.width = Screen.width;
        rt.height = Screen.height;
    }
}
