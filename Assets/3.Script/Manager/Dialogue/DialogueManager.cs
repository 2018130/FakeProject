using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DialogueData
{
    public int ID;
    public string Name;
    public string Dialogue;
    public bool HasChoice;
    public string AcceptDialogue;
    public int AcceptID;
    public string RejectDialogue;
    public int RejectID;
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
    [SerializeField]
    private List<DialogueData> dialogueDatas;
    // 다이얼로그 데이터 값
    private Queue<DialogueData> dialogueQueue = new Queue<DialogueData>();
    private bool isPrintAnyDialogue = false;

    public event Action<int> OnDialogueStarted;
    [SerializeField]
    private int _nextPrintDialogueID = 1;

    private void Start()
    {
        dialogueDatas = CsvReader.LoadCsvData();
        PrintDialogue(_nextPrintDialogueID);
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
    public void PrintDialogue(int id)
    {
        DialogueData data = dialogueDatas.Find(x => x.ID == id);

        if (!isPrintAnyDialogue)
        {
            PrintDialogue(data);
        }
    }

    private IEnumerator PrintDialogue_co()
    {
        if (dialogueQueue.Count > 0)
        {
            isPrintAnyDialogue = true;

            bool isClickedAnyKey = false;
            DialogueData currentDialogue = dialogueQueue.Dequeue();
            OnDialogueStarted?.Invoke(currentDialogue.ID);
            dialogueText.text = "";
            nameText.text = currentDialogue.Name;
            Debug.Log($"Print dialogue, Current dialogue data : {currentDialogue.Name}");

            SetButtonActive(currentDialogue,
                () =>
                {
                    DialogueSceneManager.Instance.SetFavorability(DialogueSceneManager.Instance.Favorability + currentDialogue.Favorability);
                    PrintDialogue(currentDialogue.AcceptID);
                    _nextPrintDialogueID = currentDialogue.AcceptID;
                    isClickedAnyKey = true;
                },
                () =>
                {
                    DialogueSceneManager.Instance.SetFavorability(DialogueSceneManager.Instance.Favorability - currentDialogue.Favorability);
                    PrintDialogue(currentDialogue.RejectID);
                    _nextPrintDialogueID = currentDialogue.RejectID;
                    isClickedAnyKey = true;
                });
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
            yield return new WaitUntil(() =>
            {
                if (!currentDialogue.HasChoice)
                {
                    isClickedAnyKey = Input.anyKeyDown;
                }

                return isClickedAnyKey;
            });

            isPrintAnyDialogue = false;

            if (!currentDialogue.HasChoice)
            {
                Debug.Log("1111");
                _nextPrintDialogueID++;
            }

            PrintDialogue(_nextPrintDialogueID);
        }
    }

    private void SetButtonActive(DialogueData currentDialogue, UnityEngine.Events.UnityAction acceptChoiceCallback = null, UnityEngine.Events.UnityAction rejectChoideCallback = null)
    {
        if (currentDialogue.HasChoice)
        {
            chooseButton[0].gameObject.SetActive(true);
            chooseButton[1].gameObject.SetActive(true);

            chooseButton[0].GetComponentInChildren<Text>().text = currentDialogue.AcceptDialogue;
            chooseButton[1].GetComponentInChildren<Text>().text = currentDialogue.RejectDialogue;

            chooseButton[0].onClick.RemoveAllListeners();
            chooseButton[1].onClick.RemoveAllListeners();

            chooseButton[0].onClick.AddListener(acceptChoiceCallback);
            chooseButton[1].onClick.AddListener(rejectChoideCallback);
        }
        else
        {
            chooseButton[0].gameObject.SetActive(false);
            chooseButton[1].gameObject.SetActive(false);
        }
    }
}
