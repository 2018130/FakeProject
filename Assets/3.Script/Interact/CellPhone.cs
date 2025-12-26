using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPhone : MonoBehaviour, IInteractable, ISceneContextBuilt
{
    [SerializeField]
    private Camera cellphoneCamera;
    public int Priority { get; set; }
    [SerializeField]
    private bool obtained = false;
    public bool IsObtained => obtained;
    [SerializeField]
    private string cellphoneObtainKey = "CellphoneObtained";

    [SerializeField]
    private Material screenMaterial;

    public void OnSceneContextBuilt()
    {
        obtained = PersistentDataManager.Instance.GetDataWithParsing(cellphoneObtainKey, false);
        GameManager.Instance.OnChangedGameState += SetActiveScreen;

        if (obtained)
        {
            Interact();
        }
    }

    public void Interact()
    {
        if(!obtained)
        {
            if (TryGetComponent(out FlutterEffect flutterEffect))
            {
                flutterEffect.StartFluttering();
            }

            transform.SetParent(GameManager.Instance.CurrentSceneContext.Player.PhonePoint);
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;

            GetComponentInChildren<TwinkleLight>()?.StopTwinkle();

            PhoneCommuStatusAndBatter_UI phoneCommuStatusAndBatter_UI = FindAnyObjectByType<PhoneCommuStatusAndBatter_UI>();

            phoneCommuStatusAndBatter_UI.Initialize(GetComponent<PlayerLight>());
            PhoneGalleryManager galleryManager = FindAnyObjectByType<PhoneGalleryManager>();
            galleryManager.Initialize(cellphoneCamera);
            obtained = true;
        }
    }

    private void SetActiveScreen(GameState gameState)
    {
        if(gameState == GameState.UI)
        {
            screenMaterial.color = Color.white;
        }
        else
        {
            screenMaterial.color = Color.black;
        }
    }
}
