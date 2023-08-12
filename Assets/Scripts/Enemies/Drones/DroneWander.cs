using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneWander : MonoBehaviour
{
    public float maxSpeed;
    public float maxTurn;
    public float avoidanceTurn;

    float speed;
    float turn;

    float avoidanceCoefficient;

    Rigidbody2D rb;
    ScrollingNoise speedNoise;
    ScrollingNoise turnNoise;

    Timer stuckTimer;
    Vector2 stuckPos;
    const float stuckTime = 2f;
    const float unstuckSqrDistance = 0.5f;

    public float unstuckForce;

    // Start is called before the first frame update
    void Awake()
    {
        speedNoise = gameObject.AddComponent<ScrollingNoise>();
        turnNoise = gameObject.AddComponent<ScrollingNoise>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        stuckTimer = gameObject.AddComponent<Timer>();
        stuckTimer.OnFinish += () => {
            stuckTimer.time = stuckTime;
            rb.AddRelativeForce(Vector2.down * unstuckForce, ForceMode2D.Impulse);
        };
        stuckTimer.time = stuckTime;
    }

    private void OnEnable() {
        stuckTimer.time = stuckTime;
        stuckTimer.enabled = true;
    }

    private void OnDisable() {
        stuckTimer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D check = Physics2D.OverlapPoint(transform.up * 0.6f);
        avoidanceCoefficient = (check != null && !check.CompareTag(Player.playerTag)) ? avoidanceTurn : 1;

        speed = speedNoise * maxSpeed;
        turn = turnNoise.GetSignedValue() * maxTurn * avoidanceTurn;

        if (((Vector2)transform.position - stuckPos).sqrMagnitude > unstuckSqrDistance)
        {
            stuckTimer.time = stuckTime;
            stuckPos = transform.position;
        }
    }

    private void FixedUpdate()
    {
        rb.AddTorque(turn);
        rb.AddRelativeForce(Vector2.up * speed);
    }
}
