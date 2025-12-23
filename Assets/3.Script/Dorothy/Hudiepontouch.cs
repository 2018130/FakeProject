using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
public class Hudiepontouch : MonoBehaviour
{
    public CanvasGroup mainUI; // Á¦¾îÇÒ UI±×·ì
    public GameObject blackOverlay; //½Ã²¨¸Õ°Å
    private bool isUIActive = false;

    public Animator blackScreenAnimator; //¾Ö´Ï¸ÞÀÌÅÍ 
    

    private void Start()
    {
        blackScreenAnimator = blackOverlay.GetComponent<Animator>();
        HideUI();
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            if(!isUIActive)
            {
                ShowUI();
            }
            else
            {
                //´Ù½Ã ¶£ÁãÇÏ¸é ²¨Áü
                //HideUI()
            }
        }
    }

    public void ShowUI()
    {
        Debug.Log("È­¸é ÄÑÁü!!!!!!!!!!!!!!!!!!!!!!!!!!");
        mainUI.alpha = 1f;
        mainUI.interactable = true;
        mainUI.blocksRaycasts = true;

        if(blackScreenAnimator!=null)
        {
            blackScreenAnimator.SetTrigger("DoFade");
        }
        isUIActive = true;
    }

    private IEnumerator DisableBlackOverlay(float delay)
    {
        yield return new WaitForSeconds(delay);
        blackOverlay.SetActive(false);
    }

    public void HideUI()
    {
        mainUI.alpha = 0f;
        mainUI.interactable = false;
        mainUI.blocksRaycasts = false;
        blackOverlay.SetActive(true);
        isUIActive = false;
    }

}
