using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonMagnifier : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    LayoutElement layout;
    Dampener blend;
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        blend.target = 1;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        blend.target = 0;
    }

    void Start()
    {
        blend = gameObject.AddComponent<Dampener>().Init(0, 0.1f);
        layout = GetComponent<LayoutElement>();
    }

    void Update()
    {
        layout.flexibleHeight = 1 + blend;
    }
}
