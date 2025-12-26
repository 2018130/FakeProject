using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip audioClip;
    [SerializeField]
    private ESFX sfxType;

    public void PlayeAudio()
    {
        SoundManager.Instance.PlaySFX(audioClip);
    }
    public void PlayeAudioByType()
    {
        SoundManager.Instance.PlaySFX(sfxType);
    }
    public void StopAudio()
    {
        SoundManager.Instance.StopSFX();
    }
}
