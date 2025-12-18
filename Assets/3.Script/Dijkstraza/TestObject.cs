using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log($"{gameObject.name}상호작용 완료");
    }
}
