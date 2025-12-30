using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollConfigWindow : MonoBehaviour, ISceneContextBuilt
{
    [Header("BlurOverlayImage")]
    [SerializeField]
    private GameObject blurOverlayImage;
    [Space(50)]

    [Header("Config Screen")]
    [SerializeField]
    private GameObject sideModel;
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
    private bool _isDown = false;

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
    [Space(15)]
    [Header("달성시간으로 조절")]
    [Header("RightWardTime")]
    [SerializeField]
    private float _durationToRight = 0.5f;
    [Header("RightBackWardTime")]
    [SerializeField]
    private float _durationToRightBack = 0.4f;
    private bool _isRight = false;

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

    [Space(50)]
    [Header("Camera Screen 이건 투명도로 조절")]
    [SerializeField]
    private GameObject cameraScreen;
    [SerializeField]
    private float _durationToCamera = 0.5f; // 카메라 페이드 시간
    bool isCameraOn = false;

    public int Priority { get; set; } = 2;

    // --- 화면 끄고 키는 기능 ---

    public void Toggle()
    {
        if(transform.GetChild(1).gameObject.activeSelf)
        {
            ClosePanel();
        }
        else
        {
            OpenPanel();
        }
    }

    public void ClosePanel(bool useTimeContol = true)
    {
        if(useTimeContol)
        {
            GameManager.Instance.ChangeState(GameState.Playing);
        }

        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "GalleryScreen")
                continue;
            transform.GetChild(i).gameObject.SetActive(false);
        }
        sideModel.SetActive(false);
    }

    public void OpenPanel(bool useTimeContol = true)
    {
        if (useTimeContol)
        {
            GameManager.Instance.ChangeState(GameState.UI);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        sideModel.SetActive(true);
    }

    // --- 새로 추가된 카메라 제어 함수 ---
    public void OnCameraOpen()
    {
        if (!isCameraOn)
        {
            StopAllCoroutines();
            StartCoroutine(FadeCameraGroup(true));
        }
    }

    public void OnCameraBack()
    {
        if (isCameraOn)
        {
            StopAllCoroutines();
            StartCoroutine(FadeCameraGroup(false));
        }
    }

    // --- 새로 추가된 카메라 코루틴 (자식까지 투명도 조절) ---
    IEnumerator FadeCameraGroup(bool triggerOn)
    {
        // 자식까지 조절하기 위해 CanvasGroup 사용
        CanvasGroup cg = cameraScreen.GetComponent<CanvasGroup>();
        if (cg == null) cg = cameraScreen.AddComponent<CanvasGroup>();

        float startAlpha = cg.alpha;
        float targetAlpha = triggerOn ? 1f : 0f;
        float _elapseTime = 0;

        if (triggerOn)
        {
            cameraScreen.SetActive(true);
            isCameraOn = true;
        }

        while (_elapseTime < _durationToCamera)
        {
            _elapseTime += Time.deltaTime;
            float per = _elapseTime / _durationToCamera;
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, per);
            yield return null;
        }

        cg.alpha = targetAlpha;

        if (!triggerOn)
        {
            cameraScreen.SetActive(false);
            isCameraOn = false;
        }
    }

    // --- 기존 코드 (수정 없음) ---
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
            _isDown = false;
        }
    }
    public void OnRightWardScroll()
    {
        StopAllCoroutines();
        _isRight = true;
        StartCoroutine(RightWardScroll());
    }
    public void OnRightBackWardScroll()
    {
        if (_isRight)
        {
            StopAllCoroutines();
            StartCoroutine(RightBackWardScroll());
            _isRight = false;
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

    IEnumerator DownScroll()
    {
        float startPosY = _configScreen.transform.localPosition.y;
        float _elapseTime = 0;
        Image blurOverlayImageImage = blurOverlayImage.GetComponent<Image>();
        Color blurOverlayImageColor = blurOverlayImageImage.color;
        Image image = _configScreen.GetComponent<Image>();
        Color tempColor = image.color;
        while (_elapseTime < _durationToDown)
        {
            _elapseTime += Time.deltaTime;
            float per = _elapseTime / _durationToDown;
            _configScreen.transform.localPosition = new Vector3(0, Mathf.Lerp(startPosY, _targetDownPosition, per), 0);
            tempColor.a = Mathf.Lerp(0.1f, 255f / 255f, per);
            image.color = tempColor;
            blurOverlayImageColor.a = Mathf.Lerp(0.0f, 70 / 255f, per);
            blurOverlayImageImage.color = blurOverlayImageColor;
            yield return null;
        }
        _configScreen.transform.localPosition = new Vector3(0, _targetDownPosition, 0);
        blurOverlayImageColor.a = 70 / 255f;
        blurOverlayImageImage.color = blurOverlayImageColor;
        tempColor.a = 255f / 255f;
        image.color = tempColor;
    }

    IEnumerator UpScroll()
    {
        float _elapseTime = 0;
        Image image = _configScreen.GetComponent<Image>();
        Image blurOverlayImageImage = blurOverlayImage.GetComponent<Image>();
        Color blurOverlayImageColor = blurOverlayImageImage.color;
        Color tempColor = image.color;
        float startPosY = _configScreen.transform.localPosition.y;
        while (_elapseTime < _durationToUp)
        {
            _elapseTime += Time.deltaTime;
            float per = _elapseTime / _durationToUp;
            blurOverlayImageColor.a = Mathf.Lerp(70 / 255f, 0f, per);
            blurOverlayImageImage.color = blurOverlayImageColor;
            _configScreen.transform.localPosition = new Vector3(0, Mathf.Lerp(startPosY, _targetUpPosition, per), 0);
            tempColor.a = Mathf.Lerp(1f, 0.1f, per);
            image.color = tempColor;
            yield return null;
        }
        _configScreen.transform.localPosition = new Vector3(0, _targetUpPosition, 0);
        blurOverlayImageColor.a = 0 / 255f;
        blurOverlayImageImage.color = blurOverlayImageColor;
        tempColor.a = 0;
        image.color = tempColor;
    }

    IEnumerator RightWardScroll()
    {
        float _elapseTime = 0;
        Image image = _callScreen.GetComponent<Image>();
        Image blurOverlayImageImage = blurOverlayImage.GetComponent<Image>();
        Color blurOverlayImageColor = blurOverlayImageImage.color;
        Color tempColor = image.color;
        float startPosX = _callScreen.transform.localPosition.x;
        while (_elapseTime < _durationToRight)
        {
            _elapseTime += Time.deltaTime;
            float per = _elapseTime / _durationToRight;
            _callScreen.transform.localPosition = new Vector3(Mathf.Lerp(startPosX, _targetRightPosition, per), 0, 0);
            blurOverlayImageColor.a = Mathf.Lerp(0f, 70 / 255f, per);
            blurOverlayImageImage.color = blurOverlayImageColor;
            tempColor.a = Mathf.Lerp(0.1f, 255f / 255f, per);
            image.color = tempColor;
            yield return null;
        }
        _callScreen.transform.localPosition = new Vector3(_targetRightPosition, 0, 0);
        blurOverlayImageColor.a = 70 / 255f;
        blurOverlayImageImage.color = blurOverlayImageColor;
        tempColor.a = 255f / 255f;
        image.color = tempColor;
    }

    IEnumerator RightBackWardScroll()
    {
        float _elapseTime = 0;
        Image image = _callScreen.GetComponent<Image>();
        Image blurOverlayImageImage = blurOverlayImage.GetComponent<Image>();
        Color blurOverlayImageColor = blurOverlayImageImage.color;
        Color tempColor = image.color;
        float startPosX = _callScreen.transform.localPosition.x;
        while (_elapseTime < _durationToRightBack)
        {
            _elapseTime += Time.deltaTime;
            float per = _elapseTime / _durationToRightBack;
            _callScreen.transform.localPosition = new Vector3(Mathf.Lerp(startPosX, _targetRightBackWardPosition, per), 0, 0);
            blurOverlayImageColor.a = Mathf.Lerp(70 / 255f, 0f, per);
            blurOverlayImageImage.color = blurOverlayImageColor;
            tempColor.a = Mathf.Lerp(1f, 0.1f, per);
            image.color = tempColor;
            yield return null;
        }
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
        while (_elapseTime < _durationToLeft)
        {
            _elapseTime += Time.deltaTime;
            float per = _elapseTime / _durationToLeft;
            _messageScreen.transform.localPosition = new Vector3(Mathf.Lerp(startPosX, _targetLeftPosition, per), 0, 0);
            blurOverlayImageColor.a = Mathf.Lerp(0f, 70 / 255f, per);
            blurOverlayImageImage.color = blurOverlayImageColor;
            tempColor.a = Mathf.Lerp(0.1f, 255f / 255f, per);
            image.color = tempColor;
            yield return null;
        }
        _messageScreen.transform.localPosition = new Vector3(_targetLeftPosition, 0, 0);
        blurOverlayImageColor.a = 70 / 255f;
        blurOverlayImageImage.color = blurOverlayImageColor;
        tempColor.a = 255f / 255f;
        image.color = tempColor;
    }

    IEnumerator LeftBackWardScroll()
    {
        float _elapseTime = 0;
        Image image = _messageScreen.GetComponent<Image>();
        Image blurOverlayImageImage = blurOverlayImage.GetComponent<Image>();
        Color blurOverlayImageColor = blurOverlayImageImage.color;
        Color tempColor = image.color;
        float startPosX = _messageScreen.transform.localPosition.x;
        while (_elapseTime < _durationToLeftBack)
        {
            _elapseTime += Time.deltaTime;
            float per = _elapseTime / _durationToLeftBack;
            _messageScreen.transform.localPosition = new Vector3(Mathf.Lerp(startPosX, _targetLeftBackWardPosition, per), 0, 0);
            blurOverlayImageColor.a = Mathf.Lerp(70 / 255f, 0f, per);
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
    }

    public void OnSceneContextBuilt()
    {
        if (FindAnyObjectByType<PlayerInput>() != null)
        {
            FindAnyObjectByType<PlayerInput>().OnSettingKeyDowned += Toggle;
        }
        if (GameManager.Instance.GameState == GameState.Playing)
        {
            ClosePanel();
        }
    }
}