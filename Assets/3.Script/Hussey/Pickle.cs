using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pickle : MonoBehaviour
{
    private NavMeshAgent agent;
    //private AudioSource audioPlayer;

    [SerializeField] private Transform target;
    //[SerializeField] private AudioClip[] pickleSound;

    private void Awake()
    {
        TryGetComponent(out agent);
        //TryGetComponent(out audioPlayer);
    }

    private void Start()
    {
        agent.isStopped = true;
    }

    private void Update()
    {
        if (!agent.isStopped && agent.isOnNavMesh)
        {
            Debug.Log("pickle update");
            agent.SetDestination(target.position);
        }
    }

    public void SetPos(Vector3 targetPos, Vector3 gapPos)
    {
        Vector3 destPos = targetPos + gapPos;

        transform.position = new Vector3(destPos.x, 0, destPos.z);
        Debug.Log("setPos");
        //if (agent != null)
        //{
        //    agent.Warp(destPos.x, 0, destPos.z);
        //}
    }

    public void StartNav()
    {
        agent.isStopped = false;
        Debug.Log("nav mesh started");
    }

    public void StopNav()
    {
        agent.isStopped = true;
    }
}
