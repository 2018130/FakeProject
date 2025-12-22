using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDeadBody : MonoBehaviour
{
    //private PlayerMove player;

    //[SerializeField] private float zLength = 10f;
    //[SerializeField] private float yLenhth = 30f;

    private Transform obj;

    private void Awake()
    {
        //player = FindAnyObjectByType<PlayerMove>();
        obj = transform.GetChild(0);
        obj.gameObject.SetActive(false);
    }

    public void DJDropThatShit()
    {
        //obj.position = transform.position + Vector3.forward * zLength + Vector3.up * yLenhth;
        obj.gameObject.SetActive(true);

        //SoundManager.Instance.PlaySFX(ESFX.SFX_DropDeadBody);
    }
}
