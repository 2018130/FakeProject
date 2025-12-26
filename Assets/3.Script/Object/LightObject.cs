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
        if (batteryUI != null && batteryUI.sliderBattery.value > 0)
        {
            light.enabled = true;
            // 만약 물리적으로 켜졌다면 UI 토글도 일치시켜줍니다.
            if (toggleToLightOnPhone != null) toggleToLightOnPhone.isOn = true;
        }
        else
        {
            light.enabled = false;
            // 배터리가 없으면 토글도 강제로 끔 상태로 둡니다.
            if (toggleToLightOnPhone != null) toggleToLightOnPhone.isOn = false;
            Debug.Log("배터리가 없어 불을 켤 수 없습니다.");
        }

        PersistentDataManager.Instance.SaveData(lightTurnOnKey, light.enabled);
    }

    public virtual void TurnOff()
    {
        light.enabled = false;

        // [추가] 배터리가 0이 되어 호출되거나 직접 꺼질 때 UI 토글을 OFF로 변경
        if (toggleToLightOnPhone != null)
        {
            toggleToLightOnPhone.isOn = true;
            Debug.Log("turnoff??");
        }

        PersistentDataManager.Instance.SaveData(lightTurnOnKey, light.enabled);
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