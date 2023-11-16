using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private PlayerInput playerInput;
    public PlayerInput.PlayerBasicCtrlActions BasicCtrl;

    public bool EnableSprint = false;
    [HideInInspector] public bool isGrounded;
    [Header("Player settings")]
    [HideInInspector] public float speed = 5.0f;
    public float WalkSpeed = 5.0f;
    public float sprintSpeed = 10f;
    private bool isSprinting = false;
    public float gravity = -9.8f;
    public float jumpForce = 0.5f;
    public float groundAcceleration = 5f;
    public float airAcceleration = 1f;
    public bool EnableMidAirControl = false;
    private Vector3 curSpeed;
    private Vector3 VerticalVelocity;
    private bool isCollideAbove;
    [Header("Camera")]

    public bool FirstPlayer = true;
    public bool enableChangingPov = true;
    public Camera cam;
    public GameObject camHolder;
    public float basicPoV = 60f;
    public float SprintPoV = 70f;
    public float PoVChangingSpeed = 2f;
    private PlayerInteract interact;

    private float xRotation = 0f;
    public float xSensitivity = 7f;
    public float ySensitivity = 7f;

    private float lastX;
    private float lastY;
    void Awake()
    {
        playerInput = new PlayerInput();
        BasicCtrl = playerInput.PlayerBasicCtrl;
        
        //setup events
        BasicCtrl.Jump.performed += ctx => PerformJump();
        if (EnableSprint)
        {
            BasicCtrl.Sprint.started += ctx => Sprint(true);
            BasicCtrl.Sprint.canceled += ctx => Sprint(false);
        }
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        BasicCtrl.Enable();
        interact = GetComponent<PlayerInteract>();
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = new Vector3(input.x, 0f, input.y);
        float acceleration = isGrounded ? groundAcceleration : airAcceleration;


        Vector3 currentDirection = transform.TransformDirection(moveDirection);
        curSpeed = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        Vector3 desiredVelocity = currentDirection * speed;

        Vector3 accelerationVector = isGrounded ? (desiredVelocity - curSpeed) * acceleration : desiredVelocity * acceleration;
        Debug.Log(accelerationVector.magnitude);
        curSpeed += accelerationVector * Time.deltaTime;
        //curSpeed = Vector3.ClampMagnitude(curSpeed, speed);

        VerticalVelocity.y += gravity * Time.deltaTime;

        if (isGrounded && VerticalVelocity.y < 0)
        {
            VerticalVelocity.y = -2f;
        }
        if (controller.collisionFlags == CollisionFlags.Above)
        {
            if (!isCollideAbove)
            {
                isCollideAbove = true;
                Debug.Log("Above");
                VerticalVelocity.y = -1f;
            }
        }
        else
        {
            isCollideAbove = false;
        }
        controller.Move(curSpeed * Time.deltaTime + VerticalVelocity * Time.deltaTime);
    }
    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        camHolder.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);


        if (enableChangingPov)
        {
            float targetPoV = isSprinting ? SprintPoV : basicPoV;
            cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, targetPoV, PoVChangingSpeed);
        }
    }
    public void PerformJump()
    {
        if (isGrounded)
        {
            VerticalVelocity.y = Mathf.Sqrt(jumpForce * -3.0f * gravity);
        }
    }
    private void Sprint(bool sprintPerformed)
    {      
        speed = sprintPerformed ? sprintSpeed : WalkSpeed;
        isSprinting = sprintPerformed;
    }

    private void Update()
    {
        isGrounded = controller.isGrounded;
        ProcessMove(BasicCtrl.Movement.ReadValue<Vector2>());
        ProcessLook(BasicCtrl.Look.ReadValue<Vector2>());
    }

}
