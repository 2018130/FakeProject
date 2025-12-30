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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) GameOver();
    }

    public void SetTriggerCollider(bool active)
    {
        GetComponent<Collider>().enabled = active;
    }
    public void SetPos(Vector3 sponPos, Transform target)
    {
        this.target = target;

        if (!NavMesh.SamplePosition(sponPos, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            return;
        }

        agent.Warp(hit.position);

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
    //
    /// <summary>
    /// 
    /// </summary>
    /// <param name="time">time 시간 이후 모습이 보이지 않습니다.</param>
    public void HidePickle(float time = 0)
    {
        if (!transform.GetChild(0).gameObject.activeSelf) return;

        StartCoroutine(Hide_co(time));
    }

    private IEnumerator Hide_co(float time)
    {
        yield return new WaitForSeconds(time);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ShowPickle()
    {
        if (transform.GetChild(0).gameObject.activeSelf) return;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void GameOver()
    {
        GameManager.Instance.ChangeState(GameState.Dead);
    }
}
