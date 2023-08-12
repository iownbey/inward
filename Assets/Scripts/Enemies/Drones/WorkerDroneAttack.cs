using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerDroneAttack : MonoBehaviour
{
    Transform player;
    Rigidbody2D rb;

    Vector3 stalkingOffset;
    public float maxSpeed;
    public float maxTurn;

    public float maxRushTime;
    public float minRushTime;
    public float maxBreakTime;
    public float minBreakTime;

    public bool waiting;

    List<GameObject> touching = new List<GameObject>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = Player.player.transform;
        StartCoroutine(Logic());
    }

    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator Logic()
    {
        while (enabled)
        {
            waiting = false;
            yield return new WaitForSeconds(Random.Range(minRushTime, maxRushTime));
            waiting = true;
            yield return new WaitForSeconds(Random.Range(minBreakTime, maxBreakTime));
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

    private void FixedUpdate()
    {
        if (waiting || Player.player == null) return;
        Vector2 movement = ((player.transform.position + stalkingOffset) - transform.position) * maxSpeed * maxSpeed;
        rb.AddForce(Vector2.ClampMagnitude(movement, maxSpeed));

        float angleDifference = Vector2.SignedAngle(transform.TransformDirection(Vector2.up), player.position - transform.position);
        rb.AddTorque(Mathf.Sign(angleDifference) * Mathf.Min(Mathf.Pow(angleDifference * maxTurn,2),maxTurn));
    }
}
