using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSizeFitter : MonoBehaviour
{
    [SerializeField]
    private RectTransform targetRect;
    [SerializeField]
    private Text text;

    public void SetSize()
    {
        Vector2 targetSize = targetRect.sizeDelta;
        targetSize.y = text.preferredHeight + 20;
        

        targetRect.sizeDelta = targetSize;
    }
}
