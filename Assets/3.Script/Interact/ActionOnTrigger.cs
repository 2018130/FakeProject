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

    private bool _isTriggered = false;

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
        if(other.CompareTag(triggerTargetTag) && !_isTriggered)
        {
            // ?. -> 널이라면 뒤에것 실행 x, 그렇지 않다면 실행
            FindAnyObjectByType<interkeyUI>()?.SetInteractionKeyUI(data);
            action?.Invoke();
            _isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        FindAnyObjectByType<interkeyUI>()?.SetActive(false);
    }

    public void ChangeUIData(UIData uIData)
    {
        data = uIData;
        FindAnyObjectByType<interkeyUI>()?.SetInteractionKeyUI(data);
    }
}
