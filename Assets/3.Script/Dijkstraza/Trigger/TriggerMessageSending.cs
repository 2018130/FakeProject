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
    
    /// kjh
    private static int triggerMessageSendingKeyNum=0;
    private string triggerMessageSendingKey;
    ///kjh

    private void Start()
    {
        chatController = FindAnyObjectByType<ChatController>();

        ///kjh
        triggerMessageSendingKey = gameObject.name+"Key"+ triggerMessageSendingKeyNum;
        triggerMessageSendingKeyNum++;
        ///kjh
        if (PersistentDataManager.Instance != null)
        {
            _sendMSGAlready = PersistentDataManager.Instance.GetDataWithParsing(triggerMessageSendingKey,false);
        }


    }

    public void OnSendMessage()
    {
        if(!_sendMSGAlready)
        {
            chatController.AddMessage(messageData.scriptLines);
            _sendMSGAlready = true;
            ///kjh
            PersistentDataManager.Instance.SaveData(triggerMessageSendingKey, _sendMSGAlready);
            ///kjh
        }
    }
}
