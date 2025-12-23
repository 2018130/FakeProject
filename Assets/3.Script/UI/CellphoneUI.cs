using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellphoneUI : MonoBehaviour, ISceneContextBuilt
{
    [SerializeField]
    private GameObject cellphoneModel;
    public int Priority { get; set; }

    public void OnSceneContextBuilt()
    {
        GameManager.Instance.CurrentSceneContext.Player.GetComponent<PlayerInput>().OnSettingKeyDowned += Toggle;

    }

    private void Toggle()
    {
        cellphoneModel.SetActive(!cellphoneModel.activeSelf);
    }
}
