using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public new HardLight2D light;

    float normalbrightness;

    public float minInterval = 2;
    public float maxInterval = 10;

    public float minFlickerBrightness = 0;
    public float maxFlickerBrightness = 0.1f;

    public float minFlickerLength = 0.05f;
    public float maxFlickerLength = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        normalbrightness = light.Intensity;
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker() {
        light.Intensity = Random.Range(minFlickerBrightness,maxFlickerBrightness);

        yield return new WaitForSeconds(Random.Range(minFlickerLength,maxFlickerLength));

        light.Intensity = normalbrightness;

        yield return new WaitForSeconds(Random.Range(minInterval,maxInterval));

        StartCoroutine(Flicker());
    }
}
