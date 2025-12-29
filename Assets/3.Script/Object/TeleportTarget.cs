using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTarget : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private StringDataSO messageData;

    [Header("Screen Point Spawn Layer")]
    [SerializeField]
    private LayerMask screenPointSpawnLayer;
    [SerializeField, Tooltip("해당변수가 우선권을 가집니다 0이라면 spawnPoint 실행")]
    private Vector2 screenSpawnPoint;

    private PlayerInput _playerInput;
    private ChatController chatController;

    private void Start()
    {
        _playerInput = FindAnyObjectByType<PlayerInput>();
        chatController = FindAnyObjectByType<ChatController>();
    }

    public void AddMSG()
    {
        chatController.AddMessage(messageData.scriptLines);
    }
    public void Teleport()
    {
        if(screenSpawnPoint != Vector2.zero)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenSpawnPoint);

            if(Physics.Raycast(ray, out RaycastHit hit, 100f, screenPointSpawnLayer))
            {
                //Debug.Log(hit.collider.name);
                Vector3 spawnPos = hit.point;
                spawnPos.y = 2f;
                target.position = spawnPos;
            }
        }
        else
        {
            if (target.TryGetComponent(out Pickle pickle))
            {
                pickle.SetPos(spawnPoint.position, target);
                pickle.ShowPickle();
            }
            else
            {
                target.position = spawnPoint.position;
            }
        }

    }

}
