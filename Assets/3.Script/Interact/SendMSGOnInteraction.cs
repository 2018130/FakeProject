using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMSGOnInteraction : MonoBehaviour, IInteractable
{
    [SerializeField]
    private StringDataSO messageData;
    private ChatController chatController;
    [SerializeField]
    private GameObject targetModel;

    private bool _sendMSGAlready = false;

    private void Start()
    {
        chatController = FindAnyObjectByType<ChatController>();
    }

    public void Interact()
    {
        if (!_sendMSGAlready)
        {
            chatController.AddMessage(messageData.scriptLines);
            _sendMSGAlready = true;

            targetModel.SetActive(false);
        }
    }
}
