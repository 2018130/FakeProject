using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinkleLight : LightObject, ISceneContextBuilt
{
    private bool twinkle = false;
    [SerializeField]
    private bool twinkleOnStart = true;
    [SerializeField]
    private float maxLagTime = 0.1f;
    [SerializeField]
    private float minLagTime = 0.1f;

    /// <summary>
    /// 전봇대 깜빡이는 이벤트용 스크립트
    /// </summary>
    private string twinkleLightKey = "twinkleLightKey";

    public override void OnSceneContextBuilt()
    {
        twinkle = PersistentDataManager.Instance.GetDataWithParsing(twinkleLightKey, false);

        if (twinkle)
        {
            TurnOn();
        }

        if (twinkleOnStart)
        {
            TurnOn();
        }
    }

    public override void TurnOn()
    {
        base.TurnOn();

        twinkle = true;
        StopAllCoroutines();
        StartCoroutine(Twinkle_co());


    }

    public void StopTwinkle()
    {
        twinkle = false;
        /// kjh
        PersistentDataManager.Instance.SaveData(twinkleLightKey, twinkle);
        Debug.Log("전봇대 twinkle값을 저장함");
    }

    private IEnumerator Twinkle_co()
    {
        while (twinkle)
        {
            float lagTime = UnityEngine.Random.Range(minLagTime, maxLagTime);
            yield return new WaitForSeconds(lagTime);

            Toggle();
        }

        TurnOff();
    }
}
