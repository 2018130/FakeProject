using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class interkeyUI : MonoBehaviour
{
    [SerializeField]
    private Text keyText;
    [SerializeField]
    private Text descriptionText;

    private void Start()
    {
        SetActive(false);
    }

    public void SetActive(bool active)
    {
        keyText.gameObject.SetActive(active);
        descriptionText.gameObject.SetActive(active);
    }

    public void SetInteractionKeyUI(UIData data)
    {
        if(data != null)
        {
            keyText.text = data.UIname;
            descriptionText.text = data.UImsg;
            SetActive(true);
        }
    }
}
