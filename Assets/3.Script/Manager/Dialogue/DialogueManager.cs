using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DialogueData
{
    public string Name;
    public string Dialogue;
    public bool HasChoice;

    public int Favorability;
}

public class DialogueManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text dialogueText;
    [SerializeField]
    private Button[] chooseButton;

    [Space(10f)]

    [Header("Word Setting")]
    [SerializeField]
    private float wordPrintSpeed = 0.1f;
    [SerializeField]
    private int wordPrintCountPerCycle = 1;

    [Space(10f)]

    [Header("Dialogue")]

    // 다이얼로그 데이터 값
    private Queue<DialogueData> dialogueQueue = new Queue<DialogueData>();
    private bool isPrintAnyDialogue = false;

    private void Start()
    {
        // test
        PrintDialogue(new DialogueData() { Name = "지수", Dialogue = "민준아, 다음 주 토요일에 한강공원으로 소풍 가는 거 잊지 않았지? 내가 어제 날씨를 확인해 보니까 딱 맑을 것 같아.", HasChoice = false });
        PrintDialogue(new DialogueData() { Name = "민준", Dialogue = "물론이지, 지수야! 완전 기대하고 있어. 너 도시락은 뭘 싸 올 거야? 나는 벌써부터 네가 만든 유부초밥이 아른거려. 🤤", HasChoice = false });
        PrintDialogue(new DialogueData() { Name = "지수", Dialogue = "하하, 네가 제일 좋아하는 유부초밥이랑 샌드위치도 조금 싸 갈 거야. 그리고 과일이랑 시원한 보리차도 챙기려고. 너는 혹시 특별히 먹고 싶은 거 없어?", HasChoice = false });
        PrintDialogue(new DialogueData() { Name = "민준", Dialogue = "나는 치킨 한 마리 포장해 갈까 생각 중이었는데! 그리고 탄산음료도 좀 있어야 완벽할 것 같아. 역시 소풍의 하이라이트는 맛있는 음식 아니겠어?", HasChoice = true, Favorability = 20 });
    }

    public void PrintDialogue(DialogueData dialogueData)
    {
        if (dialogueData != null)
        {
            dialogueQueue.Enqueue(dialogueData);
        }

        if (!isPrintAnyDialogue)
        {
            StartCoroutine(PrintDialogue_co());
        }
    }

    private IEnumerator PrintDialogue_co()
    {
        if(dialogueQueue.Count > 0)
        {
            isPrintAnyDialogue = true;

            DialogueData currentDialogue = dialogueQueue.Dequeue();
            dialogueText.text = "";
            nameText.text = currentDialogue.Name;
            Debug.Log($"Print dialogue, Current dialogue data : {currentDialogue.Name}");

            SetButtonActive(currentDialogue.HasChoice, 
                () => DialogueSceneManager.Instance.SetFavorability(DialogueSceneManager.Instance.Favorability + currentDialogue.Favorability),
                () => DialogueSceneManager.Instance.SetFavorability(DialogueSceneManager.Instance.Favorability - currentDialogue.Favorability));

            int index = 0;
            while (index < currentDialogue.Dialogue.Length)
            {
                string strPerCycle = "";
                for (int i = 0; i < wordPrintCountPerCycle; i++)
                {
                    if (index + i >= currentDialogue.Dialogue.Length)
                    {
                        break;
                    }
                    strPerCycle += currentDialogue.Dialogue[index + i];
                }

                dialogueText.text += strPerCycle;

                yield return new WaitForSeconds(1 / wordPrintSpeed);

                index += wordPrintCountPerCycle;
            }

            // 옵저버 패턴
            bool isClickedAnyKey = false;
            yield return new WaitUntil(() => {
                isClickedAnyKey = Input.anyKeyDown;

                return isClickedAnyKey;
            });

            isPrintAnyDialogue = false;

            PrintDialogue(null);
        }
    }

    private void SetButtonActive(bool active, UnityEngine.Events.UnityAction acceptChoiceCallback = null, UnityEngine.Events.UnityAction rejectChoideCallback = null)
    {
        foreach (var choiceBtn in chooseButton)
        {
            choiceBtn.gameObject.SetActive(active);
        }

        if(active)
        {
            chooseButton[0].onClick.RemoveAllListeners();
            chooseButton[1].onClick.RemoveAllListeners();

            chooseButton[0].onClick.AddListener(acceptChoiceCallback);
            chooseButton[1].onClick.AddListener(rejectChoideCallback);
        }
    }
}
