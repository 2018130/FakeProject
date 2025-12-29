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

    private string _sendMSGOnInteractionKey = "_sendMSGOnInteractionKey";
    private void Start()
    {
        chatController = FindAnyObjectByType<ChatController>();

        if (_sendMSGAlready.Equals(true))
            Interact();

        chatController = FindAnyObjectByType<ChatController>();

    }

    public void Interact()
    {
        if (!_sendMSGAlready)
        {
            chatController.AddMessage(messageData.scriptLines);
            _sendMSGAlready = true;

            ///kjh
            PersistentDataManager.Instance.SaveData(_sendMSGOnInteractionKey, _sendMSGAlready);
            targetModel.SetActive(false);
            Debug.Log("IDCard∏¶ »πµÊ«ﬂΩ¿¥œ¥Ÿ.");
        }
    }
}
