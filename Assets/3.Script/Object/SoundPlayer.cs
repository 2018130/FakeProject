using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip audioClip;

    public void PlayeAudio()
    {
        SoundManager.Instance.PlaySFX(audioClip);
    }

    public void StopAudio()
    {
        SoundManager.Instance.StopSFX();
    }
}
