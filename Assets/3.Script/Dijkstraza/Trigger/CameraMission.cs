using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMission : MonoBehaviour, IInteractable
{

    [SerializeField]
    private StringDataSO messageData;

    ChatController chatController;
    private bool sendMSGAlready = false;

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (sendMSGAlready.Equals(true))
        {
            Interact();
        }

        chatController = FindAnyObjectByType<ChatController>();

    }

    public void Interact()
    {
        if (!sendMSGAlready)
        {
            chatController.AddMessage(messageData.scriptLines);
        }
        sendMSGAlready = true;
    }

    
}
