using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private Vector2 moveValue = Vector2.zero;
    public Vector2 MoveValue => moveValue;

    [SerializeField]
    private Vector2 mousePos = Vector2.zero;
    public Vector2 MousePos => mousePos;

    private bool IsRun = false;
    public bool isRun => IsRun;

    public void Event_Move(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            moveValue = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveValue = Vector2.zero;
        }
    }

    public void Event_Aim(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            mousePos = context.ReadValue<Vector2>();
        }
    }

    public void Event_Run(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            IsRun = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            IsRun = false;
        }
    }
}
