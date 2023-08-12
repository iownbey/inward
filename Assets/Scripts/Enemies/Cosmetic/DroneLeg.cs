using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneLeg : MonoBehaviour
{
    new public LineRenderer renderer;
    public Transform stepPoint;
    public float maxLength = 1;
    public float minLength = 0.1f;
    Transform end;

    Vector2 velocity;
    Vector2 target;
    void Start()
    {
        end = new GameObject("Foot").transform;
        end.position = stepPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        float sqrMag = (end.position - transform.position).sqrMagnitude;
        if (sqrMag > maxLength*maxLength || sqrMag < minLength * minLength)
        {
            target = stepPoint.position;
        }

        end.position = Vector2.SmoothDamp(end.position, target, ref velocity, 0.05f);
        renderer.SetPosition(1, transform.InverseTransformPoint(end.position));
    }
}
