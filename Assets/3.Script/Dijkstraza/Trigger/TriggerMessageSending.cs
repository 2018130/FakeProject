using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TriggerMessageSending : MonoBehaviour
{
    [SerializeField]
    private StringDataSO messageData;
    private ChatController chatController;

    private bool _sendMSGAlready = false;

    private void Start()
    {
        chatController = FindAnyObjectByType<ChatController>();
    }

    public void OnSendMessage()
    {
        if(!_sendMSGAlready)
        {
            chatController.AddMessage(messageData.scriptLines);
            _sendMSGAlready = true;
        }
    }
}
