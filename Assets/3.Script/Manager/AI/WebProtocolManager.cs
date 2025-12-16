using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// === 2. API 요청/응답 직렬화(Serialization) 구조체 ===
#region Request 구조체
[System.Serializable]
public class Part
{
    public string text;
}

[System.Serializable]
public class Content
{
    // 'user' 또는 'model' 역할 지정 (대화 기록 유지에 필수)
    public string role;
    public Part[] parts;
}

[System.Serializable]
public class GeminiRequest
{
    // 전체 대화 기록을 담는 배열
    public Content[] contents;
}

// --- 응답 구조체 (간소화) ---
[System.Serializable]
public class Candidate
{
    public Content content;
}

[System.Serializable]
public class GeminiResponse
{
    public Candidate[] candidates;
}
#endregion

public enum ConversationType
{
    App,
    Romantic,
    RomanticDetector
}

[Serializable]
public class ConversationHistory
{
    public List<Content> appConversationHistory = new List<Content>();
    public List<Content> romanticConversationHistory = new List<Content>();
    public List<Content> RomanticDetectorConversationHistory = new List<Content>();

    public List<Content> GetConversationHistory(ConversationType conversationType)
    {
        switch(conversationType)
        {
            case ConversationType.App:
                return appConversationHistory;
            case ConversationType.Romantic:
                return romanticConversationHistory;
            case ConversationType.RomanticDetector:
                return RomanticDetectorConversationHistory;
        }

        return null;
    }
}

public class WebProtocolManager : MonoBehaviour
{

    [SerializeField] private Text displayText;
    [SerializeField] private InputField inputField;
    [SerializeField] private Button sendButton;

    // === 1. 설정 ===

    // Gemini API 키를 여기에 입력하세요. 
    // 실제 배포 시에는 보안을 위해 Environment Variables 또는 다른 안전한 방식을 사용해야 합니다.
    [SerializeField]
    private string API_KEY = "";

    // 사용할 모델과 API 엔드포인트 URL
    private const string MODEL_NAME = "gemini-2.5-flash";
    private string API_URL => $"https://generativelanguage.googleapis.com/v1beta/models/{MODEL_NAME}:generateContent?key={API_KEY}";

    // 대화 기록을 저장할 리스트
    // 이 리스트가 대화의 '상태'를 유지하는 역할을 합니다.
    private ConversationHistory conversationHistory = new ConversationHistory();

    // === 3. 공용 메서드 ===
    private void Start()
    {
        SetPrompt();
    }

    private void SetPrompt()
    {
        //TextAsset defaultRomanticPrompt = Resources.Load<TextAsset>("DefaultRomanticPrompt");
        //TextAsset romanticDetectorPrompt = Resources.Load<TextAsset>("RomanticDetectorPrompt");
        //SendChatMessage(defaultRomanticPrompt.text, ConversationType.Romantic, null);
        //SendChatMessage(romanticDetectorPrompt.text, ConversationType.RomanticDetector, null);
    }

    public void SendChatMessage()
    {
        SendChatMessage(inputField.text, ConversationType.RomanticDetector, (str) => displayText.text = str);
    }

    // 외부에서 호출할 대화 시작/진행 메서드
    public void SendChatMessage(string text, ConversationType conversationType, Action<string> callback)
    {
        if (string.IsNullOrEmpty(API_KEY) || API_KEY == "YOUR_GEMINI_API_KEY")
        {
            Debug.LogError("API 키를 설정해주세요.");
            return;
        }

        Debug.Log($"User: {text}");

        if(conversationType ==ConversationType.RomanticDetector)
        {
            StartCoroutine(PostRequestForDetector(text, (text) => SendChatMessage(text, ConversationType.Romantic, callback)));
        }
        else
        {
            StartCoroutine(PostRequest(text, conversationType, callback));
        }
    }

    // === 4. 통신 코루틴 ===

