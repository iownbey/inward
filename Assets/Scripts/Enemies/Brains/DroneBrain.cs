using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBrain : MonoBehaviour, IDamageable
{
    public float attentionSpan = 5;
    public float packDistance = 5;

    public Transform eye;
    public float visionDistance = 10;
    public float fov = 30;
    public float eyeRange = 10;
    Dampener eyeRot;
    public LayerMask sightFilter;

    Timer attention;

    public MonoBehaviour passiveBehaviour;
    public MonoBehaviour aggresiveBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        attention = gameObject.AddComponent<Timer>();
        eyeRot = gameObject.AddComponent<Dampener>().Init(0, 0.1f);
        BecomePassive();
        attention.OnFinish += BecomePassive;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.player == null) return;

        Vector2 ray = Player.player.transform.position - transform.position;
        float angle = Vector2.SignedAngle(eye.up, ray);
        float halfFOV = fov / 2;
        if (Mathf.Abs(angle) <= halfFOV)
        {
            //print("Player could be in view");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, ray, visionDistance, sightFilter);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                BroadcastAggression();
            }
        }
        if (aggresiveBehaviour.enabled) eyeRot.target = Mathf.Clamp(angle, -eyeRange, eyeRange);
        eye.localRotation = Quaternion.Euler(0, 0, eyeRot);
    }

    public void BecomePassive()
    {
        passiveBehaviour.enabled = true;
        aggresiveBehaviour.enabled = false;
        eyeRot.target = 0;

        BackgroundMusicController.battleMotivation.RemoveReason(this);
    }

    public void BroadcastAggression()
    {
        BecomeAggresive();

        DroneBrain check;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, packDistance);
        foreach (Collider2D c in hits)
        {
            check = c.GetComponent<DroneBrain>();
            check?.BecomeAggresive();
        }
    }

    public void BecomeAggresive()
    {
        BackgroundMusicController.battleMotivation.AddReason(this);

        passiveBehaviour.enabled = false;
        aggresiveBehaviour.enabled = true;
        attention.Set(attentionSpan);
    }

    public void Damage(float damage, Vector2 force)
    {
        BroadcastAggression();
    }

    private void OnDestroy()
    {
        BackgroundMusicController.battleMotivation.RemoveReason(this);
    }
}
