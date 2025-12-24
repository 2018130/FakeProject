using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ChangeCursor : MonoBehaviour
{
    [SerializeField] private Texture2D meunmenu;

    private void Start()
    {
        Cursor.SetCursor(meunmenu, Vector2.zero, CursorMode.Auto);
    }
}
