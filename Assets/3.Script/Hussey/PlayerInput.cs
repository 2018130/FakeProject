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

    [SerializeField]
    private Vector2 mouseDelta = Vector2.zero;
    public Vector2 MouseDelta => mouseDelta;

    private bool IsRun = false;

    public bool canRun = false; //신발 얻기 전까지 달리는거 막았음!! 1218
    public bool isRun => IsRun;

    private Animator ani;

    public event Action OnLightKeyDowned;

    //private bool IsPersonalView = false;
    //public bool isPersonalView => IsPersonalView;

    private void Awake()
    {
        ani = GetComponentInChildren<Animator>();
    }

    private void LateUpdate()
    {
        mouseDelta = Vector2.zero;
    }

    public void Event_Move(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            moveValue = context.ReadValue<Vector2>();
            ani.SetBool("Walk", true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveValue = Vector2.zero;
            ani.SetBool("Walk", false);
        }
    }


    //3인칭 구현이라 주석처리
    //public void Event_Aim(InputAction.CallbackContext context)
    //{
    //    if (context.phase == InputActionPhase.Performed)
    //    {
    //        mousePos = context.ReadValue<Vector2>();
    //    }
    //}

    public void Event_PersonalView(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            mouseDelta = context.ReadValue<Vector2>();
        }
    }

    public void Event_Run(InputAction.CallbackContext context)
    {
        if (!canRun)
            return;
        if(context.phase==InputActionPhase.Performed)
        {
            IsRun = true;
            ani.SetBool("Run", true);
        }
        else if(context.phase==InputActionPhase.Canceled)
        {
            IsRun = false;
            ani.SetBool("Run", false);
        }
        //------ ^^^^^ 신발관련 넣은것 ^^^^ 위------- 1218

        if (context.phase == InputActionPhase.Performed)
        {
            IsRun = true;
            ani.SetBool("Run", true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            IsRun = false;
            ani.SetBool("Run", false);
        }
    }

    public void Event_Light(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            Debug.Log("1111");
            OnLightKeyDowned?.Invoke();
        }
    }

    //3인칭 구현이라 주석처리
    //public void Event_ChangeView(InputAction.CallbackContext context)
    //{
    //    if(context.phase == InputActionPhase.Performed)
    //    {
    //        IsPersonalView = !IsPersonalView;
    //    }
    //    //else if(context.phase == InputActionPhase.Canceled)
    //    //{
    //    //    IsPersonalView = false;
    //    //}
    //}
}
