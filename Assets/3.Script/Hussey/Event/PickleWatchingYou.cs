using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickleWatchingYou : MonoBehaviour
{
    private Pickle pickle;
    private SceneContext sceneContext;
    [SerializeField] GameObject sponPos;
    [SerializeField] GameObject destPos;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float lookWeight = 1f;

    private Animator ani;

    private void Awake()
    {
        pickle = FindAnyObjectByType<Pickle>();
        sceneContext = FindAnyObjectByType<SceneContext>();
        TryGetComponent(out ani);
    }

    /// <summary>
    /// pickle must be humanoid type
    /// using avatar mask. separate head and body -> base => body
    /// </summary>
    //public void SetLookat()
    //{
    //    ani.SetLookAtWeight(lookWeight);
    //    ani.SetLookAtPosition(sceneContext.Player.transform.position);
    //}

    public void WatchingYou()
    {
        pickle.SetPos(sponPos.transform.position, destPos.transform);
        pickle.SetSpeed(walkSpeed);
        pickle.ShowPickle();

        //InvokeRepeating(nameof(SetLookat), 0f, 0.2f);

        pickle.StartNav();


        //SoundManager.Instance.PlaySFX(ESFX.SFX_PickleStep);
        //SoundManager.Instance.PlaySFX(ESFX.SFX_PickleBreath1);
    }
}
