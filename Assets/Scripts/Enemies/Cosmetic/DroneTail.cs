using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DroneTail : MonoBehaviour
{
    public Transform tailPoint;
    Vector2 endTarget;
    Vector2 endVelocity;

    public float interval;
    public int segmentCount;
    public LineRenderer tail;

    Vector2 lastPoint;
    Queue<Vector3> points;

    void Start()
    {
        endTarget = lastPoint = transform.position;
        Vector3[] items = new Vector3[segmentCount];
        for (int i = 0; i < segmentCount; i++) items[i] = lastPoint;
        points = new Queue<Vector3>(items);
        tailPoint.parent = null;

        tail.positionCount = segmentCount + 2;
    }

    void Update()
    {
        Vector2 currentPosition = transform.position;
        if ((currentPosition - lastPoint).sqrMagnitude >= interval)
        {
            points.Enqueue(lastPoint = currentPosition);
            points.Dequeue();
            endTarget = points.Peek();
        }

        Vector2 newPos = Vector2.SmoothDamp(tailPoint.position, endTarget, ref endVelocity, 0.05f);
        tailPoint.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, newPos - (Vector2)tailPoint.position));
        tailPoint.position = newPos;

        List<Vector3> tailPoints = new List<Vector3>();
        tailPoints.Add(tailPoint.position);
        tailPoints.AddRange(points.ToList());
        tailPoints.Add(transform.position);
        tail.SetPositions(tailPoints.ToArray());


    }
}
