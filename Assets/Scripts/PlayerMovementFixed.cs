using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementFixed : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    Vector3 _movement;
    Vector3 _movementDirection;
    public bool _isDashing;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public bool isGrounded;
    public float groundDrag = 5f;
    [SerializeField] private Vector3 castArea = new Vector3(0, 0, 0);
    [SerializeField] private float sphereCastDistance = .2f;

    [Header("Jumping")]
    public float jumpForce;
    public float airMultiplier;
    public float gravity = -75f;
    Vector3 fall;
    bool canDouble;


    private InputAction _move;
    private InputAction _jump;
    public SlapFighter playerMovement;
    Rigidbody rbody;


    private void Awake()
    {
        _isDashing = false;
        playerMovement = new SlapFighter();
    }

    private void OnEnable()
    {
        _move = playerMovement.Player.Move;
        _jump = playerMovement.Player.Jump;
        _jump.Enable();
        _move.Enable();
    }

    private void OnDisable()
    {
        _jump.Disable();
        _move.Disable();
    }

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
       movePlayer(); 
    }

    private void Update()
    {
        if (isGrounded)
        {
            canDouble = true;
        }
        //basic movement
        _movement = _move.ReadValue<Vector2>();
        if (!_isDashing)
        {
            SpeedControl();
        }

        //jump
        if (_jump.triggered)
        {
            if(isGrounded)
            {
                Jump();
            }
            else if (canDouble)
            {
                canDouble = false;
                Jump();
            }
            
        }

            //grounded check and drag
            isGrounded = Physics.BoxCast(transform.position, castArea, Vector3.down, transform.rotation,sphereCastDistance, groundLayer);
        if (isGrounded)
        {
            rbody.drag = groundDrag;
        }
        else
        {
            rbody.drag = 0;
        }

        if (isGrounded && fall.y < 0)
        {
            fall.y = -100f;
        }
        fall.y += gravity * Time.deltaTime;
        rbody.AddForce(fall * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void movePlayer()
    {
        _movementDirection = _movement * moveSpeed;
        if (isGrounded)
        {
            rbody.AddForce(new Vector3(_movementDirection.x, 0, _movementDirection.z), ForceMode.Impulse);
        }
        else if(!isGrounded)
        {
            rbody.AddForce(new Vector3(_movementDirection.x, 0, _movementDirection.z) * airMultiplier, ForceMode.Force);
        }
        
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rbody.velocity.x, 0f, 0f);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rbody.velocity = new Vector3(limitedVel.x, rbody.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        if (canDouble)
        {
            canDouble = false;
        }
        rbody.velocity = new Vector3(rbody.velocity.x, 0f, rbody.velocity.z);

        rbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }


    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color32(0, 255, 0, 200);
        Gizmos.DrawWireCube(transform.position - new Vector3(0, sphereCastDistance, 0), castArea);
    }
}
