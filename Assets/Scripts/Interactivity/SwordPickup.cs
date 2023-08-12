using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPickup : MonoBehaviour, IInteractable
{
    public AudioClip sound;

    public void Interact(Player p)
    {
        AudioSource.PlayClipAtPoint(sound, transform.position);
        p.sword.enabled = true;
        p.ui.ShowText("I can use this mandible to attack them.\nPress the Left Mouse Button to slice.\nMy sins provoke me.");
        Destroy(gameObject);
    }
}
