using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollConfigWindow : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject1;
    [SerializeField] private GameObject _gameObject2;

    void Start()
    {
        ScrollPanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ScrollPanel()
    {
        float _movNumber = Mathf.Lerp(0,1000f,200);
        _gameObject1.transform.localPosition = new Vector3(0, _movNumber, 0);
        _gameObject2.transform.localPosition = new Vector3(0, -_movNumber, 0);
    }

}
