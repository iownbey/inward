using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : MonoBehaviour
{
    public Transform head;
    public LaserController laser;
    AngleDampener targetRot;

    private void Start()
    {
        targetRot = gameObject.AddComponent<AngleDampener>().Init(0,1f);
    }

    private void Update()
    {
        if (!Player.player) return;
        targetRot.target = Vector2.SignedAngle(Vector2.up, Player.player.transform.position - transform.position);
        head.rotation = Quaternion.Euler(0, 0, targetRot);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Player.playerTag))
        {
            StartCoroutine(laser.StartFiring());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Player.playerTag))
        {
            StartCoroutine(laser.StopFiring());
        }
    }
}
