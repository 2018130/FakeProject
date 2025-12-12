using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UI;

public class LabelReferences : MonoBehaviour
{
    public Text DebugText;
    // 어드레서블의 Label을 얻어올 수 있는 필드.
    public AssetLabelReference assetLabel;
    // 경로 캐싱.
    private IList<IResourceLocation> _locations;
    // 생성된 게임오브젝트를 Destroy하기 위해 참조값을 캐싱한다.
    private List<GameObject> _gameObjects = new List<GameObject>();

    public void GetLocations()
    {
        Addressables.ClearDependencyCacheAsync(assetLabel.labelString);

        Addressables.GetDownloadSizeAsync(assetLabel.labelString).Completed +=
            (handle) =>
            {
                Debug.Log("Size : " + handle.Result);

                Addressables.LoadResourceLocationsAsync(assetLabel.labelString).Completed +=
                    (handle) =>
                    {
                        _locations = handle.Result;
                        DebugText.text = _locations.Count.ToString();

                        Instantiate();
                    };
            };
    }

    public void Instantiate()
    {
        for(int i = 0; i < _locations.Count; i++)
        {
            var location = _locations[i];

            // 경로를 인자로 GameObject를 생성한다.
            // 메모리에 GameObject가 로드된다.
            Addressables.InstantiateAsync(location).Completed +=
                (handle) =>
                {
                // 생성된 개체의 참조값 캐싱
                _gameObjects.Add(handle.Result);
                    if (_gameObjects.Count != 0)
                    {
                        DebugText.text = _gameObjects[_gameObjects.Count - 1].name + " create object";
                    }
                };
        }
    }

    public void Destroy()
    {
        if (_gameObjects.Count == 0)
            return;

        var index = _gameObjects.Count - 1;
        // InstantiateAsync <-> ReleaseInstance
        // ref count가 0이면 메모리에 GameObject가 언로드된다.
        Addressables.ReleaseInstance(_gameObjects[index]);
        _gameObjects.RemoveAt(index);
    }
}
