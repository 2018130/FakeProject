using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickleBehindYou : MonoBehaviour
{
    [SerializeField]
    private Pickle pickle;
    private SceneContext scenecontext;
    [SerializeField] GameObject sponPos;
    [SerializeField] private float delay = 3f;
    [SerializeField] private float pickleSpeed = 5f;

    private void Awake()
    {
        pickle.HidePickle();
        scenecontext = FindAnyObjectByType<SceneContext>();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        //플레이어의 뒤쪽으로 나올 경우
    //        Vector3 destPos = -other.transform.forward * 10f;
    //        pickle.gameObject.SetActive(true);
    //        pickle.SetPos(destPos, other.transform);
    //        pickle.StartNav();
    //        gameObject.SetActive(false);
    //    }
    //}

    public IEnumerator PickleInYourBack()
    {
        Vector3 sponPos = this.sponPos.transform.position;
        pickle.ShowPickle();
        pickle.SetPos(sponPos, scenecontext.Player.transform);
        pickle.SetSpeed(pickleSpeed);
        pickle.StopNav();
        yield return new WaitForSeconds(delay);
        pickle.StartNav();

        //SoundManager.Instance.PlaySFX(ESFX.SFX_PickleBreath2);
    }

    public void Start_Co()
    {
        StartCoroutine(PickleInYourBack());
        //pickle.StartNav();
    }
}
