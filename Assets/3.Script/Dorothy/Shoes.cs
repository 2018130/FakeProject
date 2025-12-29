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

    //
    [SerializeField]
    private UIData uIData;
    private PlayerInput _playerInput;

    private string shoesKey = "shoesKey";

    private void Start()
    {
        chatController = FindAnyObjectByType<ChatController>();
        _playerInput = FindAnyObjectByType<PlayerInput>();
    }

    public void Interact()
    {
        _playerInput.canRun = true;

        GetComponent<ActionOnTrigger>().ChangeUIData(uIData);

        if (!sendMSGAlready)
        {
            chatController.AddMessage(messageData.scriptLines);
            sendMSGAlready = true;

            targetModel.SetActive(false);
        }

        /// kjh
        PersistentDataManager.Instance.SaveData(shoesKey, sendMSGAlready);
    }
}
