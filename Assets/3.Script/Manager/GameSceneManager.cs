using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    [Header("Dead")]
    [SerializeField] private Transform cameraPoint;

    [SerializeField] private GameObject pickle;

    [SerializeField] private Transform destPos;

    [SerializeField] private float popupSpeed = 0.7f;

    [SerializeField] private float popupTime = 1f;

    [SerializeField] private SceneContext sceneEffect;

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject restartUI;

    [SerializeField] private Image gameOverPanel;

    private Color startColor;

    private bool isDeadScene = false;


    private void Awake()
    {
        Debug.Log("call awake");
        pickle.SetActive(false);
        gameOverUI.SetActive(false);
        restartUI.SetActive(false);
        isDeadScene = false;

        HideBackground();
    }

    private void Update()
    {
        if (!isDeadScene)
        {
            Debug.Log("!isdeadscene");
            if (GameManager.Instance.GameState == GameState.Dead)
            {
                Debug.Log("if¹® µé¾î¿È");
                isDeadScene = true;
                StartCoroutine(PicklePopup_co());
            }
        }
    }

    //private void Dead()
    //{
    //    Debug.Log("dead avtive");
    //    StartCoroutine(PicklePopup());
    //    sceneEffect.FadeOut();
    //    gameOverUI.SetActive(true);
    //}

    private IEnumerator PicklePopup_co()
    {
        ShowBackground();

        transform.position = cameraPoint.position;
        transform.rotation = cameraPoint.rotation;

        pickle.SetActive(true);
        float elapsedTime = 0f;

        while (elapsedTime < popupSpeed)
        {
            elapsedTime += Time.deltaTime;
            pickle.transform.position = Vector3.Lerp(pickle.transform.position, destPos.position, elapsedTime / popupSpeed);

            yield return null;
        }

        pickle.transform.position = destPos.position;

        yield return new WaitForSeconds(popupTime);

        sceneEffect.ShortFadeOut();
        yield return new WaitForSeconds(popupTime);

        gameOverUI.SetActive(true);
        restartUI.SetActive(true);
    }

    public void aaaaaa()
    {
        GameManager.Instance.ChangeState(GameState.Dead);
    }

    private void HideBackground()
    {
        startColor = gameOverPanel.color;
        startColor.a = 0f;
        gameOverPanel.color = startColor;
    }

    private void ShowBackground()
    {
        startColor = gameOverPanel.color;
        startColor.a = 60f/255f;
        gameOverPanel.color = startColor;
    }
}
