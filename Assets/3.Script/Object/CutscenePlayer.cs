using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutscenePlayer : MonoBehaviour, IInteractable
{
    [SerializeField]
    private CutsceneContents cutscene;

    public void Interact()
    {
        cutscene?.PlayCutscene();
    }
}
