using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLight : LightObject, ISceneContextBuilt
{
    [Header("의존성 Dependencies")]
    public PhoneCommuStatusAndBatter_UI _batteryUI;


    private Toggle _toggle;

    public bool IsTurnOn
    {
        get
        {
            if (_toggle == null)
            {
                return false;
            }

            return !_toggle.isOn;
        }
    }

    public override void OnSceneContextBuilt()
    {
        base.OnSceneContextBuilt();

        FindAnyObjectByType<PlayerInput>().OnLightKeyDowned += Toggle;

        _toggle = FindAnyObjectByType<ScrollConfigWindow>()?.transform.GetComponentInChildren<Toggle>();
        _toggle?.onValueChanged.AddListener(Lighting);
        _batteryUI = FindAnyObjectByType<PhoneCommuStatusAndBatter_UI>();
        _batteryUI.OnBatteryEmpty += TurnOff;

        if (PersistentDataManager.Instance.GetDataWithParsing(lightTurnOnKey, false))
        {
            TurnOn();
        }
    }


    public void Lighting(bool active)
    {
        light.enabled = !active;

        PersistentDataManager.Instance.SaveData(lightTurnOnKey, light.enabled);
        Debug.Log("손전등 상태가 저장되었습니다.");
    }

    public override void Toggle()
    {
        base.Toggle();

        _toggle.isOn = !light.enabled;
    }

    public override void TurnOff()
    {
        base.TurnOff();
        _toggle.isOn = true;
    }

    public override void TurnOn()
    {
        if(_batteryUI?.sliderBattery.value > 0)
        {
            base.TurnOn();
            _toggle.isOn = true;
        }
    }
}
