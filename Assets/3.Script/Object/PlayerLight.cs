using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLight : LightObject
{
    private Toggle _toggle;
    public bool IsTurnOn
    {
        get
        {
            if(_toggle == null)
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
}
