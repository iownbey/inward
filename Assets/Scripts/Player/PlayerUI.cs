using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Sprite[] ids;
    public Image idImage;

    public TMPro.TMP_Text attemptIndicator;

    public TMPro.TMP_Text dialogueBox;
    public CanvasGroup dialogueCanvas;
    Dampener dialogueAlpha;

    private void Awake()
    {
        dialogueAlpha = gameObject.AddComponent<Dampener>().Init(0, 0.2f);
    }

    private void Update()
    {
        dialogueCanvas.alpha = dialogueAlpha;
    }

    public void SetId(int id)
    {
        idImage.enabled = id > 0;
        if (id > 0) idImage.sprite = ids[id];
    }

    public void SetAttempt(int attempt)
    {
        attemptIndicator.text = attempt.ToString();
    }

    public void ShowText(string text)
    {
        StopAllCoroutines();
        StartCoroutine(E_ShowText(text));
    }

    IEnumerator E_ShowText(string text)
    {
        dialogueAlpha.target = 1;
        dialogueBox.text = text;
        yield return new WaitForSeconds(3f);
        dialogueAlpha.target = 0;
    }
}
