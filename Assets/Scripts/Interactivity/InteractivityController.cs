using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractivityController : MonoBehaviour
{
    OutlinedSpriteController outline;
    Dampener blend;

    readonly static Color highlight = new Color32(255, 0, 217, 255);
    readonly static Color invisible = new Color32(255, 0, 217, 0);

    private void Start()
    {
        blend = gameObject.AddComponent<Dampener>().Init(0, 0.5f);
        outline = GetComponent<OutlinedSpriteController>();
    }

    private void Update()
    {
        outline.outlineColor = Color.Lerp(invisible, highlight, blend);
        outline.outlineThickness = blend * 10;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Player.playerTag))
        {
            blend.target = 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Player.playerTag))
        {
            blend.target = 0;
        }
    }
}
