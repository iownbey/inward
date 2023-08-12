using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleWave : MonoBehaviour
{
    public Transform rootBone;

    public float boneOffsetInWavelengths = 0.07f;

    public float amplitudeDegrees = 15f;

    public float wavelengthInSeconds = 1;

    Transform[] bones;

    // Start is called before the first frame update
    void Start()
    {
        bones = rootBone.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.time;
        foreach (Transform bone in bones)
        {
            float degrees = amplitudeDegrees * Mathf.Sin(time * (2 * Mathf.PI / wavelengthInSeconds));
            bone.localRotation = Quaternion.Euler(0,0, degrees);
            time += wavelengthInSeconds * boneOffsetInWavelengths;
        }
    }
}
