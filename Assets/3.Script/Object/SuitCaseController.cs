using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCaseController : MonoBehaviour
{
    [Header("Float Setting")]
    [SerializeField, Tooltip("키 입력이 몇번 들어오고 열릴건지")]
    private int maxFloatCount = 3;
    private int floatCount = 0;
    [SerializeField]
    private float floatingPower = 10f;
    [SerializeField]
    private float floatingPowerWeightPerCycle = 2f;
    [SerializeField]
    private float floatTimePerCycle = 1f;
    private bool _isFloating = false;

    [Space]

    [Header("Reference")]
    [SerializeField]
    private GameObject closedCover;
    [SerializeField]
    private GameObject openedCover;

    private PlayerInput playerInput;

    private void Start()
    {
        playerInput = FindAnyObjectByType<PlayerInput>();
        playerInput.OnSpaceKeyDowned += StartFloating;
    }

    public void OpenCover()
    {
        closedCover.SetActive(false);
        openedCover.SetActive(true);
    }
    public void CloseCover()
    {
        closedCover.SetActive(true);
        openedCover.SetActive(false);
    }

    public void StartFloating()
    {
        if (floatCount >= maxFloatCount)
        {
            // TODO 시점변환
        }
        else if (!_isFloating)
        {
            floatCount++;
            StartCoroutine(Floating_co(2));
        }
    }

    private IEnumerator Floating_co(int floatCount)
    {
        _isFloating = true;
        Vector3 rotAxis = new Vector3(UnityEngine.Random.Range(0f, 1f),
                                        UnityEngine.Random.Range(0f, 1f),
                                          UnityEngine.Random.Range(1f, 2f));
        rotAxis = rotAxis.normalized;
        for (int i = 0; i < floatCount; i++)
        {
            float time = 0f;
            while (time <= floatTimePerCycle / 2)
            {
                yield return null;
                time += Time.deltaTime;

                transform.Rotate(rotAxis, floatingPower * floatingPowerWeightPerCycle * (this.floatCount + 1)
                    * Time.deltaTime * time * GameManager.Instance.CurrentSceneContext.GameTimeScale);
            }

            while (time > 0)
            {
                yield return null;
                time -= Time.deltaTime;

                transform.Rotate(-rotAxis, floatingPower * floatingPowerWeightPerCycle * (this.floatCount + 1)
                    * Time.deltaTime * time * GameManager.Instance.CurrentSceneContext.GameTimeScale);
            }
        }

        transform.rotation = Quaternion.identity;
        _isFloating = false;
    }
}
