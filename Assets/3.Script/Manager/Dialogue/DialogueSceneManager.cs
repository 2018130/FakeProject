using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DialogueSceneManager : MonoBehaviour
{
    public static DialogueSceneManager Instance;

    private int minFavorability = -50;
    private int maxFavorability = 100;
    private int favorability = 30;
    public int Favorability => favorability;

    [SerializeField]
    private Material glitchShader;

    [Header("Background")]
    [SerializeField]
    private Explodable explodablePanel;
    [SerializeField]
    private GameObject oddBackground;

    [Header("Volume")]
    [SerializeField]
    private VolumeProfile volumeProfile;

    [Header("Error Setting"), Space()]
    [SerializeField]
    private float intensityUpTime = 0.3f;
    [SerializeField]
    private float intensityDownTime = 0.2f;
    [SerializeField]
    private float delay = 1f;
    [SerializeField]
    private int count = 5;
    [SerializeField]
    private float chzzSpeed = 1f;
    [SerializeField]
    private float glitchWeight = 0.1f;

    public event Action<float, float> OnFavorabilityChanged;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        volumeProfile.TryGet(out FilmGrain filmGrain);
        filmGrain.intensity.value = 0f;
    }
    public void SetFavorability(int value)
    {
        favorability = Mathf.Clamp(value, minFavorability, maxFavorability);
        OnFavorabilityChanged?.Invoke(favorability - minFavorability, maxFavorability - minFavorability);
    }

    public void PlayGame()
    {
        StartCoroutine(ChangeTo3DScene());
    }

    private IEnumerator ChangeTo3DScene()
    {
        // 점차 치지직 거리는 효과
        yield return FilmGrain_co();

        SceneChangeManager.Instance.ChangeScene(SceneType.GameScene);
    }
    private void OnApplicationQuit()
    {
        volumeProfile.TryGet(out FilmGrain filmGrain);
        filmGrain.intensity.value = 0f;
    }

    private IEnumerator FilmGrain_co()
    {
        if(volumeProfile.TryGet(out FilmGrain filmGrain))
        {
            for (int i = 0; i < count; i++)
            {
                float time = 0f;
                while (time <= intensityUpTime * (1 + i))
                {
                    time += Time.deltaTime;
                    yield return null;
                    filmGrain.intensity.value += Time.deltaTime * chzzSpeed;
                    ApplayGlitchMaterialToImage(intensityUpTime * (1 + i) * glitchWeight);
                }

                if(i != count - 1)
                {
                    time = 0f;
                    /*
                    while (time <= intensityDownTime * (1 + i))
                    {
                        time += Time.deltaTime;
                        yield return null;
                        filmGrain.intensity.value -= Time.deltaTime * chzzSpeed;
                        ApplayGlitchMaterialToImage(0);
                    }
                    */

                    ApplayGlitchMaterialToImage(0);
                    filmGrain.intensity.value = 0f;
                }

                yield return new WaitForSeconds(delay);
            }

            oddBackground.SetActive(true);
            ExplodeBackground();
        }
    }

    private void ApplayGlitchMaterialToImage(float amount)
    {
        Image[] images = FindObjectsByType<Image>(FindObjectsSortMode.None);

        foreach(var image in images)
        {
            if (amount == 0)
            {
                image.material = null;
            }
            else
            {
                glitchShader.SetFloat("_GlitchIntensity", amount);
                image.material = glitchShader;
            }
        }
    }

    private void ExplodeBackground()
    {
        explodablePanel.explode();
        ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
        ef.doExplosion(transform.position);
    }
}
