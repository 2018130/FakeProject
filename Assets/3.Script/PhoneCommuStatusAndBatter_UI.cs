using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PhoneCommuStatusAndBatter_UI : MonoBehaviour
{
   
    //
    //public enum callStatus
    //{
    //    Disconnected,
    //    Connected
    //}
    //
    //
    //[Header("현재 통신 상태")]
    //[SerializeField]
    //private callStatus currentStatus = callStatus.Disconnected;
    //

    [Header("AutoReference - 통신상태 슬라이더(Slider_Back)를 연결해 주세요.")]
    [SerializeField]
    private Slider sliderCommu;

    [Header("통신상태 = 0.25단위 최대치 1.0")]
    [SerializeField]
    private float statusNum;
    private bool isAbleCommu=false;

    [Space(50)]

    [Header("핸드폰 배터리 슬라이더(Slider_Back)를 연결해 주세요.")]
    [SerializeField]
    private Slider sliderBattery;
    
    [Header("핸드폰 배터리 숫자(Text (Legacy))를 연결해 주세요.")]
    [SerializeField]
    private Text textBattery;

    [Header("AutoReference - Phone 프리팹을 참조하겠습니다.")]
    [SerializeField]
    private GameObject phonePrefab;

    [Header("배터리의 최대최소는 100-0 ")]
    [SerializeField]
    private float batteryStartNum=50f;
    private float batteryMaxNum=100f;
    private float batteryMinNum=0f;

    PlayerLight phoneLight;

    private void Start()
    {
        textBattery.text = ((batteryStartNum * 100) / batteryMaxNum).ToString();
    }

    // 의존성 주입 참조의 통로 - 참조 성공 여부의 결정권이 피참조 대상에게 있음
    public void Initialize(PlayerLight playerLight)
    {
        phoneLight = playerLight;
    }
    

    //
    //public void CommuStatusUpdate()
    //{
    //    switch (currentStatus)
    //    {
    //        case callStatus.Disconnected
    //            : statusNum = 0f;
    //            break;
    //        case callStatus.Connected
    //            : statusNum = 0.5f;
    //            break;
    //    }
    //
    //    slider.value = statusNum;
    //}
    //
    public void OnOffSwitchPhoneCommuStatus()
    {
        if (isAbleCommu == false)
        {

            sliderCommu.value = 0.5f;
            isAbleCommu = true;
        }
        else if(isAbleCommu == true)
        {
            sliderCommu.value = 0f;
            isAbleCommu = false;
        }

    }
    public void Update()
    {

        if (phoneLight != null && phoneLight.IsTurnOn == true)
        {
            sliderBattery.value -= 1f*Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameTimeScale;
            textBattery.text = sliderBattery.value.ToString("F0");
        }

    }
    
    


}
