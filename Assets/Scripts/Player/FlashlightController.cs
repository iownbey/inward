using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public AudioSource sound;
    new public MeshRenderer light;

    void Update()
    {
        if (Player.c.Player.Flashlight.triggered)
        {
            light.enabled = !light.enabled;
            sound.Play();
        }
    }
}
