using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionOnTrigger : MonoBehaviour
{
    [SerializeField]
    UIData data;


    [SerializeField]
    private UnityEvent action;

    [SerializeField]
    [TagSelector]
    private string triggerTargetTag = "Player";

    private void Start()
    {
        if(!TryGetComponent(out Collider collider))
        {
            BoxCollider col = (BoxCollider)gameObject.AddComponent(typeof(BoxCollider));
            col.isTrigger = true;

            Debug.LogWarning($"Don't have collider in {gameObject.name}, created child within box collider");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(triggerTargetTag))
        {
            FindAnyObjectByType<interkeyUI>().SetInteractionKeyUI(data);
            action?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        FindAnyObjectByType<interkeyUI>().HideUI();
    }
}
