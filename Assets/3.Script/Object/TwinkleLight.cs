using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinkleLight : LightObject
{
    private bool twinkle = false;
    [SerializeField]
    private bool twinkleOnStart = true;
    [SerializeField]
    private float maxLagTime = 0.1f;
    [SerializeField]
    private float minLagTime = 0.1f;

    private void Start()
    {
        if(twinkleOnStart)
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
    }

    private IEnumerator Twinkle_co()
    {
        while(twinkle)
        {
            float lagTime = UnityEngine.Random.Range(minLagTime, maxLagTime);
            yield return new WaitForSeconds(lagTime);

            Toggle();
        }

        TurnOff();
    }
}
