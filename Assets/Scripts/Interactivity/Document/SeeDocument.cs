using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeDocument : MonoBehaviour, IInteractable
{
    public Sprite document;
    [Multiline]
    public string comment;
    bool showing = false;
    public void Interact(Player p)
    {
        showing = !showing;
        if (showing)
        {
            DocumentViewer.Show(document);
            p.ui.ShowText(comment);
        }
        else DocumentViewer.Hide();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Player.playerTag)) DocumentViewer.Hide();
        showing = false;
    }
}
