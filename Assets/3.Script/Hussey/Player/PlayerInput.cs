using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private bool _blockBaseInput = false;
    public bool BlockBaseInput => _blockBaseInput;

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

    public bool canRun = false; //Ω≈πﬂ æÚ±‚ ¿¸±Ó¡ˆ ¥ﬁ∏Æ¥¬∞≈ ∏∑æ“¿Ω!! 1218
    public bool isRun => IsRun;

    private Animator ani;

    public event Action OnLightKeyDowned;
    public event Action OnInteractionDowned;
    public event Action OnSpaceKeyDowned;
    public event Action OnSettingKeyDowned;

    private bool _interactionPerformed = false;
    public bool InteractionPerformed => _interactionPerformed;

    //private bool IsPersonalView = false;
    //public bool isPersonalView => IsPersonalView;

    private void Awake()
    {
        ani = GetComponentInChildren<Animator>();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        mouseDelta = Vector2.zero;
    }

    public void Event_Move(InputAction.CallbackContext context)
    {
        if(GameManager.Instance.GameState == GameState.Playing)
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
        else
        {
            moveValue = Vector2.zero;
            ani.SetBool("Walk", false);
        }

    }


    //3?∏Ïπ≠ Íµ¨ÌòÑ?¥Îùº Ï£ºÏÑùÏ≤òÎ¶¨
    public void Event_Aim(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            mousePos = context.ReadValue<Vector2>();
        }
    }

    public void Event_PersonalView(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.GameState == GameState.Playing)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                mouseDelta = context.ReadValue<Vector2>();
            }
        }
    }

    public void Event_Run(InputAction.CallbackContext context)
    {
        if (!canRun)
            return;

        if (GameManager.Instance.GameState == GameState.Playing)
        {
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
    }

    public void Event_Light(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.GameState == GameState.Playing)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnLightKeyDowned?.Invoke();
            }
        }
    }
    public void Event_Interact(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            _interactionPerformed = true;
            OnInteractionDowned?.Invoke();
            
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            _interactionPerformed = false;
        }
    }
    public void Event_SpaceKeydown(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            OnSpaceKeyDowned?.Invoke();
        }
    }

    public void Event_SettingKeydown(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            OnSettingKeyDowned?.Invoke();
        }
    }
    //3?∏Ïπ≠ Íµ¨ÌòÑ?¥Îùº Ï£ºÏÑùÏ≤òÎ¶¨
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
