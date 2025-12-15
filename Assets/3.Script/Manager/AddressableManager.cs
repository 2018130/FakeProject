using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

public class AddressableManager : SingletonBehaviour<AddressableManager>
{
    private AssetLabelReference currentAssetLabel = new AssetLabelReference();

    private IList<IResourceLocation> _locations;

    private List<GameObject> _gameObjects = new List<GameObject>();

    public bool Initialized => _locations != null && _locations.Count != 0;

    public void SetAssestLabel(AssetLabelReference assetLabel)
    {
        if (currentAssetLabel.labelString == assetLabel.labelString)
            return;

        currentAssetLabel.labelString = assetLabel.labelString;

        if(_locations != null && _locations.Count > 0)
        {
            _locations.Clear();
        }
    }

    public IEnumerator GetLocations_co()
    {
        var op = Addressables.LoadResourceLocationsAsync(currentAssetLabel.labelString);

        while (op.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.None)
        {
            yield return null;
        }

        _locations = op.Result;
    }

    /// <summary>
    /// 비동기 방식으로 서버에 있는 게임오브젝트를 불러옴
    /// 사용 전 경로를 불러와야 하기에 GetLocations()를 호출해 주어야 함
    /// </summary>
    /// <returns></returns>
    public IEnumerator Instantiate_co(string objectKey)
    {
        if (_locations == null || _locations.Count <= 0)
        {
            Debug.Log($"Addressable location not reloaded");
            yield break;
        }

        IResourceLocation resourceLocation = GetIResourceLocation(objectKey);

        if(resourceLocation != null)
        {
            var op = Addressables.InstantiateAsync(resourceLocation, Vector3.zero, Quaternion.identity);

            while (op.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.None)
            {
                yield return null;
            }

            _gameObjects.Add(op.Result);

            Debug.Log($"Spawn gameobject from aws server, created {op.Result}");
        }
    }

    public GameObject Instantiate(string objectKey)
    {
        if (_locations == null || _locations.Count <= 0)
        {
            Debug.Log($"Addressable location not reloaded");
            return null;
        }

        IResourceLocation resourceLocation = GetIResourceLocation(objectKey);

        if (resourceLocation != null)
        {
            var clone = Addressables.InstantiateAsync(resourceLocation, Vector3.zero, Quaternion.identity).WaitForCompletion();
            _gameObjects.Add(clone);
            Debug.Log($"Spawn gameobject from aws server, created {clone}");

            return clone;
        }

        return null;
    }

    private IResourceLocation GetIResourceLocation(string key)
    {
        if (_locations == null)
        {
            Debug.Log($"Not initialized location");
            return default;
        }

        for(int i = 0; i < _locations.Count; i++)
        {
            if(_locations[i].PrimaryKey == key)
            {
                return _locations[i];
            }
        }

        return default;
    }

    public void Release()
    {
        if (_gameObjects.Count == 0)
            return;

        int index = _gameObjects.Count - 1;
        Addressables.ReleaseInstance(_gameObjects[index]);
        _gameObjects.RemoveAt(index);
    }
}