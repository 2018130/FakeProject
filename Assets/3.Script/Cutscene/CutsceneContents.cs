using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneContents : MonoBehaviour,ISceneContextBuilt
{
    [SerializeField]
    private string playedKey = "CutscenePlayedKey";
    [SerializeField]
    private bool isPlayed = false;
    private bool _isPlaying = false;

    [SerializeField]
    private List<SceneEffect> loadedSceneEffects = new List<SceneEffect>();

    [SerializeField]
    private List<SceneEffect> sceneEffects = new List<SceneEffect>();

    public int Priority { get; set; }

    public void OnSceneContextBuilt()
    {
        isPlayed = PersistentDataManager.Instance.GetDataWithParsing(playedKey, false);

        if(isPlayed)
        {
            PlayLoadedCutscene();
        }
    }

    private void PlayLoadedCutscene()
    {
        StartCoroutine(PlayLoadedSceneEffect_co());
    }

    public void PlayCutscene()
    {
        if (!_isPlaying && !isPlayed)
        {
            Debug.Log("여기 나의 족적을 남기고 간다.... ㅋ");
            StartCoroutine(PlaySceneEffect_co());
        }
        /// kjh
    }

    public IEnumerator PlaySceneEffect_co()
    {
        _isPlaying = true;
        foreach (var sceneEffect in sceneEffects)
        {
            yield return null;

            switch (sceneEffect.Type)
            {
                case SceneEffect.EffectType.ChangeGameState:
                    GameManager.Instance.ChangeState(sceneEffect.GameState);
                    Debug.Log("Change game state by cutscene");
                    break;
                case SceneEffect.EffectType.PlaySoundClip:
                    if (sceneEffect.IsBGM)
                    {
                        // TODO : play sound from sound clip
                        //SoundManager.Instance.PlayBGM(EBGM.BGM_2D);
                        Debug.Log("Play bgm by cutscene");
                    }
                    else
                    {
                        //SoundManager.Instance.PlaySFX(ESFX.SFX_PlayerBreath);
                        Debug.Log("Play sfx by cutscene");
                    }
                    break;
                case SceneEffect.EffectType.WaitForSeconds:
                    Debug.Log($"Wait for second '{sceneEffect.Time}'by cutscene");
                    yield return new WaitForSeconds(sceneEffect.Time);
                    break;
                case SceneEffect.EffectType.FunctionCall:
                    Debug.Log($"Calling function by cutscene");
                    sceneEffect.Function?.Invoke();
                    break;
            }
        }
        _isPlaying = false;
        isPlayed = true;
        PersistentDataManager.Instance.SaveData(playedKey, isPlayed);
    }
    public IEnumerator PlayLoadedSceneEffect_co()
    {
        _isPlaying = true;
        foreach (var sceneEffect in loadedSceneEffects)
        {
            yield return null;

            switch (sceneEffect.Type)
            {
                case SceneEffect.EffectType.ChangeGameState:
                    GameManager.Instance.ChangeState(sceneEffect.GameState);
                    Debug.Log("Change game state by cutscene");
                    break;
                case SceneEffect.EffectType.PlaySoundClip:
                    if (sceneEffect.IsBGM)
                    {
                        // TODO : play sound from sound clip
                        //SoundManager.Instance.PlayBGM(EBGM.BGM_2D);
                        Debug.Log("Play bgm by cutscene");
                    }
                    else
                    {
                        //SoundManager.Instance.PlaySFX(ESFX.SFX_PlayerBreath);
                        Debug.Log("Play sfx by cutscene");
                    }
                    break;
                case SceneEffect.EffectType.WaitForSeconds:
                    Debug.Log($"Wait for second '{sceneEffect.Time}'by cutscene");
                    yield return new WaitForSeconds(sceneEffect.Time);
                    break;
                case SceneEffect.EffectType.FunctionCall:
                    Debug.Log($"Calling function by cutscene");
                    sceneEffect.Function?.Invoke();
                    break;
            }
        }
        _isPlaying = false;
        isPlayed = true;
        PersistentDataManager.Instance.SaveData(playedKey, isPlayed);
    }
}
