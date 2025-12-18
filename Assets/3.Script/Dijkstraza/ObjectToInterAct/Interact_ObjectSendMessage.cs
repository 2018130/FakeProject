using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class Interact_ObjectSendMessage : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject objectSendMsg;
    public Text text;

    private Collider col;

    private void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);
    }

    public void Interact()
    {
        //if (player.CompareTag("Shoes"))
        //{
        //    StartCoroutine("FindObject");
        //}
    }

    IEnumerator FindObject()
    {
        yield return null;
    }
}
