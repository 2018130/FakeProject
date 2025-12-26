using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip audioClip;
    [SerializeField]
    private ESFX sfxType;
    [SerializeField]
    private bool playOnStart;
    [SerializeField]
    private EBGM bGM;
    private void Start() //1226 Ãß°¡
    {
        if (playOnStart)
        {
            PlayBGM();
        }
    }
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

    public void PlayBGM()
    {
        SoundManager.Instance.PlayBGM(bGM);
    }
}
