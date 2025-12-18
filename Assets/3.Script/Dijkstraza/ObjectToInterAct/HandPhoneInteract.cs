using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class HandPhoneInteract : MonoBehaviour, IInteractable
{
    private void Start()
    {
        Debug.Log("핸드폰이 등장했습니다.");
    }

    public void Interact(InputAction.CallbackContext context)
    {
        Debug.Log("핸드폰과 상호작용시작,");
        // 핸드폰을 핸드폰을 들고다니는 손의 point에 고정시킵니다.
    }

    public void Interact()
    {
        throw new NotImplementedException();
    }
}

