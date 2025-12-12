using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

public class AddressableManager : SingletonBehaviour<AddressableManager>
{
    [SerializeField]
    private AssetLabelReference assetLabel;

    private IList<IResourceLocation> _locations;

    private List<GameObject> _gameObjects = new List<GameObject>();

    private IEnumerator GetLocations()
    {
        var op = Addressables.LoadResourceLocationsAsync(assetLabel.labelString);

        while(op.Status != UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.None)
        {
            yield return null;
        }
        Debug.Log(op.Result.Count);
        _locations = op.Result;
    }

    public IEnumerator Instantiate()
    {
        yield return GetLocations();

        var op = Addressables.InstantiateAsync(_locations[0], Vector3.zero, Quaternion.identity);

        while(op.Status != UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.None)
        {
            yield return null;
        }

        _gameObjects.Add(op.Result);
        foreach(var g in _gameObjects)
        {
            Debug.Log(g);
        }
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
