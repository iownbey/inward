using UnityEngine;
using MathNet.Numerics.Distributions;

public class FlyingDroneWander : MonoBehaviour
{
    public Transform positionalAnchor;
    public float radiusMultiplier = 5;

    ScrollingNoise xNoise;
    ScrollingNoise yNoise;

    public float noiseMag = 3;

    Dampener noiseFade;

    DampenedVector2 position;

    AngleDampener rot;

    Timer attentionSpan;

    public float minAttentionSpan;
    public float maxAttentionSpan;
    public float maxDistance;

    ScrollingNoise xJitter;
    ScrollingNoise yJitter;

    public float jitterMag;

    void Awake() {
        xNoise = gameObject.AddComponent<ScrollingNoise>();
        yNoise = gameObject.AddComponent<ScrollingNoise>();
        xNoise.scrollSpeed = yNoise.scrollSpeed = 0.5f;

        xJitter = gameObject.AddComponent<ScrollingNoise>();
        yJitter = gameObject.AddComponent<ScrollingNoise>();
        xJitter.scrollSpeed = yJitter.scrollSpeed = 3f;

        noiseFade = gameObject.AddComponent<Dampener>();
        noiseFade.smoothTime = 3f;

        position = gameObject.AddComponent<DampenedVector2>();
        rot = gameObject.AddComponent<AngleDampener>();

        attentionSpan = gameObject.AddComponent<Timer>();

        attentionSpan.OnFinish += SwitchAttention;
        SwitchAttention();
    }

    void OnEnable() {
        position.Init(transform.position, 2f);
        noiseFade.value = 0;
        noiseFade.target = 1;
        rot.Init(Vector2.SignedAngle(Vector2.up,transform.up),0.1f);
    }

    void SwitchAttention() {
        var distribution = new Normal();
        Vector2 newTarget = Gizmo_Target = (Vector2)positionalAnchor.position + (new Vector2((float)distribution.InverseCumulativeDistribution(Random.value), (float)distribution.InverseCumulativeDistribution(Random.value)).normalized * radiusMultiplier);
        attentionSpan.time = Random.Range(minAttentionSpan, maxAttentionSpan);
        Vector2 dir = (newTarget - (Vector2)transform.position).normalized;
        float distance = Random.Range(5, maxDistance);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distance);
        if (hit) {
            position.target = hit.point;
        }
        else {
            position.target += dir.normalized * distance;
        }
    }

    void Update()
    {
        Vector2 pos = position.value + (new Vector2(xNoise,yNoise) * noiseFade * noiseMag);

        Vector2 delta = pos - (Vector2)transform.position;
        rot.target = Vector2.SignedAngle(Vector2.up,delta);
        transform.localRotation = Quaternion.Euler(0,0, rot);
        transform.position = pos + (new Vector2(xJitter,yJitter) * jitterMag);
    }

    Vector2 Gizmo_Target;
    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(Gizmo_Target, 0.01f);
        Gizmos.DrawLine(Gizmo_Target, transform.position);
    }
}
