using System.Collections;
using UnityEngine;

public class FlyingDroneAttack : MonoBehaviour
{
    ScrollingNoise xNoise;
    ScrollingNoise yNoise;

    public float noiseMag = 3;

    Dampener noiseFade;

    DampenedVector2 position;

    AngleDampener rot;

    ScrollingNoise xJitter;
    ScrollingNoise yJitter;

    public float jitterMag;

    Vector2 source;
    Vector2 dest;

    public float attackRadius;

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
    }

    void OnEnable() {
        position.Init(transform.position, 0.5f);
        noiseFade.value = 0;
        noiseFade.target = 1;
        rot.Init(Vector2.SignedAngle(Vector2.up,transform.up),0.1f);
        StartCoroutine(Logic());
    }

    void OnDisable() {
        StopAllCoroutines();
    }

    IEnumerator Logic() {
        yield return new WaitForSeconds(2f);
        source = transform.position;
        position.target = dest = (Vector2)Player.player.transform.position + (Helper.Rotate(Vector2.up, 180f + AngleToPlayer() + (AttackEasing(Random.value) * 360f)) * attackRadius);
        StartCoroutine(Logic());
    }

    void Update()
    {
        Vector2 pos = transform.position;
        Vector2 logPos = position.value + (new Vector2(xNoise,yNoise) * noiseMag * noiseFade);
        transform.position = logPos + (new Vector2(xJitter,yJitter) * jitterMag);

        rot.target = AngleToPlayer();
        transform.localRotation = Quaternion.Euler(0,0,rot);
    }

    void OnCollisionEnter2D() {
        position.target = source;
        position.velocity *= new Vector2(-1,-1);
    }

    float AngleToPlayer() {
        Vector2 delta = Player.player.transform.position - transform.position;
        return Vector2.SignedAngle(Vector2.up,delta);
    }

    float AttackEasing(float input) {
        input -= 0.5f;
        return (4 * (input * input * input)) + 0.5f;
    }
}
