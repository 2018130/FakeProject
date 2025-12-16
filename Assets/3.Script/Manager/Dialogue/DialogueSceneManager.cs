using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSceneManager : MonoBehaviour
{
    public static DialogueSceneManager Instance;

    private int minFavorability = -50;
    private int maxFavorability = 100;
    private int favorability = 30;
    public int Favorability => favorability;

    public event Action<float, float> OnFavorabilityChanged;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetFavorability(int value)
    {
        favorability = Mathf.Clamp(value, minFavorability, maxFavorability);
        OnFavorabilityChanged?.Invoke(favorability - minFavorability, maxFavorability - minFavorability);
    }
}
