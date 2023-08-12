using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCard : MonoBehaviour, IInteractable
{
    public int clearance = 0;
    public AudioClip sound;

    public void Interact(Player p)
    {
        AudioSource.PlayClipAtPoint(sound, transform.position);
        if (clearance > Player.securityLevel)
        {
            Player.securityLevel = clearance;
            p.ui.SetId(clearance);
            p.ui.ShowText("I can open more doors now.");
        }
        else
        {
            p.ui.ShowText("This card is worse than the one I have.");
        }
        Destroy(gameObject);
    }
}
