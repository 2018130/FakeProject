using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float runSpeed = 100f;

    private PlayerInput playerInput;
    private Rigidbody playerR;
    //private Animator ani;

    private void Awake()
    {
        TryGetComponent(out playerInput);
        TryGetComponent(out playerR);
        //TryGetComponent(out ani);
    }

    private void Update()
    {
        Move();
        Aim();
    }

    private void Move()
    {
        //Vector3 movePos = new Vector3(playerInput.MoveValue.x * moveSpeed * Time.deltaTime, 0, playerInput.MoveValue.y * moveSpeed * Time.deltaTime);//timescale 필요
        Vector3 forward = transform.forward * playerInput.MoveValue.y;
        Vector3 right = transform.right * playerInput.MoveValue.x;
        Vector3 direction = (forward + right).normalized;
        Vector3 movePos = direction * moveSpeed * Time.deltaTime;// * GameManager.Instance.CurrentSceneContext.GameDeltaTime;
        playerR.MovePosition(transform.position + movePos);
        //ani.걷기
    }

    private void Run(bool isRun)
    {
        //ani.SetBool("Run", isRun);
        Vector3 runPos = new Vector3(playerInput.MoveValue.x * runSpeed * Time.deltaTime, 0, playerInput.MoveValue.y * runSpeed * Time.deltaTime);//timescale 필요
        playerR.MovePosition(transform.position + runPos);

    }

    private void Aim()
    {
        Ray ray = Camera.main.ScreenPointToRay(playerInput.MousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
    }

    private void FollowingCamera()
    {

    }
}
