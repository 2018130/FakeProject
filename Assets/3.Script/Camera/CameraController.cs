using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineBrain brain;
    [SerializeField]
    private CinemachineCamera playerAimCamera;
    [SerializeField]
    private CinemachineCamera freeLookCamera;
    [SerializeField]
    private CinemachineCamera startFreeLookCamera;
    [SerializeField]
    private CinemachineCamera dollyCamera;

    private Volume _volume;

    [Space]
    [Header("Vignette")]
    [SerializeField]
    private int maxBlinkCount = 5;
    [SerializeField]
    private float blinkDuration = 1;
    [SerializeField]
    private float blinkDurationWeightPerCycle = 0.2f;
    [SerializeField]
    private float blinkLagTime = 1f;

    private void Start()
    {
        brain = GetComponent<CinemachineBrain>();
        _volume = GetComponent<Volume>();
        foreach (var cam in FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None))
        {
            if(cam.name.Contains("Cinemachine"))
            {
                playerAimCamera = cam;
            }
            else if(cam.name.Contains("PickleLookCamera"))
            {
                freeLookCamera = cam;
            }
            else if(cam.name.Contains("Start"))
            {
                startFreeLookCamera = cam;
            }
            else if (cam.name.Contains("Dolly"))
            {
                dollyCamera = cam;
            }
        }
    }

    public void ChangeCameraToPlayerAimed()
    {
        if (playerAimCamera == null)
            return;

        playerAimCamera.Priority = 1;
        freeLookCamera.Priority = 0;
    }

    public void ChangeCameraToFreeLook()
    {
        if (freeLookCamera == null)
            return;

        freeLookCamera.Priority = 1;
    }

    public void ChangeCameraToStartFreeLook()
    {
        if (startFreeLookCamera == null)
            return;
        startFreeLookCamera.Priority = 1;
    }
    public void ChangeCameraToDolly()
    {
        if (dollyCamera == null)
            return;

        dollyCamera.Priority = 1;
    }

    public void StartDolly()
    {
        dollyCamera.GetComponent<CinemachineSplineDolly>().AutomaticDolly.Enabled = true;
    }

    public void Blink()
    {
        StartCoroutine(Blink_co());
    }

    private IEnumerator Blink_co()
    {
        float maxVignetteIntensity = 1f;
        float minVignetteIntensity = 0.3f;
        for (int i = 0; i < maxBlinkCount; i++)
        {
            //´« ¶ß±â
            yield return SetVignette_co(maxVignetteIntensity, minVignetteIntensity, blinkDuration * MathF.Pow(blinkDurationWeightPerCycle, i + 1));
            //´« °¨±â
            yield return SetVignette_co(minVignetteIntensity, maxVignetteIntensity, blinkDuration * MathF.Pow(blinkDurationWeightPerCycle, i + 1));

            yield return new WaitForSeconds(blinkLagTime);
        }
        //´« ¶ß±â
        yield return SetVignette_co(maxVignetteIntensity, minVignetteIntensity, blinkDuration * MathF.Pow(blinkDurationWeightPerCycle, maxBlinkCount));
    }

    private IEnumerator SetVignette_co(float startValue, float endValue, float duration)
    {
        float time = 0f;
        

        if (_volume.profile.TryGet(out Vignette vignette))
        {
            while (time <= duration)
            {
                time += Time.deltaTime;
                   yield return null;

                vignette.intensity.value = Mathf.Lerp(startValue, endValue, time / duration);
            }
        }

        vignette.intensity.value = endValue;
    }
}
