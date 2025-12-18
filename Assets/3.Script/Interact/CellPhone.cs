using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPhone : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if(TryGetComponent(out FlutterEffect flutterEffect))
        {
            flutterEffect.StartFluttering();
        }

        transform.SetParent(GameManager.Instance.CurrentSceneContext.Player.PhonePoint);
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
    }
}
