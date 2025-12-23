using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePickle : MonoBehaviour
{
    private Pickle pickle;

    private void Awake()
    {
        pickle = FindAnyObjectByType<Pickle>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            pickle.StopNav();
            pickle.HidePickle();
        }
    }
}
