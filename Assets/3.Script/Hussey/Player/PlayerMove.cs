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
    [SerializeField] private float pitchDegreeLimit = 80f;

    [Header("Player Cam UI Setting")]
    [SerializeField] private float camLookSpeed = 1f;
    [SerializeField] private float camLookDegreeLimit = 80f;
    [SerializeField] private float camHorizontalDegreeLimit = 20f;

    private PlayerInput playerInput;
    private Rigidbody playerR;

    //private Animator ani;
    [SerializeField]
    public Transform cameraPoint;

    public Vector3 baseCameraPosition;
    public Quaternion baseCameraRotation;

    private float mouseX;
    private float mouseY;

    private float xRotation = 0f;
    private float yRotation = 0f;

    [SerializeField]
    private bool isMouseStop = true;

    private void Awake()
    {
        TryGetComponent(out playerInput);
        TryGetComponent(out playerR);
        baseCameraPosition = cameraPoint.localPosition;
        baseCameraRotation = cameraPoint.localRotation;
        //TryGetComponent(out ani);

        //Cursor.lockState = CursorLockMode.Locked;//커서 고정시키는거임
        //Cursor.visible = false;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentSceneContext == null)
            return;
        Move();

        mouseX = playerInput.MouseDelta.x * lookSpeed * GameManager.Instance.CurrentSceneContext.GameTimeScale;
        mouseY = playerInput.MouseDelta.y * lookSpeed * GameManager.Instance.CurrentSceneContext.GameTimeScale;
    }

    private void LateUpdate()
    {
        PersonalView();
    }

    private void FixedUpdate()
    {
        //PersonalView();
    }

    private void Move()
    {
        if (GameManager.Instance.GameState == GameState.Playing)
        {
            //Vector3 movePos = new Vector3(playerInput.MoveValue.x * moveSpeed * Time.deltaTime, 0, playerInput.MoveValue.y * moveSpeed * Time.deltaTime);//timescale 필요

            Vector3 forward = transform.forward * playerInput.MoveValue.y;
            Vector3 right = transform.right * playerInput.MoveValue.x;

            Vector3 direction = (forward + right).normalized;//normalized = 방향벡터의 크기를 1로 맞춤
            //Debug.Log(direction);
            if (playerInput.isRun)
            {
                Vector3 movePos = direction * runSpeed * Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameTimeScale;
                playerR.MovePosition(transform.position + movePos);
                //Debug.Log("1111");
            }
            else
            {
                Vector3 movePos = direction * moveSpeed * Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameTimeScale;
                playerR.MovePosition(transform.position + movePos);
                //Debug.Log("2222");
            }
        }
    }

    private void PersonalView()
    {
        if (GameManager.Instance.GameState == GameState.Playing)
        {
            if (isMouseStop)
            {
                cameraPoint.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                xRotation -= lookSpeed * playerInput.MouseDelta.y;
                xRotation = Mathf.Clamp(xRotation, -pitchDegreeLimit, pitchDegreeLimit);


                yRotation += lookSpeed * playerInput.MouseDelta.x;

                //transform.Rotate(Vector3.up * mouseY);
                transform.Rotate(Vector3.up * mouseX);
                cameraPoint.localRotation = Quaternion.Euler(xRotation, 0, 0);
            }
        }
        else if (GameManager.Instance.GameState == GameState.UI)
        {
            xRotation -= playerInput.MoveValue.y;
            xRotation = Mathf.Clamp(xRotation, -camLookDegreeLimit, camLookDegreeLimit);

            yRotation += playerInput.MoveValue.x;
            yRotation = Mathf.Clamp(yRotation, -camHorizontalDegreeLimit, camHorizontalDegreeLimit);

            cameraPoint.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
    }


    public void MouseStop()
    {
        isMouseStop = false;
    }
}
