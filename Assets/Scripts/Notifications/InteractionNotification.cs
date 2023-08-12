using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionNotification : MonoBehaviour
{
    [SerializeField]
    SmallNotification notification;
    public string verb;
    static Controls c;

    private void Awake()
    {
        c = new Controls();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var message = $"{c.Player.Interact.bindings[0].ToDisplayString()} {verb}";
        if (collision.CompareTag(Player.playerTag)) 
            notification.Show(message);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Player.playerTag)) notification.Hide();
    }
}
