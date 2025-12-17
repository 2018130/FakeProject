using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlutterEffect : MonoBehaviour, ISceneContextBuilt
{
    private Vector2 orign;

    [SerializeField]
    private float maxFlutterHeight = 0.1f;
    [SerializeField]
    private float maxFlutterWidth = 0.1f;

    [SerializeField]
    private float flutterHeightSpeed = 1f;
    [SerializeField]
    private float flutterWidthSpeed = 1f;

    [SerializeField]
    private float flutterDelay = 0.1f;

    private bool endOfLower = true;

    public int Priority { get; set; }

    public void OnSceneContextBuilt()
    {
        orign = transform.localPosition;

        StartCoroutine(Flutter_co());
    }

    private IEnumerator Flutter_co()
    {
        while (true)
        {
            // 올라감
            if(endOfLower)
            {
                yield return MoveToUpper_co();
            }

            // 내려감
            yield return MoveToLower_co();


            yield return new WaitForSeconds(flutterDelay);
        }
    }

    private IEnumerator MoveToUpper_co()
    {
        while (transform.localPosition.x < orign.x + maxFlutterWidth ||
               transform.localPosition.y < orign.y + maxFlutterHeight)
        {
            yield return null;

            Vector3 newPos = transform.localPosition;
            float newPosX = transform.localPosition.x + flutterWidthSpeed * Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameTimeScale;
            float newPosY = transform.localPosition.y + flutterHeightSpeed * Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameTimeScale;
            
            if (transform.localPosition.x < orign.x + maxFlutterWidth)
            {
                newPos.x = newPosX;
            }
            if (transform.localPosition.y < orign.y + maxFlutterHeight)
            {
                newPos.y = newPosY;
            }
            transform.localPosition = newPos;
        }
    }

    private IEnumerator MoveToLower_co()
    {
        endOfLower = false;
        while (transform.localPosition.x > orign.x ||
                transform.localPosition.y > orign.y)
        {
            yield return null;

            Vector3 newPos = transform.localPosition;
            float newPosX = transform.localPosition.x - flutterWidthSpeed * Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameTimeScale;
            float newPosY = transform.localPosition.y - flutterHeightSpeed * Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameTimeScale;

            if (transform.localPosition.x > orign.x)
            {
                newPos.x = newPosX;
            }
            if (transform.localPosition.y > orign.y)
            {
                newPos.y = newPosY;
            }
            transform.localPosition = newPos;
        }
        endOfLower = true;
    }
}
