using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallNotification : MonoBehaviour
{
    public Transform canvasAnchor;
    Vector3 anchorPos;
    Vector3 rootPos;
    public RectTransform box;
    public TMPro.TMP_Text text;

    Dampener blend;
    ScrollingNoise xGlitch;
    ScrollingNoise yGlitch;

    private void Start()
    {
        rootPos = transform.position;
        blend = gameObject.AddComponent<Dampener>().Init(0,0.05f);
        xGlitch = gameObject.AddComponent<ScrollingNoise>();
        xGlitch.scrollSpeed = 50;
        yGlitch = gameObject.AddComponent<ScrollingNoise>();
        yGlitch.scrollSpeed = 50;
        transform.rotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        anchorPos = canvasAnchor.position;
        transform.position = Vector3.Lerp(rootPos, anchorPos, blend);
    }

    private void Update()
    {
        Vector2 pos = transform.position;
        box.position = Vector2.Lerp(pos, anchorPos, blend);
        box.localScale = new Vector3(Mathf.Pow(blend, 0.3333f) + xGlitch * blend * 0.025f, Mathf.Pow(blend, 3) + yGlitch * blend * 0.025f, 0);
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
