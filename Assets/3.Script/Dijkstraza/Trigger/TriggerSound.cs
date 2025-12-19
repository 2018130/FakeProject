using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//디텍트 지역을 판정하는 오브젝트에 달아줄 스크립트
public class TriggerSound : MonoBehaviour
{
    [SerializeField]
    private ESFX eSFX;


   public void OnPlaySound()
    {
        SoundManager.Instance.PlaySFX(eSFX);
    }
  
}