    private IEnumerator PostRequest(string newPrompt, ConversationType conversationType, Action<string> callback)
    {
        // 1. 새로운 사용자 메시지를 대화 기록에 추가 (role: user)
        Content newUserContent = new Content
        {
            role = "user",
            parts = new Part[] { new Part { text = newPrompt } }
        };
        conversationHistory.GetConversationHistory(conversationType).Add(newUserContent);

        // 2. 전체 대화 기록을 요청 본문에 담기
        GeminiRequest requestData = new GeminiRequest
        {
            contents = conversationHistory.GetConversationHistory(conversationType).ToArray()
        };

        // 객체를 JSON 문자열로 직렬화
        string jsonRequestBody = JsonUtility.ToJson(requestData);
        byte[] rawBody = new UTF8Encoding().GetBytes(jsonRequestBody);

        // 3. UnityWebRequest 설정 및 전송
        using (UnityWebRequest webRequest = new UnityWebRequest(API_URL, "POST"))
        {
            List<Content> conversation = conversationHistory.GetConversationHistory(conversationType);

            webRequest.uploadHandler = new UploadHandlerRaw(rawBody);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            // 4. 응답 처리
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Gemini API Error: {webRequest.error}\nResponse Text: {webRequest.downloadHandler.text}");

                // 오류 발생 시, 방금 추가한 사용자 메시지를 기록에서 제거하여 재시도를 돕습니다.
                conversation.RemoveAt(conversation.Count - 1);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;

                // JSON 응답을 C# 객체로 역직렬화
                GeminiResponse response = JsonUtility.FromJson<GeminiResponse>(jsonResponse);

                // 5. 모델 응답을 추출하고 기록에 추가 (role: model)
                if (response?.candidates?.Length > 0 && response.candidates[0].content.parts.Length > 0)
                {
                    Content modelContent = response.candidates[0].content;
                    string generatedText = modelContent.parts[0].text;

                    // 모델 응답을 대화 기록에 추가 (role: model)
                    modelContent.role = "model";
                    conversation.Add(modelContent);

                    Debug.Log($"Gemini: {generatedText}");
                    //callback?.Invoke(generatedText);
                }
                else
                {
                    Debug.LogWarning("Gemini 응답에 텍스트가 포함되어 있지 않습니다. 응답 전체: " + jsonResponse);
                    conversation.RemoveAt(conversation.Count - 1);
                }
            }
        }
    }

    private IEnumerator PostRequestForDetector(string newPrompt, Action<string> callback)
    {
        // 1. 새로운 사용자 메시지를 대화 기록에 추가 (role: user)
        Content newUserContent = new Content
        {
            role = "user",
            parts = new Part[] { new Part { text = newPrompt } }
        };
        conversationHistory.RomanticDetectorConversationHistory.Add(newUserContent);

        // 2. 전체 대화 기록을 요청 본문에 담기
        GeminiRequest requestData = new GeminiRequest
        {
            contents = conversationHistory.RomanticDetectorConversationHistory.ToArray()
        };

        // 객체를 JSON 문자열로 직렬화
        string jsonRequestBody = JsonUtility.ToJson(requestData);
        byte[] rawBody = new UTF8Encoding().GetBytes(jsonRequestBody);

        // 3. UnityWebRequest 설정 및 전송
        using (UnityWebRequest webRequest = new UnityWebRequest(API_URL, "POST"))
        {
            List<Content> conversation = conversationHistory.RomanticDetectorConversationHistory;

            webRequest.uploadHandler = new UploadHandlerRaw(rawBody);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            // 4. 응답 처리
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Gemini API Error: {webRequest.error}\nResponse Text: {webRequest.downloadHandler.text}");

                // 오류 발생 시, 방금 추가한 사용자 메시지를 기록에서 제거하여 재시도를 돕습니다.
                conversation.RemoveAt(conversation.Count - 1);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;

                // JSON 응답을 C# 객체로 역직렬화
                GeminiResponse response = JsonUtility.FromJson<GeminiResponse>(jsonResponse);

                // 5. 모델 응답을 추출하고 기록에 추가 (role: model)
                if (response?.candidates?.Length > 0 && response.candidates[0].content.parts.Length > 0)
                {
                    Content modelContent = response.candidates[0].content;
                    string generatedText = modelContent.parts[0].text;

                    // 모델 응답을 대화 기록에 추가 (role: model)
                    modelContent.role = "model";
                    conversation.RemoveAt(conversation.Count - 1);

                    Debug.Log($"Gemini: {generatedText}");

                    if(generatedText != "Reject")
                    {
                        callback?.Invoke(displayText.text);
                    }
                    else
                    {
                        displayText.text = newPrompt + " rejected";
                    }
                }
                else
                {
                    Debug.LogWarning("Gemini 응답에 텍스트가 포함되어 있지 않습니다. 응답 전체: " + jsonResponse);
                    conversation.RemoveAt(conversation.Count - 1);
                }
            }
        }
    }


    // 테스트 및 확인용 메서드 (선택 사항)
    public void ClearChatHistory(ConversationType conversationType)
    {
        conversationHistory.GetConversationHistory(conversationType).Clear();
        Debug.Log("대화 기록이 초기화되었습니다.");
    }
}
