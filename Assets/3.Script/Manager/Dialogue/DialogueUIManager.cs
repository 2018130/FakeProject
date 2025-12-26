using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIManager : MonoBehaviour, ISceneContextBuilt
{
    [SerializeField]
    private Slider favorabilityProgressBar;
    [SerializeField]
    private Image characterImage;
    [SerializeField]
    private Image backgroundImage; //1219 배경
    [SerializeField]
    private DialogueManager dialogueManager;
    public DialogueManager DialogueManager => dialogueManager;

    public int Priority { get; set; } = 1;

    //1219 ID리소스 관리
    public List<DialogueVisualData> visualData;



    private void Start()
    {
    }

    private void SetFavorabilityProgressBar(float value, float valueAmount)
    {
        favorabilityProgressBar.value = value / valueAmount;
    }

    public void UpdatevisualbyID(int id) //특정 id 받았을 때 배경과 일러스트를 변경함+1226 배경음 추가
    {
        DialogueVisualData data = visualData.Find(x => x.dialogueID == id);
        Debug.Log(id);
        if (data!=null)
        {
            if (data.characterSprite != null) characterImage.sprite = data.characterSprite;
            if (data.backgroundSprite != null) backgroundImage.sprite = data.backgroundSprite;
            if (data.bgmType != EBGM.None)
            {
                SoundManager.Instance.PlayBGM(data.bgmType);
            }
        }
    }

    public void SetCharacterImage(Sprite sprite)
    {
        characterImage.sprite = sprite;
    }

    public void OnSceneContextBuilt()
    {
        dialogueManager.OnDialogueStarted += UpdatevisualbyID;
        DialogueSceneManager.Instance.OnFavorabilityChanged += SetFavorabilityProgressBar;
        DialogueSceneManager.Instance.SetFavorability(DialogueSceneManager.Instance.Favorability);
    }

    [System.Serializable]
    public class DialogueVisualData
    {
        public int dialogueID;
        public Sprite characterSprite;
        public Sprite backgroundSprite;
        public EBGM bgmType;
    }
}