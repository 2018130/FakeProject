using UnityEngine;
using UnityEngine.UI; // Toggle 사용을 위해 필수

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
        if (light.enabled)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }
}