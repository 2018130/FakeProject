using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouMoveYouDie : MonoBehaviour
{
    private PlayerLight light;
    private PlayerInput player;

    private bool _checkMove = false;

    private void Awake()
    {
        light = FindAnyObjectByType<PlayerLight>();
        player = FindAnyObjectByType<PlayerInput>();
    }

    private void Update()
    {
        if (_checkMove)
        {
            DontMove();
        }
    }

    public void StartCheckMove()
    {
        _checkMove = true;
    }
    public void StopCheckMove()
    {
        _checkMove = false;
    }
    private void DontMove()
    {
                if (player.MoveValue != Vector2.zero || player.MouseDelta != Vector2.zero || light.IsTurnOn)
                {
                    GameManager.Instance.ChangeState(GameState.Dead);
                    //TODO : End Game
                }
    }
}
