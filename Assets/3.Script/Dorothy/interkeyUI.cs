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

    private void Awake()
    {
        /*
         
         .¡ü__,¡ü
         (.¡¡¡¡¡¡£©
         (¡¡¡¡n¡¡£©
         .£àu¡ªu¢¥


         */
    }

    public void SetInteractionKeyUI(UIData data)
    {
        keyText.text = data.UIname;
        descriptionText.text = data.UImsg;
        gameObject.SetActive(true);
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }
}
