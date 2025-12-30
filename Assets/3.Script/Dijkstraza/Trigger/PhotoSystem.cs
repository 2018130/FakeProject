using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class PhotoSystem : MonoBehaviour
{
    public Camera photoCamera;     // 렌더 텍스처용 카메라
    public string StatueTag = "Statue"; // 판정할 대상의 태그 (예: Pickle)
    public Vector3 boxHalfExtents = new Vector3(0.5f, 0.5f, 0.1f); // 박스 크기의 절반 (가로, 세로, 두께)
    public float maxDistance = 50f; // 사진 찍히는 최대 거리
    public LayerMask layerMask;

    private PickleBehindYou pickleBehindYou;

    private void Start()
    {
        pickleBehindYou = GetComponent<PickleBehindYou>();
    }
    public void TakePhoto()
    {
        RaycastHit hit;
        // 카메라의 위치에서 앞방향(forward)으로 박스를 발사합니다.
        // photoCamera.transform.rotation을 사용해 카메라가 보는 각도 그대로 박스를 날립니다.
        bool isHit = Physics.BoxCast(photoCamera.transform.position, boxHalfExtents, photoCamera.transform.forward,out hit, photoCamera.transform.rotation, maxDistance, layerMask);

        if (isHit)
        {
            // 맞은 대상의 정보를 가져와서 태그나 이름을 비교합니다.
            if (hit.collider.CompareTag(StatueTag))
            {
                Debug.Log($"{hit.collider.name} 포착! 이벤트를 실행합니다.");
                ExecuteEvent();
            }
            else
            {
                Debug.Log($"대상이 아닙니다. 맞은 물체: {hit.collider.tag}");
            }
        }
        else
        {
            Debug.Log("아무것도 찍히지 않았습니다.");
        }
    }

    void ExecuteEvent()
    {
        // 다음 이벤트 로직 (예: 컷신 재생, 점수 획득 등)
        pickleBehindYou.Start_Co();
        SoundManager.Instance.PlayBGM(EBGM.BGM_3Dchase);
        SoundManager.Instance.PlaySFX(ESFX.SFX_DropDeadBody);
        GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        Debug.Log("피클을 소환하라!");

    }

    // 에디터 뷰에서 박스 레이가 어디로 나가는지 시각적으로 보여줍니다.
    void OnDrawGizmos()
    {
        if (photoCamera == null) return;
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(photoCamera.transform.position + photoCamera.transform.forward * (maxDistance / 2), photoCamera.transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(boxHalfExtents.x * 2, boxHalfExtents.y * 2, maxDistance));
    }
}