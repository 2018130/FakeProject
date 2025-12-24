using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPhone : MonoBehaviour, IInteractable, ISceneContextBuilt
{
    public int Priority { get; set; }
    [SerializeField]
    private bool obtained = false;
    public bool IsObtained => obtained;
    [SerializeField]
    private string cellphoneObtainKey = "CellphoneObtained";


    public void OnSceneContextBuilt()
    {
        obtained = PersistentDataManager.Instance.GetDataWithParsing(cellphoneObtainKey, false);

        if(obtained)
        {
            Interact();
        }
    }

    public void Interact()
    {
        if(TryGetComponent(out FlutterEffect flutterEffect))
        {
            flutterEffect.StartFluttering();
        }

        transform.SetParent(GameManager.Instance.CurrentSceneContext.Player.PhonePoint);
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;

        GetComponentInChildren<TwinkleLight>().StopTwinkle();
    }

}
