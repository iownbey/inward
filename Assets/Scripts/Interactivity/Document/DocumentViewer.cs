using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentViewer : MonoBehaviour
{
    public SpriteRenderer document;
    Transform docT;
    public Transform hidden;
    public Transform visible;

    static DocumentViewer singleton;

    Dampener blend;

    private void Start()
    {
        singleton = this;
        blend = gameObject.AddComponent<Dampener>().Init(0,0.1f);
        docT = document.transform;
    }

    private void Update()
    {
        docT.position = Vector2.Lerp(hidden.position, visible.position, blend);
        docT.localScale = Vector2.Lerp(hidden.localScale, visible.localScale, blend);
        docT.rotation = Quaternion.Slerp(hidden.rotation, visible.rotation, blend);
    }

    public static void Show(Sprite sprite)
    {
        singleton.blend.target = 1;
        singleton.document.sprite = sprite;
    }

    public static void Hide()
    {
        singleton.blend.target = 0;
    }
}
