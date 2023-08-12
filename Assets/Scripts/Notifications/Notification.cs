using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour
{
    public Transform canvasAnchor;
    public float zigZagLength;
    Vector3 anchorPos;
    public RectTransform canvas;
    public RectTransform box;
    public Collider2D lineEdges;
    public LineRenderer line;
    public TMPro.TMP_Text text;

    Dampener blend;
    ScrollingNoise xGlitch;
    ScrollingNoise yGlitch;

    private void Start()
    {
        blend = gameObject.AddComponent<Dampener>().Init(0,0.05f);
        xGlitch = gameObject.AddComponent<ScrollingNoise>();
        xGlitch.scrollSpeed = 50;
        yGlitch = gameObject.AddComponent<ScrollingNoise>();
        yGlitch.scrollSpeed = 50;
        line.useWorldSpace = true;
    }

    private void FixedUpdate()
    {
        anchorPos = canvasAnchor.position;
    }

    private void Update()
    {
        Vector2 pos = transform.position;
        box.position = Vector2.Lerp(pos, anchorPos, blend);
        box.localScale = new Vector3(Mathf.Pow(blend, 0.3333f) + xGlitch * blend * 0.025f, Mathf.Pow(blend, 3) + yGlitch * blend * 0.025f, 0);

        lineEdges.enabled = true;
        Vector2 lineAnchor = lineEdges.ClosestPoint(pos);
        lineEdges.enabled = false;

        Vector2 midpoint = Vector2.Lerp(pos, lineAnchor, 0.5f);
        float zigZagBlend = 0;
        if (transform.position != anchorPos) zigZagBlend = (Mathf.Min(Vector2.Angle(Vector2.up, lineAnchor - pos), Vector2.Angle(Vector2.down, lineAnchor - pos)) / 90);
        line.SetPositions(new Vector3[] 
        { 
            pos,
            midpoint + (Vector2.up * zigZagLength * zigZagBlend),
            midpoint,
            midpoint + (Vector2.down * zigZagLength * zigZagBlend),
            lineAnchor
        });
    }

    public void Show(string content)
    {
        gameObject.SetActive(true);
        text.text = content;
        blend.target = 1;
    }

    public void Hide()
    {
        blend.target = 0;
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
