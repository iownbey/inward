using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : MonoBehaviour, IInteractable
{
    public AudioClip sound;

    public void Interact(Player p)
    {
        AudioSource.PlayClipAtPoint(sound, transform.position);
        p.shield.enabled = true;
        p.ui.ShowText("I can use their carapace to defend myself.\nPress the Right Mouse Button to guard.\nMy sins harden me.");
        Destroy(gameObject);
    }
}
