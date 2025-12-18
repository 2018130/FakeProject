using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class Shoes : MonoBehaviour
{
    private PlayerInput _playerInput;

    private void Start()
    {
        _playerInput = FindAnyObjectByType<PlayerInput>();
    }

    public void CanRun()
    {
        _playerInput.canRun = true;
    }
}
