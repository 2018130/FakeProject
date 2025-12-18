using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;
    

    [Header("Overlap Check Variable")]
    [SerializeField]
    private float interactableDistance = 3f;
    [SerializeField]
    private string tagName;
    [SerializeField]
    private LayerMask interactLayer;
    [SerializeField]
    private float overlapChcekLagTime = 0.5f;

    private Camera _camera;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        playerInput.OnInteractionDowned += OverlapCheck;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OverlapCheck()
    {
        StartCoroutine(OverlapCheckCoroutine());
    }

    private IEnumerator OverlapCheckCoroutine()
    {
        while (playerInput.InteractionPerformed)
            //플레이어의 인풋이 실행중일때
        {
            Collider[] overlapped = Physics.OverlapSphere(transform.position, interactableDistance, interactLayer);
            // 오버랩스피어가 검출한 하나 이상의 대상을 담아야하므로 collider배열([])에 담아야한다. 단일 자료구조로는 안된다.

            if (overlapped.Length > 0)
                // 담겼다면 그 자료구조의 길이를 쟀을때 0이 아닐테고 음수가 아닐 것이므로 0보다 커야한다.
            {
                IInteractable closest = null;
                // 우리가 정한 인터페이스르 담을 변수 
                float minDistanceSqr = float.MaxValue; // 비교를 위해 최대값으로 초기화

                // 1. 가장 가까운 오브젝트 찾기
                foreach (Collider col in overlapped)
                {
                    //Debug.Log($"검출된 오브젝트 : {col.name}");
                    //origin
                    if (col.TryGetComponent(out IInteractable a))
                    //if (col.TryGetComponent<IInteractable>(out IInteractable a))

                    //sjh 1400
                    // if(col is IInteractable)

                    //kjh 1425    
                    //if (col.GetComponent<IInteractable>()!=null)

                    //kjh 1430
                    //if (col.TryGetComponent<IInteractable>(out IInteractable a))
                    {
                        Debug.Log($"검출된 오브젝트 : {col.name}");
                        // Vector3.Distance 대신 sqrMagnitude를 사용하면 루트 연산을 안해서 더 빠릅니다.
                        float distanceSqr = (col.transform.position - transform.position).sqrMagnitude;
                        //Debug.Log($"검출된 오브젝트 : {col.name}");
                        if (distanceSqr < minDistanceSqr)
                        {
                            minDistanceSqr = distanceSqr;
                            //kjh   1425
                            closest = a;
                        }
                    }
                }

                // 2. 가장 가까운 오브젝트가 Interactable을 상속받고 있다면 Interact() 수행
                ///kjh
               /// if (closest != null)
               /// {
                    // Interactable이 인터페이스(IInteractable)인지 클래스인지에 따라 타입만 변경해서 쓰세요.
                    // 여기서는 안전하고 빠른 TryGetComponent를 사용합니다.
                        closest.Interact();

                        // 상호작용 후 루프를 종료하고 싶다면 break; 추가
                        // 계속 유지해야 한다면 그대로 둠
                 ///   }
                
            }

            yield return new WaitForSeconds(overlapChcekLagTime);
        }
    }
}
