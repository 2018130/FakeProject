using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObject : MonoBehaviour
{
    protected Light light;
    public Light Light => light;

    protected virtual void Awake()
    {
        light = GetComponentInChildren<Light>();
    }

    public virtual void TurnOn()
    {
        light.enabled = true;
    }
    public virtual void TurnOff()
    {
        light.enabled = false;
    }


    public virtual void Toggle()
    {
        if(light.enabled)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }
}
