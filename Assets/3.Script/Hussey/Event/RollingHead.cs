using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingHead : MonoBehaviour
{
    private Transform obj;
    private Rigidbody rigid;
    [SerializeField] private float headSpeed = 5f;

    private void Awake()
    {
        obj = transform.GetChild(0);
        obj.TryGetComponent(out rigid);
        obj.gameObject.SetActive(false);
    }

    public void SpinThatShit()
    {
        obj.gameObject.SetActive(true);
        rigid.AddForce(Vector3.left * headSpeed);
    }
}
