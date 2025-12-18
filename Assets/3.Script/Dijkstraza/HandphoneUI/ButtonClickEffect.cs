using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // UI 이벤트를 위해 필수

public class ButtonClickEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 originalScale;
    public float pressedScale = 0.9f; // 눌렀을 때 크기 비율

    void Start()
    {
        originalScale = transform.localScale;
    }

    // 버튼을 눌렀을 때
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = originalScale * pressedScale;
    }

    // 버튼에서 손을 뗐을 때
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
}