using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pickle : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private Transform target;

    private void Awake()
    {
        TryGetComponent(out agent);
    }

    private void OnEnable()
    {
        agent.ResetPath();
        StopNav();
    }

    //private void Start()
    //{
    //    agent.ResetPath();
    //    StopNav();
    //}

    public void SetPos(Vector3 destPos, Transform target)
    {
        this.target = target;

        if (!NavMesh.SamplePosition(destPos, out NavMeshHit hit, 10f, NavMesh.AllAreas))
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
}
