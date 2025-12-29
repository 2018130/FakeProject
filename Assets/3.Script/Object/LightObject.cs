using UnityEngine;
using UnityEngine.UI; // Toggle 사용을 위해 필수

public class LightObject : MonoBehaviour
{
    protected Light light;
    public Light Light => light;

    [SerializeField]
    protected string lightTurnOnKey = "LightTurnOnKey";

    protected virtual void Awake()
    {
        light = GetComponentInChildren<Light>();
    }

    public virtual void TurnOn()
    {
        light.enabled = true;
        PersistentDataManager.Instance.SaveData(lightTurnOnKey, light.enabled);
        Debug.Log("TurnOn가 저장되었습니다.");
    }

    public virtual void TurnOff()
    {
        light.enabled = false;
        PersistentDataManager.Instance.SaveData(lightTurnOnKey, light.enabled);
        Debug.Log("TurnOff가 저장되었습니다.");
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