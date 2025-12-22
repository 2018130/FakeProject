using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouMoveYouDie : MonoBehaviour
{
    private PlayerLight light;
    private PlayerInput player;

    private void Awake()
    {
        light = FindAnyObjectByType<PlayerLight>();
        player = FindAnyObjectByType<PlayerInput>();
    }

    public void DontMove()
    {
        if(light.Light.enabled)
        {
            //TODO : if(isCellphone true)
            if(player.MoveValue != Vector2.zero && player.MouseDelta != Vector2.zero)
            {
                //TODO : End Game
            }
        }
    }
}
