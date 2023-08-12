using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightPickup : MonoBehaviour, IInteractable
{
    public AudioClip sound;

    public void Interact(Player p)
    {
        AudioSource.PlayClipAtPoint(sound, transform.position);
        p.flashlight.SetActive(true);
        p.ui.ShowText("This flashlight will illuminate the darkness.\nYou can now explore the darkest parts of the complex\nMy sins hide in the shadows.");
        Destroy(gameObject);
    }
}
