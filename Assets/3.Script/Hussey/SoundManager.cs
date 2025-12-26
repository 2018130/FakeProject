using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum EBGM
{
    BGM_T,
    BGM_2D,
    BGM_3D,
    BGM_3Dchase,
}

public enum ESFX
{
    SFX_Playerwalk,
    SFX_PlayerRun,
    SFX_BreathFast,
    SFX_BreathSlowly,
    SFX_CarAlarm,
    SFX_CarAlarm1,
    SFX_CarAlarm2,
    SFX_BirdSound,
    SFX_Rustle1,
    SFX_Rustle2,
    SFX_ItemPick,
    SFX_PickleCry1,
    SFX_PickleCry2,
    SFX_PickleBreath1,
    SFX_PickleBreath2,
    SFX_PickleStep,
    SFX_PickleRun,
    SFX_TurnOnLamp,
    SFX_DropDeadBody,
    SFX_RollingHead,
    SFX_MessageAlarm,
    SFX_Picklescare,
}


/// <summary>
/// 메인 카메라, 피클에게 audioSource 2개씩 넣는게 어떤지
/// 메인카메라 1. BGM, 2. Player SFX 출력
/// 피클은 뛰는소리 울음소리 같이 출력할 때를 대비
/// </summary>



public class SoundManager : SingletonBehaviour<SoundManager>
{
    [SerializeField] private AudioClip[] bgms;
    [SerializeField] private AudioClip[] sfxs;

    [SerializeField] private AudioSource bgmPlayer;
    [SerializeField] private AudioSource sfxPlayer;

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        //SetBGMVolume(bgmSlider.value);
        //SetSFXVolume(sfxSlider.value);
    }

    public void PlayBGM(EBGM bgmIndex)
    {
        bgmPlayer.clip = bgms[(int)bgmIndex];
        bgmPlayer.Play();
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PlaySFX(ESFX sfxIndex)
    {
        sfxPlayer.PlayOneShot(sfxs[(int)sfxIndex]);
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxPlayer.PlayOneShot(clip);
    }
    public void StopSFX()
    {
        sfxPlayer.Stop();
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", MathF.Log10(volume) * 20);
    }
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", MathF.Log10(volume) * 20);
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", MathF.Log10(volume) * 20);
    }
}
