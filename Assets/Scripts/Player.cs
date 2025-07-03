using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Player : Character
{
    private TheRPGProject CustomInput = null;

    private bool moving = false;
    private Vector3 movementAmount = Vector3.zero;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float JumpPower;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float gravityScale = 1;
    [SerializeField] private static float GlobalGravity = -9.81f;
    [SerializeField] private float SprintMultiplier = 200;
    [SerializeField] private float PlayerSpeed = 0;


    // Start is called before the first frame update
    void Awake()
    {
        CustomInput = new TheRPGProject();
        rb.useGravity = false;
        PlayerSpeed = MoveSpeed;
    }

    private void Start()
    {
        PlayerSpeed = MoveSpeed;
    }

    private void OnEnable()
    {
        CustomInput.Enable();
        CustomInput.Player.Move.performed += MovePlayer;
        CustomInput.Player.Move.canceled += StopMoving;
        CustomInput.Player.Jump.performed += Jump;
        CustomInput.Player.Sprint.performed += StartSprinting;
        CustomInput.Player.Sprint.canceled += StopSprinting;
    }

    private void OnDisable()
    {
        CustomInput.Player.Move.performed -= MovePlayer;
        CustomInput.Player.Move.canceled -= StopMoving;
        CustomInput.Player.Jump.performed -= Jump;
        CustomInput.Player.Sprint.performed -= StartSprinting;
        CustomInput.Player.Sprint.canceled -= StopSprinting;
        CustomInput.Disable();
    }

    void MovePlayer(InputAction.CallbackContext context)
    {
        moving = true;
        Vector2 input =  context.ReadValue<Vector2>();

        movementAmount = new Vector3(input.x, 0, input.y).normalized * PlayerSpeed;
    }

    void StopMoving(InputAction.CallbackContext context)
    {
        moving = false;
        if (Mathf.Abs(rb.velocity.y) <= 0.1)
        {
            rb.velocity = Vector3.zero;
        }
    }

    void StartSprinting(InputAction.CallbackContext context)
    {
        PlayerSpeed = MoveSpeed * SprintMultiplier;
        movementAmount = movementAmount.normalized * PlayerSpeed;
    }

    void StopSprinting(InputAction.CallbackContext context)
    {
        PlayerSpeed = MoveSpeed;
        movementAmount = movementAmount.normalized * PlayerSpeed;
    }

    void Jump(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        Physics.Raycast(groundCheck.position, Vector3.down, out hit, 3, ground);

        if(hit.transform != null)
        {
            rb.AddForce(new Vector3(0, JumpPower, 0));
        }
        else
        {
            Debug.Log(hit.transform);
        }
            
    }

    private void FixedUpdate()
    {
        Vector3 gravity = GlobalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);

        if (!moving) return;

        rb.AddForce(movementAmount * Time.deltaTime);
    }
}
