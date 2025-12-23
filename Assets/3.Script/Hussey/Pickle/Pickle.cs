using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pickle : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private Transform target;
    [SerializeField] private float pickleSpeed = 3f;

    private void Awake()
    {
        TryGetComponent(out agent);
    }

    private void OnEnable()
    {
        agent.ResetPath();
        StopNav();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    //private void Start()
    //{
    //    agent.ResetPath();
    //    StopNav();
    //}

    public void SetPos(Vector3 sponPos, Transform target)
    {
        this.target = target;

        if (!NavMesh.SamplePosition(sponPos, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            return;
        }

        agent.Warp(hit.position);

        StartNav();

        InvokeRepeating(nameof(UpdatePath), 0f, 0.2f);
    }

    private void UpdatePath()
    {
        if (target == null) return;
        if (!agent.isOnNavMesh) return;

        agent.SetDestination(target.position);
    }

    //public IEnumerator Setpath()
    //{
    //    yield return null;
    //    agent.SetDestination(target.position);
    //}

    public void StartNav()
    {
        agent.isStopped = false;
    }

    public void StopNav()
    {
        agent.isStopped = true;
    }

    public void SetSpeed(float pickleSpeed)
    {
        agent.speed = pickleSpeed;
    }

    public void HidePickle()
    {
        if (!transform.GetChild(0).gameObject.activeSelf) return;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ShowPickle()
    {
        if (transform.GetChild(0).gameObject.activeSelf) return;
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
