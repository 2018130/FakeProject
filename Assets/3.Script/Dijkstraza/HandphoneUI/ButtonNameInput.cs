using UnityEngine;
using UnityEngine.UI; // Text 컴포넌트 사용을 위해 필수

public class ButtonNameInput : MonoBehaviour
{
    [Header("숫자가 표시될 텍스트 (Legacy)")]
    public Text screenText; // InputField 대신 Text를 사용합니다.

    [Header("숫자 버튼들")]
    public Button[] digitButtons; // 1~9, *, 0, # 버튼들을 담을 배열


    private bool _isInputNum = false;
    void Start()
    {
        // 1. 화면 텍스트 초기화 (선택사항: 시작할 때 비워두기)
        screenText.text = "";

        // 2. 버튼 이벤트 연결
        foreach (Button btn in digitButtons)
        {
            btn.onClick.AddListener(() => OnDigitClick(btn));
        }
    }

    // 버튼 클릭 시 실행
    void OnDigitClick(Button clickedButton)
    {
        _isInputNum = true;
        // 버튼 오브젝트의 이름 가져오기 (예: "1 Button (Legacy)")
        string buttonName = clickedButton.gameObject.name;

        // 이름의 첫 글자만 가져와서 텍스트에 추가
        if (buttonName.Length > 0)
        {
            // "1 Button..." -> "1"만 추출
            string number = buttonName.Substring(0, 1);

            // 기존 텍스트 뒤에 이어 붙이기
            screenText.text += number;
        }
    }

    // (선택 기능) 텍스트 지우기 함수 - 필요하면 버튼을 만들어 연결하세요
    public void OnClearClick()
    {
        if (_isInputNum)
        {

        screenText.text = "";
        }
        _isInputNum = false;
    }
}