using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class testScroll : MonoBehaviour
{
    [Header("이건 속도로 조절")]
    [SerializeField]
    private GameObject _gameObject;
    [SerializeField]
    private float _targetPosition = 85;
    [SerializeField]
    private float _speed = 4000;

    [Space(15)]
    [SerializeField]
    private float _targetPosition2 = 1835;
    [SerializeField]
    private float _speed2 = 4000;


    [Space(50)]
    [Header("이건 달성시간으로 조절")]
    [SerializeField]
    private float _duration = 5f;
    [Space(15)]
    [SerializeField]
    private float _duration2 = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //dijkstraza 테스트용
        //StartCoroutine(UpScroll());
        //StartCoroutine(DownScroll());
    }

    public void OnUpperScroll()
    {
        StartCoroutine(UpScroll());
    }
    public void DownUpperScroll()
    {
        StartCoroutine(DownScroll());
    }



    IEnumerator DownScroll()
    {
        ////목표 위치가 될때까지 하는 방식
        //while (_gameObject.transform.localPosition.y > _targetPosition)   //목표 위치와 비슷한 위치가 되면 while문을 벗어남
        //{
        //    _gameObject.transform.localPosition -= Vector3.up * Time.deltaTime * _speed;
        //    yield return null;
        //}

        //달성 시간을 정하여 그 시간안에 달성하게 하는 방식
        float _elapseTime = 0;
        while (_elapseTime < _duration)      //달성 시간과 비슷한 시간이 되면 while문을 벗어남
        {
            _elapseTime += Time.deltaTime;          //누적시간만들기
            float per = _elapseTime / _duration;    //누적시간이 지속시간에 비하여 몇%인지 확인하여 넣기

            _gameObject.transform.localPosition = new Vector3(0, Mathf.Lerp(_gameObject.transform.localPosition.y, _targetPosition, _elapseTime), 0);
            yield return null;
        }


        // 목표의 위치가 2씩 차는데 마지막이 99라면 합했을 때 101이 됨으로 while문 내부를 실행하지 않고 밖으로 나가버림 
        // 그러나 우리의 목표는 100임, 이 차이를 보정하기 위해 마지막에 대상의 position에 목표 position 을 대입함.
        // 마찬가지로 진행도가 0.4%씩 차는데 마지막이 99.7%이면 합했을 때 100.1%이므로 while문 내부를 실행하지 않고 밖으로 나가버림
        // 그러나 우리의 목표는 100%에 해당하는 값임, 이 차이를 보정하기 위해 최종적으로 대상의 position에 목표 position값을 대입해버림
        _gameObject.transform.localPosition = new Vector3(0, _targetPosition, 0);
    }

    IEnumerator UpScroll()
    {
        ////목표 위치가 될때까지 하는 방식
        //while (_gameObject.transform.localPosition.y < _targetPosition2)   //목표 위치와 비슷한 위치가 되면 while문을 벗어남
        //{
        //    _gameObject.transform.localPosition += Vector3.up * Time.deltaTime * _speed;
        //    yield return null;
        //}

        //달성 시간을 정하여 그 시간안에 달성하게 하는 방식
        float _elapseTime = 0;
        while (_elapseTime < _duration2)      //달성 시간과 비슷한 시간이 되면 while문을 벗어남
        {
            _elapseTime += Time.deltaTime;              //누적시간만들기
            float per = _elapseTime / _duration2;       //누적시간이 지속시간에 비하여 몇%인지 확인하여 넣기

            _gameObject.transform.localPosition = new Vector3(0, Mathf.Lerp(_gameObject.transform.localPosition.y, _targetPosition2, _elapseTime), 0);
            yield return null;
        }


        // 목표의 위치가 2씩 차는데 마지막이 99라면 합했을 때 101이 됨으로 while문 내부를 실행하지 않고 밖으로 나가버림 
        // 그러나 우리의 목표는 100임, 이 차이를 보정하기 위해 마지막에 대상의 position에 목표 position 을 대입함.
        // 마찬가지로 진행도가 0.4%씩 차는데 마지막이 99.7%이면 합했을 때 100.1%이므로 while문 내부를 실행하지 않고 밖으로 나가버림
        // 그러나 우리의 목표는 100%에 해당하는 값임, 이 차이를 보정하기 위해 최종적으로 대상의 position에 목표 position값을 대입해버림
        _gameObject.transform.localPosition = new Vector3(0, _targetPosition2, 0);
    }

}
