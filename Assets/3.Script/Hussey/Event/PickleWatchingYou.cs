using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickleWatchingYou : MonoBehaviour
{
    [SerializeField]
    private Pickle pickle;
    private SceneContext sceneContext;
    [SerializeField] GameObject sponPos;
    [SerializeField] GameObject destPos;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float lookWeight = 1f;

    [SerializeField] Transform headTransform;

    private Animator ani;



    private void Awake()
    {
        sceneContext = FindAnyObjectByType<SceneContext>();
        ani = pickle.GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// pickle must be humanoid type
    /// using avatar mask. separate head and body -> base => body
    /// </summary>
    public void SetLookat()
    {
        ani.SetLookAtWeight(lookWeight);
        ani.SetLookAtPosition(sceneContext.Player.transform.position);
    }

    public void WatchingYou()
    {
        pickle.ShowPickle();
        pickle.SetPos(sponPos.transform.position, destPos.transform);
        pickle.SetSpeed(walkSpeed);
        pickle.StopNav();

        //InvokeRepeating(nameof(SetLookat), 0f, 0.2f);

        pickle.StartNav();


        //SoundManager.Instance.PlaySFX(ESFX.SFX_PickleStep);
        //SoundManager.Instance.PlaySFX(ESFX.SFX_PickleBreath1);
    }

    public void OnAnimatorIK(int layerIndex)
    {
        //ani.SetLookAtWeight(lookWeight);
        //ani.SetLookAtPosition(sceneContext.Player.transform.position);

        Vector3 dir = (sceneContext.Player.transform.position - headTransform.position); // world space target direction
        dir = headTransform.InverseTransformDirection(dir); // to head local space target direction
        Quaternion headRot = Quaternion.LookRotation(dir); // Get local quaternion

        ani.SetBoneLocalRotation(HumanBodyBones.Head, headRot); // apply to bone
    }
}
