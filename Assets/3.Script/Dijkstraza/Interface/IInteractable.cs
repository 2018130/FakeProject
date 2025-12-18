using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public interface IInteractable
{
    void Interact();

    Transform transform { get; }
    GameObject gameObject { get; }

}
