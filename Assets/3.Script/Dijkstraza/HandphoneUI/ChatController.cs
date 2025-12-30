using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject messagePrefab;       // 말풍선 프리팹
    public Transform contentTransform;     // ScrollView의 Content
    public ScrollRect scrollRect;          // 자동 스크롤용

    [Header("Notification Settings")]
    // [연결 1] 알림 배지 오브젝트 (Image_NewMessageArrive)
    public GameObject notificationBadge;

    // [연결 2] 배지 안의 숫자 텍스트 (Text_NewMessageNumber)
    public Text messageCountText;

    private int currentMessageCount = 0;   // 내부 카운트 변수

    private CellPhone cellPhone;

    private void Start()
    {
        // 시작할 때 상태 업데이트 (0이면 숨김)
        UpdateBadgeState();
        cellPhone = FindAnyObjectByType<CellPhone>();
    }

    // 메시지 추가 함수
    public void AddMessage(string text)
    {
        SoundManager.Instance.PlaySFX(ESFX.SFX_MessageAlarm);
        cellPhone?.TurnOnScreenForSeconds(0.5f);

        // 1. 메시지 생성
        GameObject newMessage = Instantiate(messagePrefab, contentTransform);
        Text messageText = newMessage.GetComponentInChildren<Text>();
        messageText.text = text;
        newMessage.GetComponent<TextSizeFitter>().SetSize();

        // 높이 계산 및 컨텐츠 크기 조절
        float messageHeight = newMessage.GetComponent<RectTransform>().rect.height;
        Vector2 contentRect = contentTransform.GetComponent<RectTransform>().sizeDelta;
        contentRect.y += messageHeight;
        contentTransform.GetComponent<RectTransform>().sizeDelta = contentRect;

        // 2. 카운트 증가 및 배지 상태 갱신
        currentMessageCount++;
        UpdateBadgeState();

        // 3. 스크롤 내리기
        if (scrollRect.verticalNormalizedPosition != 0f)
        {
            StartCoroutine(ScrollToBottom());
        }
    }

    // [중요] 버튼 클릭 시 호출할 함수 (Button_SendMessage의 OnClick에 연결)
    public void ResetMessageCount()
    {
        currentMessageCount = 0;
        UpdateBadgeState();
        Debug.Log("메시지 카운트 초기화됨");
    }

    // 배지 보임/숨김 및 텍스트 갱신을 처리하는 내부 함수
    private void UpdateBadgeState()
    {
        if (notificationBadge != null)
        {
            if (currentMessageCount > 0)
            {
                // 카운트가 1 이상이면 활성화하고 숫자 갱신
                notificationBadge.SetActive(true);

                if (messageCountText != null)
                    messageCountText.text = currentMessageCount.ToString();
            }
            else
            {
                // 카운트가 0이면 비활성화 (안 보이게 함)
                notificationBadge.SetActive(false);
            }
        }
    }

    IEnumerator ScrollToBottom()
    {
        yield return null;
        // scrollRect.verticalNormalizedPosition = 0f;
    }
}