using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebProtocolManager : MonoBehaviour
{
    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private Text text;

    // Gemini API 키 (보안을 위해 실제 프로젝트에서는 다른 방식으로 관리하는 것이 좋습니다)
    private const string API_KEY = "AIzaSyC7ShO1fWlneRO1EyDf79YV6dId7-wXvBk";

    // 사용할 모델과 API 엔드포인트 URL
    private const string API_URL = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key=" + API_KEY;

    // API 요청 데이터를 직렬화할 수 있는 구조체
    [System.Serializable]
    public class GeminiRequest
    {
        public Content[] contents;
    }

    [System.Serializable]
    public class Content
    {
        public Part[] parts;
    }

    [System.Serializable]
    public class Part
    {
        public string text;
    }

    // API 응답 데이터를 역직렬화할 수 있는 구조체 (응답 구조에 따라 복잡해질 수 있음)
    [System.Serializable]
    public class GeminiResponse
    {
        public Candidate[] candidates;
    }

    [System.Serializable]
    public class Candidate
    {
        public Content content;
    }
    
    public void OnSendButtonClicked()
    {
        string prompt = inputField.text;
        SendGeminiRequest(prompt);
    }

    public void SendGeminiRequest(string prompt)
    {
        StartCoroutine(PostRequest(prompt));
    }

    private IEnumerator PostRequest(string prompt)
    {
        // 1. 요청할 데이터 객체 생성
        GeminiRequest requestData = new GeminiRequest
        {
            contents = new Content[]
            {
                new Content
                {
                    parts = new Part[]
                    {
                        new Part { text = prompt }
                    }
                }
            }
        };

        // 2. 객체를 JSON 문자열로 직렬화 (Unity의 JsonUtility 사용)
        string jsonRequestBody = JsonUtility.ToJson(requestData);
        byte[] rawBody = new UTF8Encoding().GetBytes(jsonRequestBody);

        // 3. UnityWebRequest 객체 생성 (POST 메서드)
        using (UnityWebRequest webRequest = new UnityWebRequest(API_URL, "POST"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(rawBody);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            // 4. Content-Type 헤더 설정
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // 5. 요청 전송
            yield return webRequest.SendWebRequest();

            // 6. 응답 처리
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                // 7. JSON 응답을 C# 객체로 역직렬화
                GeminiResponse response = JsonUtility.FromJson<GeminiResponse>(jsonResponse);

                // 예시: 첫 번째 결과에서 텍스트 추출
                if (response?.candidates?.Length > 0)
                {
                    string generatedText = response.candidates[0].content.parts[0].text;
                    Debug.Log("Generated Text: " + generatedText);

                    text.text = generatedText;

                }
            }
        }
    }
}
