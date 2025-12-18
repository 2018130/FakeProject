using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonAlphaEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Image targetImage;

    // 투명도 설정 (0.0f = 완전 투명, 1.0f = 완전 불투명)
    public float pressedAlpha = 0.3f; // 눌렀을 때 목표 투명도
    public float defaultAlpha = 0.0f; // 평상시 투명도

    void Start()
    {
        targetImage = GetComponent<Image>();

        // 시작하자마자 투명도를 0으로 설정
        SetAlpha(defaultAlpha);
    }

    // 버튼을 눌렀을 때 -> 투명도 0.3
    public void OnPointerDown(PointerEventData eventData)
    {
        SetAlpha(pressedAlpha);
    }

    // 손을 뗐을 때 -> 투명도 0
    public void OnPointerUp(PointerEventData eventData)
    {
        SetAlpha(defaultAlpha);
    }

    // 투명도 변경을 처리하는 함수
    private void SetAlpha(float alpha)
    {
        if (targetImage != null)
        {
            Color color = targetImage.color;
            color.a = alpha;
            targetImage.color = color;
        }
    }
}