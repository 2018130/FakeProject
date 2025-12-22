using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickleWatchingYou : MonoBehaviour
{
    private Pickle pickle;
    private PlayerMove player;
    [SerializeField] GameObject sponPos;
    [SerializeField] GameObject moveTarget;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float lookWeight = 1f;

    private Animator ani;

    private void Awake()
    {
        pickle = FindAnyObjectByType<Pickle>();
        player = FindAnyObjectByType<PlayerMove>();
        TryGetComponent(out ani);
    }

    public void SetLookat()
    {
        ani.SetLookAtWeight(lookWeight);
        ani.SetLookAtPosition(player.transform.position);
    }

    public void WatchingYou()
    {
        pickle.SetPos(sponPos.transform.position, moveTarget.transform);
        pickle.gameObject.SetActive(true);
        pickle.SetSpeed(walkSpeed);
        pickle.StartNav();

        InvokeRepeating(nameof(SetLookat), 0f, 0.2f);
    }
}
