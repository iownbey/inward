using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneWing : MonoBehaviour
{
    public float rotAmount = 5f;
    public float wingTime = 0.1f;

    public float diveAmount = 20;

    IEnumerator c_flutter;

    AngleDampener osc;

    Quaternion localRot;

    void Awake()
    {
        osc = gameObject.AddComponent<AngleDampener>().Init(0,0.01f);
    }

    void Start()
    {
        localRot = transform.localRotation;
        Flutter();
    }

    void Update()
    {
        transform.localRotation = localRot * Quaternion.Euler(0,0,osc);
    }

    IEnumerator C_Flutter() {
        osc.target = rotAmount;
        yield return new WaitForSeconds(wingTime);
        osc.target = -rotAmount;
        yield return new WaitForSeconds(wingTime);
        StartCoroutine(c_flutter = C_Flutter());
    }

    public void Dive() {
        StopCoroutine(c_flutter);
        osc.target = diveAmount;
    }

    public void Flutter() {
        StartCoroutine(c_flutter = C_Flutter());
    }
}
