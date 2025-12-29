using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickListener : MonoBehaviour
{
    [SerializeField]
    private SoundPlayer soundPlayer;

    private void Awake()
    {
        soundPlayer = GetComponent<SoundPlayer>();
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            soundPlayer.PlayeAudioByType();
        }
    }
}
