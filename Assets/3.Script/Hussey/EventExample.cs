using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventExample : MonoBehaviour
{
    private Pickle pickle;

    private void Awake()
    {
        pickle = FindAnyObjectByType<Pickle>();
        pickle.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //플레이어의 뒤쪽으로 나올 경우
            Vector3 destPos = -other.transform.forward * 10f;
            pickle.gameObject.SetActive(true);
            pickle.SetPos(destPos, other.transform);
            pickle.StartNav();
            gameObject.SetActive(false);
        }
    }
}
