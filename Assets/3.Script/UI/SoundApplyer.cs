using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundApplyer : MonoBehaviour
{
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private Slider bgmSlider;

    private void Start()
    {
        sfxSlider.value = SoundManager.Instance.GetSFXVolume();
        bgmSlider.value = SoundManager.Instance.GetBGMVolume();

        sfxSlider.onValueChanged.AddListener(SoundManager.Instance.SetSFXVolume);
        bgmSlider.onValueChanged.AddListener(SoundManager.Instance.SetBGMVolume);
    }
}
