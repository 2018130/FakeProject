using UnityEngine;
using UnityEngine.UI; // Toggle 사용을 위해 필수

public class LightObject : MonoBehaviour
{
    protected Light light;
    public Light Light => light;

    [Header("의존성 Dependencies")]
    public PhoneCommuStatusAndBatter_UI batteryUI;

    // UI 토글 컴포넌트 참조
    private Toggle toggleToLightOnPhone;

    protected virtual void Awake()
    {
        light = GetComponentInChildren<Light>();

        if (batteryUI == null)
        {
            batteryUI = FindObjectOfType<PhoneCommuStatusAndBatter_UI>();
        }

        // batteryUI가 있다면 그 하위에서 Toggle을 찾아 할당합니다.
        if (batteryUI != null)
        {
            toggleToLightOnPhone = batteryUI.GetComponentInChildren<Toggle>();
        }
    }

    protected virtual void OnEnable()
    {
        if (batteryUI != null)
        {
            batteryUI.OnBatteryEmpty += TurnOff;
        }
    }

    protected virtual void OnDisable()
    {
        if (batteryUI != null)
        {
            batteryUI.OnBatteryEmpty -= TurnOff;
        }
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