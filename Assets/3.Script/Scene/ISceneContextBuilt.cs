using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneContextBuilt
{
    public int Priority { get; set; }

    public void OnSceneContextBuilt();
}
