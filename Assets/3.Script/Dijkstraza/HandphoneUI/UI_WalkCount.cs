using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UI_WalkCount : MonoBehaviour, ISceneContextBuilt
{
    [Header("UI References")]
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private float fillSpeed = 5f; // 슬라이더가 부드럽게 차오르는 속도
    [SerializeField]
    private Text currentWalkNum;

    [Header("Auto References")]
    [SerializeField]
    private PlayerInput playerInput;

    [Header("Settings")]
    public float stepInterval = 0.5f;
    public float minWalkingSpeed = 0.1f;
    public int maxSteps = 10000; // 슬라이더의 최대치 (목표 걸음 수)

    [Header("Status")]
    public int currentSteps = 0;
    private float _timer = 0f;

    public int Priority { get; set; }


    public void OnSceneContextBuilt()
    {
        // 안전하게 참조 가져오기 (Null 체크 권장)
        playerInput = GameManager.Instance.CurrentSceneContext.Player.GetComponent<PlayerInput>();
    }

    private void Start()
    {
        // 슬라이더 초기화
        if (slider != null)
        {
            slider.maxValue = 1f; // 슬라이더를 0~1 비율로 사용
            slider.value = 0f;
        }
    }

    private void Update()
    {
        if (playerInput == null) return;

        CheckMovement();
        UpdateUI();
    }

    private void CheckMovement()
    {
        // [중요] Y축(점프/낙하) 속도를 제외하고 수평 속도(X, Z)만 계산합니다.
        Vector3 horizontalVelocity = new Vector3(playerInput.MoveValue.x, 0, playerInput.MoveValue.y);

        if (horizontalVelocity.magnitude > minWalkingSpeed)
        {
            ProcessSteps();
        }
        else
        {
            _timer = 0f; // 멈추면 타이머 리셋 (연속 걸음 끊김)
        }
    }

    private void ProcessSteps()
    {
        _timer += Time.deltaTime;

        if (_timer >= stepInterval)
        {
            currentSteps++;
            currentWalkNum.text = currentSteps.ToString();
            // 목표치보다 커지지 않게 제한 (선택 사항)
            if (currentSteps > maxSteps) currentSteps = maxSteps;

            _timer = 0f;
        }
    }

    private void UpdateUI()
    {
        if (slider == null) return;

        // 현재 걸음 수 비율 (0.0 ~ 1.0)
        float targetValue = (float)currentSteps / maxSteps;

        // fillSpeed를 사용하여 부드럽게 슬라이더 업데이트 (Lerp)
        slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * fillSpeed);
    }
}