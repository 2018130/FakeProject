using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class SceneContext : MonoBehaviour
{
    [SerializeField]
    private float gameTimeScale = 1f;
    public float GameTimeScale => gameTimeScale;

    [SerializeField]
    private AssetLabelReference assetLable;

    [SerializeField]
    private PlayerMove player;
    public PlayerMove Player => player;

    [Header("Fade in/out")]
    [SerializeField]
    private Image fadePanel;


    public void Initialize()
    {
        player = FindAnyObjectByType<PlayerMove>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Fade(3, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Fade(3, false);
        }
    }
    public void Fade(float time, bool isFadeIn = true)
    {
        StartCoroutine(Fade_co(time, isFadeIn));
    }

    private IEnumerator Fade_co(float time, bool isFadeIn = true)
    {
        float timer = 0f;
        float startAlpha = isFadeIn ? 1 : 0;
        float endAlpha = isFadeIn ? 0 : 1;

        Color startColor = fadePanel.color;
        startColor.a = startAlpha;
        fadePanel.color = startColor;

        while(timer <= time)
        {
            timer += Time.deltaTime;
            yield return null;

            Color newColor = startColor;
            newColor.a = Mathf.Lerp(startAlpha, endAlpha, timer / time);

            fadePanel.color = newColor;
        }

        startColor.a = endAlpha;
        fadePanel.color = startColor;
    }
}