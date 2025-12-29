using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class Shoes : MonoBehaviour, IInteractable
{
    //msg
    [SerializeField]
    private StringDataSO messageData;
    private ChatController chatController;
    [SerializeField]
    private GameObject targetModel;

    private bool sendMSGAlready = false;
    PlayerInput playerInput;

    //
    [SerializeField]
    private UIData uIData;
    private PlayerInput _playerInput;

    private string shoesKey = "shoesKey";

    private void Start()
    {
        ///kjh1229 -
        sendMSGAlready = PersistentDataManager.Instance.GetDataWithParsing(shoesKey, false);

        if (sendMSGAlready.Equals(true))
            Interact();


        chatController = FindAnyObjectByType<ChatController>();
    }

    public void Interact()
    {
        _playerInput = FindAnyObjectByType<PlayerInput>();
        _playerInput.canRun = true;

        GetComponent<ActionOnTrigger>().ChangeUIData(uIData);

        if (!sendMSGAlready)
        {
            chatController.AddMessage(messageData.scriptLines);
        }
        sendMSGAlready = true;

        targetModel.SetActive(false);
        /// kjh
        PersistentDataManager.Instance.SaveData(shoesKey, sendMSGAlready);
        Debug.Log("shoesKey가 저장되었습니다.");
    }
}
