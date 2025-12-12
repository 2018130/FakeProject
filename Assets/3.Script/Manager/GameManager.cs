using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : SingletonBehaviour<GameManager>
{
    [SerializeField]
    private string defaultSceneContextPath = "DefaultSceneContext";

    // 게임 씬 호출 이후 무조건 초기화 되어 있어야 함
    private SceneContext currentSceneContext;
    public SceneContext CurrentSceneContext => currentSceneContext;

    public IEnumerator Initialize()
    {
        currentSceneContext = FindAnyObjectByType<SceneContext>();

        if(currentSceneContext == null)
        {
            yield return AddressableManager.Instance.Instantiate();
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
