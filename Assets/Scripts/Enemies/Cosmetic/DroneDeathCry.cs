using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneDeathCry : MonoBehaviour
{
    public AudioSource source;
    public AudioGetter sounds;
    void Start()
    {
        source.PlayOneShot(sounds.Get());
    }
}
