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
    public bool _canMove = true;
    public bool _canMoveLag = true;
    public bool _canJump = true;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public LayerMask platformLayer;
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

    [Header("Animation")]
    public Animator animator;
    [SerializeField] GameObject animatedObject;

    [Header("Other")]
    private InputAction _move;
    private InputAction _jump;
    public SlapFighter playerMovement;
    private InputActionAsset inputActions;
    private InputActionMap player;
    public Transform playerTransform;
    Rigidbody rbody;


    private void Awake()
    {
        animator = animatedObject.GetComponent<Animator>();
        _canMove = true;
        _isDashing = false;
        inputActions = GetComponent<PlayerInput>().actions;
        player = inputActions.FindActionMap("Player");
    }

    private void OnEnable()
    {
        _jump = player.FindAction("Jump");
        _move = player.FindAction("Move");
        player.Enable();
    }

    private void OnDisable()
    {
        player.Enable();
    }

    void Start()
    {
        rbody = transform.root.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_canMove && _canMoveLag)
        {
            movePlayer();
            animator.SetFloat("Move", _move.ReadValue<Vector2>().x); 
        }
        if (_movement.y < 0)
        {
            rbody.velocity = Vector3.ClampMagnitude(rbody.velocity, new Vector3(rbody.velocity.x, Mathf.Abs(175), rbody.velocity.z).magnitude);
        }
        else
        {
            rbody.velocity = Vector3.ClampMagnitude(rbody.velocity, new Vector3(rbody.velocity.x, Mathf.Abs(150), rbody.velocity.z).magnitude);
        }
    }

    private void Update()
    {
        if (isGrounded)
        {
            canDouble = true;
            animator.SetBool("grounded", true);
            animator.SetBool("FreeFall", false);
        }
        else
        {
            animator.SetBool("grounded", false);
            animator.SetBool("FreeFall", true);
        }
        //basic movement
        _movement = _move.ReadValue<Vector2>();
        if (!_isDashing)
        {
            SpeedControl();
        }

        //jump
        if (_jump.triggered && _canJump)
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
            if (Physics.BoxCast(transform.position, castArea, Vector3.down, transform.rotation, sphereCastDistance, groundLayer) || 
                Physics.BoxCast(transform.position, castArea, Vector3.down, transform.rotation, sphereCastDistance, platformLayer))
                {
                 isGrounded = true;
                }
            else
        {
            isGrounded = false;
        }
           // isGrounded = Physics.BoxCast(transform.position, castArea, Vector3.down, transform.rotation,sphereCastDistance, groundLayer);
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
            fall.y = -75f;
        }
        if(fall.y < -150f)
        {
            fall.y = -150f;
        }

        fall.y += gravity * Time.deltaTime;
        rbody.AddForce(fall * Time.deltaTime, ForceMode.VelocityChange);

        turnPlayer();
        
    }

    private void movePlayer()
    {
        _movementDirection = _movement * moveSpeed;
        if (isGrounded)
        {
            if(_movementDirection.y < 0)
            {
                rbody.AddForce(new Vector3(_movementDirection.x, _movementDirection.y, _movementDirection.z), ForceMode.Impulse);
            }
            else
            {
                rbody.AddForce(new Vector3(_movementDirection.x, 0, _movementDirection.z), ForceMode.Impulse);
            }
            
        }
        else if(!isGrounded)
        {
            rbody.AddForce(new Vector3(_movementDirection.x, 0, _movementDirection.z) * airMultiplier, ForceMode.Force);
        }
    }

    private void turnPlayer()
    {
        if (_canMoveLag)
        {
            if (_move.ReadValue<Vector2>().x > 0.4f)
            {
                playerTransform.eulerAngles = new Vector3(0, 90, 0);
            }
            else if (_move.ReadValue<Vector2>().x < -0.4)
            {
                playerTransform.eulerAngles = new Vector3(0, -90, 0);
            }
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
        animator.SetTrigger("Jump");
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
