using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLight : LightObject
{
    [Header("ÀÇÁ¸¼º Dependencies")]
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

    private void Start()
    {
        FindAnyObjectByType<PlayerInput>().OnLightKeyDowned += Toggle;

        _toggle = FindAnyObjectByType<ScrollConfigWindow>()?.transform.GetComponentInChildren<Toggle>();
        _toggle?.onValueChanged.AddListener(Lighting);
        _batteryUI = FindAnyObjectByType<PhoneCommuStatusAndBatter_UI>();
        _batteryUI.OnBatteryEmpty += TurnOff;
    }


    public void Lighting(bool active)
    {
        light.enabled = !active;
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
        if(_batteryUI.sliderBattery.value > 0)
        {
            base.TurnOn();
            _toggle.isOn = true;
        }
    }
}
