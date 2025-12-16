using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIManager : MonoBehaviour
{
    [SerializeField]
    private Slider favorabilityProgressBar;
    [SerializeField]
    private Image characterImage;
    [SerializeField]
    private DialogueManager dialogueManager;
    public DialogueManager DialogueManager => dialogueManager;

    private void Start()
    {
        DialogueSceneManager.Instance.OnFavorabilityChanged += SetFavorabilityProgressBar;
        DialogueSceneManager.Instance.SetFavorability(DialogueSceneManager.Instance.Favorability);
    }

    private void SetFavorabilityProgressBar(float value, float valueAmount)
    {
        favorabilityProgressBar.value = value / valueAmount;
    }

    public void SetCharacterImage(Sprite sprite)
    {
        characterImage.sprite = sprite;
    }
}