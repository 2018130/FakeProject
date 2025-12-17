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
        foreach(DialogueData data in CsvReader.LoadCsvData())
        {
            PrintDialogue(data);
        }
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
