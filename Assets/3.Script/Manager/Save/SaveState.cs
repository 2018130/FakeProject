using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveState : MonoBehaviour, ISceneContextBuilt
{
    public int Priority { get; set; }

    [SerializeField]
    private string stateKey = "test";

    // public 프로퍼티를 사용하여 Editor 스크립트가 private 필드에 접근하도록 합니다.
    public string StateKey => stateKey;

    public void OnSceneContextBuilt()
    {
        Vector3 position = PersistentDataManager.Instance.GetDataWithParsing<Vector3>(StateKey, transform.position);
        transform.position = position;
        Debug.Log(transform.position);

        PersistentDataManager.Instance.OnBeforeSaveDataToFile += SaveInfomation;
    }

    public void SaveInfomation()
    {
        Debug.Log("Player position info saved");
        PersistentDataManager.Instance.SaveData(StateKey, transform.position);
    }
}
