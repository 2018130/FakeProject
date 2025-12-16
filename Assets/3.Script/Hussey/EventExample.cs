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

    //private void ontriggetenter(Collision collision)
    //{
    //    if(collision.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("comapareTag player");
    //        //플레이어의 뒤쪽으로 나올 경우
    //        pickle.SetPos(Vector3.back * 10f);
    //        pickle.gameObject.SetActive(true);
    //        pickle.StartNav();
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("comapareTag player");
            Vector3 playerPos = other.gameObject.transform.position;
            //플레이어의 뒤쪽으로 나올 경우
            Vector3 destPos = -other.transform.forward * 10f;
            pickle.gameObject.SetActive(true);
            pickle.SetPos(playerPos, destPos);
            pickle.StartNav();
            gameObject.SetActive(false);
        }
    }
}
