using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameSceneManager : MonoBehaviour
{
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

    [SerializeField] private Volume postProcessVolume;

    private ChromaticAberration chromaticAberration;
    private FilmGrain filmGrain;
    private LensDistortion lensDistortion;

    [SerializeField] private float glitchTime = 0.5f;

    private void Awake()
    {
        Debug.Log("call awake");
        pickle.SetActive(false);
        gameOverUI.SetActive(false);
        restartUI.SetActive(false);
        isDeadScene = false;

        HideBackground();
    }

    private void Start()
    {
        postProcessVolume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
        postProcessVolume.profile.TryGet<FilmGrain>(out filmGrain);
        postProcessVolume.profile.TryGet<LensDistortion>(out lensDistortion);
    }

    private void Update()
    {
        if (!isDeadScene)
        {
            if (GameManager.Instance.GameState == GameState.Dead)
            {
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


        yield return StartCoroutine(GlitchEffect_co(glitchTime));
        //yield return new WaitForSeconds(popupTime);

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
        startColor.a = 50f / 255f;
        gameOverPanel.color = startColor;
    }

    private IEnumerator GlitchEffect_co(float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (chromaticAberration != null) chromaticAberration.intensity.value = 1.0f;
            if (filmGrain != null) filmGrain.intensity.value = Random.Range(0.5f, 0.8f);
            //if (lensDistortion != null) lensDistortion.intensity.value = Random.Range(-0.5f, 0.5f);

            yield return new WaitForSeconds(0.01f);
        }
    }

    private void ResetPostProcess()
    {
        if (chromaticAberration != null) chromaticAberration.intensity.value = 0f;
        if (filmGrain != null) filmGrain.intensity.value = 0.13f;
    }
}
