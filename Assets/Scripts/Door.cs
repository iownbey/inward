using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int requiredLevel = 0;

    Dampener blend;
    public Transform topDoor;
    public Transform bottomDoor;
    public float openOffset;

    public AudioSource open;

    public Notification notification;

    private void Start()
    {
        blend = gameObject.AddComponent<Dampener>().Init(0, 0.5f);
    }

    private void Update()
    {
        Vector2 offset = Vector2.up * blend * openOffset;
        topDoor.localPosition = offset;
        bottomDoor.localPosition = offset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Player.playerTag))
        {
            if (Player.securityLevel >= requiredLevel)
            {
                blend.target = 1;
                open.Play();
            }
            else
            {
                notification.Show("CLEARANCE LEVEL\n\"" + requiredLevel + "\"\nREQUIRED");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Player.playerTag))
        {
            if (blend.target == 1)
            {
                blend.target = 0;
                //close.Play();
            }
            notification.Hide();
        }
    }
}
