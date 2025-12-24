using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollConfigWindow : MonoBehaviour
{
    [Header("BlurOverlayImage")]
    [SerializeField]
    private GameObject blurOverlayImage;
    [Space(50)]

    [Header("Config Screen")]
    [SerializeField]
    private GameObject _configScreen;
    [Header("DownWardPosition")]
    [SerializeField]
    private float _targetDownPosition = 0;
    [Header("UpWardPosition")]
    [SerializeField]
    private float _targetUpPosition = 1920;
    [Space(15)]
    [Header("달성시간으로 조절")]
    [Header("DownWardTime")]
    [SerializeField]
    private float _durationToDown = 0.6f;
    [Header("UpWardTime")]
    [SerializeField]
    private float _durationToUp = 0.5f;
    private bool _isDown=false;



    [Space(50)]
    [Header("Call Screen")]
    [SerializeField]
    private GameObject _callScreen;
    [Header("RightWardPosition")]
    [SerializeField]
    private float _targetRightPosition = 0;
    [Header("RightBackWardPosition")]
    [SerializeField]
    private float _targetRightBackWardPosition = -1080;
    //[Header("이건 속도로 조절")]
    //[SerializeField]
    //private float _speed = 4000;
    //[SerializeField]
    //private float _speed2 = 4000;
    [Space(15)]
    [Header("달성시간으로 조절")]
    [Header("RightWardTime")]
    [SerializeField]
    private float _durationToRight = 0.5f;
    [Header("RightBackWardTime")]
    [SerializeField]
    private float _durationToRightBack = 0.4f;
    private bool _isRight=false;



    [Space(50)]
    [Header("Message Screen")]
    [SerializeField]
    private GameObject _messageScreen;
    [Header("RightWardPosition")]
    [SerializeField]
    private float _targetLeftPosition = 0;
    [Header("RightBackWardPosition")]
    [SerializeField]
    private float _targetLeftBackWardPosition = 1080;
    [Space(15)]
    [Header("달성시간으로 조절")]
    [Header("LeftWardTime")]
    [SerializeField]
    private float _durationToLeft = 0.5f;
    [Header("RightWardTime")]
    [SerializeField]
    private float _durationToLeftBack = 0.4f;
    private bool _isLeft = false;

    private void Awake()
    {
       
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //dijkstraza 테스트용
        //StartCoroutine(UpScroll());
        //StartCoroutine(DownScroll());

    }

    public void OnDownWardScroll()
    {
        StopAllCoroutines();
        _isDown = true;
        StartCoroutine(DownScroll());
    }
    public void OnUpWardScroll()
    {
        if (_isDown)
        {

        StopAllCoroutines();
        StartCoroutine(UpScroll());
        _isDown=false;
        }
    }
    public void OnRightWardScroll()
    {
        StopAllCoroutines();
        _isRight=true;
        StartCoroutine(RightWardScroll());
    }
    public void OnRightBackWardScroll()
    {
        if (_isRight)
        {
        StopAllCoroutines();
        StartCoroutine(RightBackWardScroll());
            _isRight=false;
        }
    }
    public void OnLefttWardScroll()
    {
        StopAllCoroutines();
        _isLeft = true;
        StartCoroutine(LeftWardScroll());
    }
    public void OnLeftBackWardScroll()
    {
        if (_isLeft)
        {
            StopAllCoroutines();
            StartCoroutine(LeftBackWardScroll());
            _isLeft = false;
        }
    }

    /// <summary>
    /// screen의 alpha값을 250으로 맞춰놓아서 뒷 배경이 비쳐보였으므로
    /// 255로 고정시켜 놨습니다
    /// 기본 코드는 주석처리 해뒀습니다
    /// </summary>
    /// <returns></returns>
    IEnumerator DownScroll()
    {
        
        ////목표 위치가 될때까지 하는 방식
        //while (_gameObject.transform.localPosition.y > _targetPosition)   //목표 위치와 비슷한 위치가 되면 while문을 벗어남
        //{
        //    _gameObject.transform.localPosition -= Vector3.up * Time.deltaTime * _speed;
        //    yield return null;
        //}

        //달성 시간을 정하여 그 시간안에 달성하게 하는 방식
        float startPosY = _configScreen.transform.localPosition.y;
        float _elapseTime = 0;

        Image blurOverlayImageImage = blurOverlayImage.GetComponent<Image>();
        Color blurOverlayImageColor = blurOverlayImageImage.color;

        Image image = _configScreen.GetComponent<Image>();
        Color tempColor = image.color;

        while (_elapseTime < _durationToDown)      //달성 시간과 비슷한 시간이 되면 while문을 벗어남
        {
            _elapseTime += Time.deltaTime;          //누적시간만들기
            float per = _elapseTime / _durationToDown;    //누적시간이 지속시간에 비하여 몇%인지 확인하여 넣기

            _configScreen.transform.localPosition = new Vector3(0, Mathf.Lerp(startPosY, _targetDownPosition, per), 0);
            tempColor.a = Mathf.Lerp(0.1f, 255f, per);
            //tempColor.a = Mathf.Lerp(0.1f, 250/255f, per);
            image.color = tempColor;

            blurOverlayImageColor.a = Mathf.Lerp(0.0f, 70/255f, per);
            blurOverlayImageImage.color = blurOverlayImageColor;

            yield return null;
        }


        // 목표의 위치가 2씩 차는데 마지막이 99라면 합했을 때 101이 됨으로 while문 내부를 실행하지 않고 밖으로 나가버림 
        // 그러나 우리의 목표는 100임, 이 차이를 보정하기 위해 마지막에 대상의 position에 목표 position 을 대입함.
        // 마찬가지로 진행도가 0.4%씩 차는데 마지막이 99.7%이면 합했을 때 100.1%이므로 while문 내부를 실행하지 않고 밖으로 나가버림
        // 그러나 우리의 목표는 100%에 해당하는 값임, 이 차이를 보정하기 위해 최종적으로 대상의 position에 목표 position값을 대입해버림
        _configScreen.transform.localPosition = new Vector3(0, _targetDownPosition, 0);

        blurOverlayImageColor.a = 70/255f;
        blurOverlayImageImage.color = blurOverlayImageColor;

        tempColor.a = 255f;
        //tempColor.a = 250 / 255f;
        image.color = tempColor;

       
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
        Image image = _configScreen.GetComponent<Image>();

        Image blurOverlayImageImage = blurOverlayImage.GetComponent<Image>();
        Color blurOverlayImageColor = blurOverlayImageImage.color;

        Color tempColor = image.color;
        float startPosY = _configScreen.transform.localPosition.y;
        while (_elapseTime < _durationToUp)      //달성 시간과 비슷한 시간이 되면 while문을 벗어남
        {
            _elapseTime += Time.deltaTime;              //누적시간만들기
            float per = _elapseTime / _durationToUp;       //누적시간이 지속시간에 비하여 몇%인지 확인하여 넣기

            blurOverlayImageColor.a = Mathf.Lerp(70/255f, 0f, per);
            blurOverlayImageImage.color = blurOverlayImageColor;

            _configScreen.transform.localPosition = new Vector3(0, Mathf.Lerp(startPosY, _targetUpPosition, per), 0);
            tempColor.a = Mathf.Lerp(1f, 0.1f, per);
            image.color = tempColor;
            yield return null;
        
        }

        // 목표의 위치가 2씩 차는데 마지막이 99라면 합했을 때 101이 됨으로 while문 내부를 실행하지 않고 밖으로 나가버림 
        // 그러나 우리의 목표는 100임, 이 차이를 보정하기 위해 마지막에 대상의 position에 목표 position 을 대입함.
        // 마찬가지로 진행도가 0.4%씩 차는데 마지막이 99.7%이면 합했을 때 100.1%이므로 while문 내부를 실행하지 않고 밖으로 나가버림
        // 그러나 우리의 목표는 100%에 해당하는 값임, 이 차이를 보정하기 위해 최종적으로 대상의 position에 목표 position값을 대입해버림
        _configScreen.transform.localPosition = new Vector3(0, _targetUpPosition, 0);

        blurOverlayImageColor.a = 0/255f;
        blurOverlayImageImage.color = blurOverlayImageColor;

        tempColor.a = 0;
        image.color = tempColor;
    }

    IEnumerator RightWardScroll()
    {
        ////목표 위치가 될때까지 하는 방식
        //while (_gameObject.transform.localPosition.y < _targetPosition2)   //목표 위치와 비슷한 위치가 되면 while문을 벗어남
        //{
        //    _gameObject.transform.localPosition += Vector3.up * Time.deltaTime * _speed;
        //    yield return null;
        //}

        //달성 시간을 정하여 그 시간안에 달성하게 하는 방식
        float _elapseTime = 0;
        Image image = _callScreen.GetComponent<Image>();

        Image blurOverlayImageImage = blurOverlayImage.GetComponent<Image>();
        Color blurOverlayImageColor = blurOverlayImageImage.color;

        Color tempColor = image.color;
        float startPosX = _callScreen.transform.localPosition.x;
        while (_elapseTime < _durationToRight)      //달성 시간과 비슷한 시간이 되면 while문을 벗어남
        {
            _elapseTime += Time.deltaTime;              //누적시간만들기
            float per = _elapseTime / _durationToRight;       //누적시간이 지속시간에 비하여 몇%인지 확인하여 넣기

            _callScreen.transform.localPosition = new Vector3(Mathf.Lerp(startPosX, _targetRightPosition, per),0, 0);

            blurOverlayImageColor.a = Mathf.Lerp(0f, 70/255f,  per);
            blurOverlayImageImage.color = blurOverlayImageColor;

            tempColor.a = Mathf.Lerp(0.1f, 255f, per);
            //tempColor.a = Mathf.Lerp(0.1f, 250 / 255f, per);
            image.color = tempColor;
            yield return null;
            Debug.Log(_elapseTime + "  " + _durationToRight);
        }

        // 목표의 위치가 2씩 차는데 마지막이 99라면 합했을 때 101이 됨으로 while문 내부를 실행하지 않고 밖으로 나가버림 
        // 그러나 우리의 목표는 100임, 이 차이를 보정하기 위해 마지막에 대상의 position에 목표 position 을 대입함.
        // 마찬가지로 진행도가 0.4%씩 차는데 마지막이 99.7%이면 합했을 때 100.1%이므로 while문 내부를 실행하지 않고 밖으로 나가버림
        // 그러나 우리의 목표는 100%에 해당하는 값임, 이 차이를 보정하기 위해 최종적으로 대상의 position에 목표 position값을 대입해버림
        _callScreen.transform.localPosition = new Vector3(_targetRightPosition,0, 0);

        blurOverlayImageColor.a = 70 / 255f;
        blurOverlayImageImage.color = blurOverlayImageColor;

        tempColor.a = 255f;
        //tempColor.a = 250 / 255f;
        image.color = tempColor;
    }

    IEnumerator RightBackWardScroll()
    {
        ////목표 위치가 될때까지 하는 방식
        //while (_gameObject.transform.localPosition.y < _targetPosition2)   //목표 위치와 비슷한 위치가 되면 while문을 벗어남
        //{
        //    _gameObject.transform.localPosition += Vector3.up * Time.deltaTime * _speed;
        //    yield return null;
        //}

        //달성 시간을 정하여 그 시간안에 달성하게 하는 방식
        float _elapseTime = 0;
        Image image = _callScreen.GetComponent<Image>();

        Image blurOverlayImageImage = blurOverlayImage.GetComponent<Image>();
        Color blurOverlayImageColor = blurOverlayImageImage.color;

        Color tempColor = image.color;
        float startPosX = _callScreen.transform.localPosition.x;
        while (_elapseTime < _durationToRightBack)      //달성 시간과 비슷한 시간이 되면 while문을 벗어남
        {
            _elapseTime += Time.deltaTime;              //누적시간만들기
            float per = _elapseTime / _durationToRightBack;       //누적시간이 지속시간에 비하여 몇%인지 확인하여 넣기

            _callScreen.transform.localPosition = new Vector3(Mathf.Lerp(startPosX, _targetRightBackWardPosition, per), 0, 0);

            blurOverlayImageColor.a = Mathf.Lerp(70 / 255f, 0f, per);
            blurOverlayImageImage.color = blurOverlayImageColor;

            tempColor.a = Mathf.Lerp(1f, 0.1f, per);
            image.color = tempColor;
            yield return null;
            Debug.Log(_elapseTime + "  " + _durationToRightBack);
        }

        // 목표의 위치가 2씩 차는데 마지막이 99라면 합했을 때 101이 됨으로 while문 내부를 실행하지 않고 밖으로 나가버림 
        // 그러나 우리의 목표는 100임, 이 차이를 보정하기 위해 마지막에 대상의 position에 목표 position 을 대입함.
        // 마찬가지로 진행도가 0.4%씩 차는데 마지막이 99.7%이면 합했을 때 100.1%이므로 while문 내부를 실행하지 않고 밖으로 나가버림
        // 그러나 우리의 목표는 100%에 해당하는 값임, 이 차이를 보정하기 위해 최종적으로 대상의 position에 목표 position값을 대입해버림
        _callScreen.transform.localPosition = new Vector3(_targetRightBackWardPosition, 0, 0);

        blurOverlayImageColor.a = 0f;
        blurOverlayImageImage.color = blurOverlayImageColor;

        tempColor.a = 0;
        image.color = tempColor;
    }

    IEnumerator LeftWardScroll()
    {
        float _elapseTime = 0;
        Image image = _messageScreen.GetComponent<Image>();

        Image blurOverlayImageImage = blurOverlayImage.GetComponent<Image>();
        Color blurOverlayImageColor = blurOverlayImageImage.color;

        Color tempColor = image.color;
        float startPosX = _messageScreen.transform.localPosition.x;
        while (_elapseTime < _durationToLeft)      //달성 시간과 비슷한 시간이 되면 while문을 벗어남
        {
            _elapseTime += Time.deltaTime;              //누적시간만들기
            float per = _elapseTime / _durationToLeft;       //누적시간이 지속시간에 비하여 몇%인지 확인하여 넣기

            _messageScreen.transform.localPosition = new Vector3(Mathf.Lerp(startPosX, _targetLeftPosition, per), 0, 0);

            blurOverlayImageColor.a = Mathf.Lerp(0f, 70 / 255f,  per);
            blurOverlayImageImage.color = blurOverlayImageColor;

            tempColor.a = Mathf.Lerp(0.1f, 255f, per);
            //tempColor.a = Mathf.Lerp(0.1f, 250 / 255f, per);
            image.color = tempColor;
            yield return null;
            
        }
        _messageScreen.transform.localPosition = new Vector3(_targetRightPosition, 0, 0);

        blurOverlayImageColor.a = 70 / 255f;
        blurOverlayImageImage.color = blurOverlayImageColor;

        tempColor.a = 255f;
        //tempColor.a = 250 / 255f;
        image.color = tempColor;
        yield return null;
    }

    IEnumerator LeftBackWardScroll()
    {
        float _elapseTime = 0;
        Image image = _messageScreen.GetComponent<Image>();

        Image blurOverlayImageImage = blurOverlayImage.GetComponent<Image>();
        Color blurOverlayImageColor = blurOverlayImageImage.color;

        Color tempColor = image.color;
        float startPosX = _messageScreen.transform.localPosition.x;
        while (_elapseTime < _durationToLeftBack)      //달성 시간과 비슷한 시간이 되면 while문을 벗어남
        {
            _elapseTime += Time.deltaTime;              //누적시간만들기
            float per = _elapseTime / _durationToLeftBack;       //누적시간이 지속시간에 비하여 몇%인지 확인하여 넣기

            _messageScreen.transform.localPosition = new Vector3(Mathf.Lerp(startPosX, _targetLeftBackWardPosition, per), 0, 0);

            blurOverlayImageColor.a = Mathf.Lerp(70 / 255f, 0f,per);
            blurOverlayImageImage.color = blurOverlayImageColor;

            tempColor.a = Mathf.Lerp(1f, 0.1f, per);
            image.color = tempColor;
            yield return null;
            
        }
        _messageScreen.transform.localPosition = new Vector3(_targetLeftBackWardPosition, 0, 0);

        blurOverlayImageColor.a = 0f;
        blurOverlayImageImage.color = blurOverlayImageColor;

        tempColor.a = 0;
        image.color = tempColor;

        yield return null;
    }

}
