using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "interact KEY",menuName = "interaction UI Date")]
public class UIData : ScriptableObject
{
    /*
      
         ^---^
        ( 0*0 ) |
         (       )
           u u u u
     
    */

    public string UIname; // UI이름

    public string UImsg; // UI에 띄울 내용

}
