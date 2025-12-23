using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 추가
    [SerializeField] private Transform phonePoint;
    public Transform PhonePoint => phonePoint;


    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float runSpeed = 50f;

    [SerializeField] private float lookSpeed = 1f;
    [SerializeField] private float lookDegreeLimit = 80f;

    private PlayerInput playerInput;
    private Rigidbody playerR;
    //private Animator ani;
    [SerializeField]
    private Transform cameraPoint;

    private float mouseX;
    private float mouseY;

    private float xRotation = 0f;
    private float yRotation = 0f;

    private void Awake()
    {
        TryGetComponent(out playerInput);
        TryGetComponent(out playerR);
        //TryGetComponent(out ani);

        //Cursor.lockState = CursorLockMode.Locked;//커서 고정시키는거임
        //Cursor.visible = false;
    }

    private void Update()
    {
        Move();

        mouseX = playerInput.MouseDelta.x * lookSpeed * GameManager.Instance.CurrentSceneContext.GameTimeScale;
        mouseY = playerInput.MouseDelta.y * lookSpeed * GameManager.Instance.CurrentSceneContext.GameTimeScale;
    }

    private void LateUpdate()
    {
        PersonalView();
    }

    private void Move()
    {
        //Vector3 movePos = new Vector3(playerInput.MoveValue.x * moveSpeed * Time.deltaTime, 0, playerInput.MoveValue.y * moveSpeed * Time.deltaTime);//timescale 필요

        Vector3 forward = transform.forward * playerInput.MoveValue.y;
        Vector3 right = transform.right * playerInput.MoveValue.x;

        Vector3 direction = (forward + right).normalized;//normalized = 방향벡터의 크기를 1로 맞춤

        if (playerInput.isRun)
        {
            Vector3 movePos = direction * runSpeed * Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameTimeScale;
            playerR.MovePosition(transform.position + movePos);
        }
        else
        {
            Vector3 movePos = direction * moveSpeed * Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameTimeScale;
            playerR.MovePosition(transform.position + movePos);
        }
    }

    private void PersonalView()
    {
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -lookDegreeLimit, lookDegreeLimit);

        yRotation += mouseX;

        if (cameraPoint != null)
        {
            cameraPoint.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }

        transform.Rotate(Vector3.up * mouseX);
    }
}
