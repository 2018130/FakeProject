using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChatController : MonoBehaviour
{
    public GameObject messagePrefab; // 말풍선 프리팹
    public Transform contentTransform; // ScrollView의 Content 오브젝트
    public ScrollRect scrollRect; // 자동 스크롤을 위해 필요


    private void Start()
    {
        AddMessage("testmessage,나야나 마리오");
        Debug.Log("addmessage는 작동함");
        AddMessage("testmessage,루이지는 루이지, 오늘도 굼바를 괴롭히고 있었어. 근데 쿠파가 나타나더니 불덩이를 던지더라");
    }
    public void AddMessage(string text)
    {
        // 1. 메시지 프리팹 생성 (Content를 부모로 설정)
        GameObject newMessage = Instantiate(messagePrefab, contentTransform);

        // 2. 텍스트 내용 변경 (프리팹 내부 구조에 따라 경로 수정 필요)
        // 예: 프리팹의 자식에 Text 컴포넌트가 있다고 가정
        Text messageText = newMessage.GetComponentInChildren<Text>();
        messageText.text = text;

        newMessage.GetComponent<TextSizeFitter>().SetSize();
        //newMessage.transform.localPosition = Vector3.zero;

        float messageHeight = newMessage.GetComponent<RectTransform>().rect.height;
        Vector2 contentRect = contentTransform.GetComponent<RectTransform>().sizeDelta;
        contentRect.y += messageHeight;
        contentTransform.GetComponent<RectTransform>().sizeDelta = contentRect;


        // 3. 레이아웃 갱신 및 스크롤 내리기
        // 유니티 UI는 프레임 단위로 갱신되므로, 즉시 크기를 다시 계산하게 강제해야 함
        if(scrollRect.verticalNormalizedPosition != 0f)
        {
            StartCoroutine(ScrollToBottom());
        }
    }

    IEnumerator ScrollToBottom()
    {
        // 한 프레임 대기 (유니티가 늘어난 길이를 계산할 틈을 줌)
        yield return null;

        // 스크롤바를 맨 아래(0)로 강제 이동
        // 이렇게 하면 시점이 맨 아래로 고정되면서, 
        // 시각적으로는 "새 메시지가 생기면서 이전 메시지를 위로 밀어버린 것"처럼 보임
        // scrollRect.verticalNormalizedPosition = 0f;
        
    }
}
