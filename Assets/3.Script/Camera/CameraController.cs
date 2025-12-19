using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

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

    private void Start()
    {
        brain = GetComponent<CinemachineBrain>();
        foreach (var cam in FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None))
        {
            if(cam.name.Contains("Cinemachine"))
            {
                playerAimCamera = cam;
            }
            else if(cam.name.Contains("FreeLook"))
            {
                freeLookCamera = cam;
            }
            else if(cam.name.Contains("Start"))
            {
                startFreeLookCamera = cam;
            }
        }
    }

    public void ChangeCameraToPlayerAimed()
    {
        if (playerAimCamera == null)
            return;
        playerAimCamera.Priority = 1;
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
}
