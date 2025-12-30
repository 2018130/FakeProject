using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
public class JumpScare : MonoBehaviour
{
    public ESFX x;
    public GameObject scareob; //나타날거
    public Transform playerCamera; //카메라 흔들거
    private CinemachineImpulseSource impulseSource;


    [Header("흔들림설정")]
    public float shakeDuration = 0.5f; //흔들시간

    private bool isTriggered = false;
    private Vector3 originalpos;



    private void Start()
    {
        impulseSource = playerCamera.GetComponent<CinemachineImpulseSource>();

        if (scareob != null)
            scareob.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")&&!isTriggered)
        {
            isTriggered = true;
            StartCoroutine(TriggerScare());
        }
    }

    IEnumerator TriggerScare()
    {
        
        //갑툭튀 등장 및 사운드 재생
        if (scareob != null)
            scareob.SetActive(true);
        if (scareob != null)
            SoundManager.Instance.PlaySFX(x);

        //카메라 흔들기
        if (impulseSource!=null)
        {
            impulseSource.GenerateImpulse();
        }


        yield return new WaitForSeconds(shakeDuration);
        if (scareob != null)
            scareob.SetActive(false);
    }
}
