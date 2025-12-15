using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : SingletonBehaviour<GameManager>
{
    [SerializeField]
    private string defaultSceneContextPath = "SceneContext";

    // 게임 씬 호출 이후 무조건 초기화 되어 있어야 함
    private SceneContext currentSceneContext;
    public SceneContext CurrentSceneContext => currentSceneContext;

    [SerializeField]
    private AssetLabelReference defaultAssetLabel;

    public IEnumerator Initialize()
    {
        currentSceneContext = FindAnyObjectByType<SceneContext>();

        if(currentSceneContext == null)
        {
            AddressableManager.Instance.SetAssestLabel(defaultAssetLabel);

            yield return AddressableManager.Instance.GetLocations_co();
            // 비동기
            //yield return AddressableManager.Instance.Instantiate_co(defaultSceneContextPath);
            // 동기
            AddressableManager.Instance.Instantiate(defaultSceneContextPath);
        }

        CallOnSceneContextBuilt();
    }

    private void CallOnSceneContextBuilt()
    {
        var allMonoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        ISceneContextBuilt[] sceneContextBuilts = allMonoBehaviours
            .OfType<ISceneContextBuilt>()
            .ToArray();
        sceneContextBuilts = sceneContextBuilts.OrderBy(x => x.Priority).ToArray();

        foreach (var sceneContextBuilt in sceneContextBuilts)
        {
            sceneContextBuilt.OnSceneContextBuilt();
        }
    }
}
