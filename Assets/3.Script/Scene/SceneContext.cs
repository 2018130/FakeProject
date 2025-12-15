using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SceneContext : MonoBehaviour
{
    [SerializeField]
    private float gameDeltaTime = 1f;
    public float GameDeltaTime => gameDeltaTime;

    [SerializeField]
    private AssetLabelReference assetLable;


    public void Initialize()
    {

    }
}