using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PlayerData
{
    public string Name = "TEST";
}


public enum GameState
{
    None,
    Playing,
    UI,
    TimeStop,
    Dead,
}

public class GameManager : SingletonBehaviour<GameManager>
{
    [SerializeField]
    private string defaultSceneContextPath = "SceneContext";

    // 게임 씬 호출 이후 무조건 초기화 되어 있어야 함
    private SceneContext currentSceneContext;
    public SceneContext CurrentSceneContext => currentSceneContext;

    [SerializeField]
    private AssetLabelReference defaultAssetLabel;

    [SerializeField]
    private GameState gameState = GameState.Playing;
    public GameState GameState => gameState;

    public event Action<GameState> OnChangedGameState;

    [Header("Data")]
    private PlayerData playerData;

    private void Start()
    {
        //todo - 로컬의 세이브파일의 데이터를 버퍼로 불러오는 작업
        StartCoroutine(Initialize());
    }

    public void ChangeState(GameState newState)
    {
        Debug.Log($"{gameState}");
        if (gameState == newState)
            return;

        gameState = newState;
        OnChangedGameState?.Invoke(gameState);
        switch (gameState)
        {
            case GameState.None:
                break;
            case GameState.Playing:
                GameManager.Instance.currentSceneContext.GameTimeScale = 1f;
                break;
            case GameState.UI:
                GameManager.Instance.currentSceneContext.GameTimeScale = 0f;
                break;
            case GameState.TimeStop:
                GameManager.Instance.currentSceneContext.GameTimeScale = 0f;
                break;
            case GameState.Dead:
                GameManager.Instance.currentSceneContext.GameTimeScale = 0f;
                break;
        }
        Debug.Log($"{gameState}");
    }

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
            GameObject obj = AddressableManager.Instance.Instantiate(defaultSceneContextPath);
            currentSceneContext = obj.GetComponent<SceneContext>();
        }

        currentSceneContext.Initialize();

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
