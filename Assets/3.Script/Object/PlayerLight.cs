using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : LightObject
{
    private void Start()
    {
        FindAnyObjectByType<PlayerInput>().OnLightKeyDowned += Toggle;
    }
}
