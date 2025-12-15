using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SceneContext : MonoBehaviour
{
    [SerializeField]
    private float gameTimeScale = 1f;
    public float GameTimeScale => gameTimeScale;

    [SerializeField]
    private AssetLabelReference assetLable;


    public void Initialize()
    {

    }
}