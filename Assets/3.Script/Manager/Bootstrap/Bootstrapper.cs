using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod]
    static void GameInitializer()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        SceneManager.LoadScene("BootstrapScene");
        SceneManager.LoadScene(currentScene.name);
    }
}
