using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
public class JumpScare : MonoBehaviour
{
    public ESFX x;
    public GameObject scareob; //³ªÅ¸³¯°Å
    public Transform playerCamera; //Ä«¸Þ¶ó Èçµé°Å
    private CinemachineImpulseSource impulseSource;


    [Header("Èçµé¸²¼³Á¤")]
    public float shakeDuration = 0.5f; //Èçµé½Ã°£
    public float shakeMagnitude = 0.5f;//Èçµé°­µµ

    private bool isTriggered = false;
    private Vector3 originalpos;



    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        SoundManager.Instance.PlaySFX(x);
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
        
        //°©ÅöÆ¢ µîÀå ¹× »ç¿îµå Àç»ý
        if (scareob != null)
            scareob.SetActive(true);
        if (scareob != null)
            SoundManager.Instance.PlaySFX(x);

        //Ä«¸Þ¶ó Èçµé±â
        if(impulseSource!=null)
        {
            impulseSource.GenerateImpulse();
        }


        yield return new WaitForSeconds(1.0f);
        if (scareob != null)
            scareob.SetActive(false);
    }
}
