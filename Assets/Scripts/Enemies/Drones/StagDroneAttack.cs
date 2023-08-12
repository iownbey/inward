using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagDroneAttack : MonoBehaviour
{
    public float stalkDistance = 4;
    public float resetDistance = 2;
    Transform player;
    Rigidbody2D rb;

    Vector3 stalkingOffset;
    float angle;
    public float maxSpeed;
    public float maxTurn;
    public float strafingSpeed;

    ScrollingNoise strafingNoise;

    public float minPrepDuration = 3f;
    public float maxPrepDuration = 5f;
    float currentPrepDuration;
    float prepTimer;
    public float lungeForce;
    public float endLag = 1;

    bool lunging = false;
    List<GameObject> touching = new List<GameObject>();
    bool Hit { get => touching.Count != 0; }

    public Mandibles mandibles;

    void Start()
    {
        strafingNoise = gameObject.AddComponent<ScrollingNoise>();
        strafingNoise.scrollSpeed = 0.2f;
        rb = GetComponent<Rigidbody2D>();
        currentPrepDuration = prepTimer = Random.Range(minPrepDuration, maxPrepDuration);
        player = Player.player.transform;
        lunging = false;

        mandibles.blend = gameObject.AddComponent<Dampener>().Init(0, 0.05f);
        mandibles.noise = gameObject.AddComponent<ScrollingNoise>();

        StopAllCoroutines();
        StartCoroutine(Logic());
    }

    void OnEnable()
    {
        StartCoroutine(Logic());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator Logic()
    {
        yield return null;
        while (enabled && Player.player != null)
        {
            if ((transform.position - (Player.player.transform.position + stalkingOffset)).sqrMagnitude > resetDistance * resetDistance)
            {
                angle = Vector2.SignedAngle(Vector2.up, transform.position - Player.player.transform.position);
                mandibles.blend.target = 0 + (mandibles.noise.GetSignedValue() * mandibles.noiseAmount);
            }
            else
            {
                float noise = strafingNoise.GetSignedValue();
                noise = Mathf.Sign(noise) * (0.5f + (Mathf.Abs(noise) * 0.5f));
                angle += noise * strafingSpeed * Time.deltaTime;

                prepTimer -= Time.deltaTime;
                mandibles.blend.target = 1 - (prepTimer / currentPrepDuration);
                if (prepTimer <= 0)
                {
                    currentPrepDuration = prepTimer = prepTimer = Random.Range(minPrepDuration, maxPrepDuration);
                    rb.AddForce((player.position - transform.position) * lungeForce, ForceMode2D.Impulse);
                    mandibles.Open();
                    lunging = true;
                    yield return new WaitUntil(() => rb.velocity.sqrMagnitude < 0.01 || Hit);

                    mandibles.blend.target = 0;
                    yield return new WaitForSeconds(endLag);
                    lunging = false;
                }
            }
            stalkingOffset = Quaternion.Euler(0, 0, angle) * (Vector2.up * stalkDistance);
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        touching.Add(collision.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        touching.Remove(collision.gameObject);
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (lunging || Player.player == null) return;
        Vector2 movement = ((player.transform.position + stalkingOffset) - transform.position) * maxSpeed * maxSpeed;
        rb.AddForce(Vector2.ClampMagnitude(movement, maxSpeed));

        float angleDifference = Vector2.SignedAngle(transform.TransformDirection(Vector2.up), player.position - transform.position);
        rb.AddTorque(Mathf.Sign(angleDifference) * Mathf.Min(Mathf.Pow(angleDifference * maxTurn,2),maxTurn));

        //mandibles.Update(transform.up);
    }

    [System.Serializable]
    public class Mandibles
    {
        public Rigidbody2D right;
        public Rigidbody2D left;
        public HingeJoint2D rightJoint;
        public HingeJoint2D leftJoint;
        public float closedRotation;
        public float openRotation;
        public float noiseAmount;
        [HideInInspector]
        public Dampener blend;
        public ScrollingNoise noise;

        public void Open()
        {
            right.AddTorque(5, ForceMode2D.Impulse);
            left.AddTorque(-5, ForceMode2D.Impulse);
        }

        public void Update()
        {
            //float rot = Mathf.Lerp(closedRotation, openRotation, blend);
            //float offset = Vector2.SignedAngle(Vector2.up, forward);
            JointAngleLimits2D limits = rightJoint.limits;
            limits.min = Mathf.Lerp(closedRotation, openRotation, blend);

            limits = leftJoint.limits;
            limits.min = -Mathf.Lerp(closedRotation, openRotation, blend);
        }
    }
}
