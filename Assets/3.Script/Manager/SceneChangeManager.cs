using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    TitleScene,
    DialogueScene,
    GameScene,
    Bootstap,
}

public class SceneChangeManager : SingletonBehaviour<SceneChangeManager>
{
    public void ChangeScene(SceneType sceneType)
    {
        StartCoroutine(ChangeScene_co(sceneType));
    }

    private IEnumerator ChangeScene_co(SceneType sceneType)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync((int)sceneType);

        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            yield return null;

            if (ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }
        }

        yield return GameManager.Instance.Initialize();
    }
}
