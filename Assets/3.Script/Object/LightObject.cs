using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObject : MonoBehaviour
{
    protected Light light;

    protected virtual void Awake()
    {
        light = GetComponentInChildren<Light>();
    }

    public void Toggle()
    {
        Debug.Log("2222");
        light.enabled = !light.enabled;
    }
}
