using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfRoad : MonoBehaviour
{
    private Pickle pickle;
    private SceneContext sceneContext;

    [SerializeField] private Transform sponPos;
    [SerializeField] private float pickleSpeed = 5f;

    private void Awake()
    {
        pickle = FindAnyObjectByType<Pickle>();
        sceneContext = FindAnyObjectByType<SceneContext>();
    }

    public void WhenYouArrived()
    {
        pickle.SetPos(sponPos.position, sceneContext.transform);
        pickle.SetSpeed(pickleSpeed);
        pickle.ShowPickle();
        pickle.StartNav();

        SoundManager.Instance.PlaySFX(ESFX.SFX_PickleRun);
        SoundManager.Instance.PlaySFX(ESFX.SFX_PickleCry1);
    }
}
