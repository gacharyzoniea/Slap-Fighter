using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    Vector3 _movement;
    Vector3 _currentMovement;

    private float verticalDirection;
    Rigidbody rbody;
    public SlapFighter playerMovement; //input object
    public LayerMask groundLayer;
    private bool isGrounded;
    private bool _changingDirection => (rbody.velocity.x > 0f && _movement.x < 0f) || (rbody.velocity.x < 0f && _movement.x > 0f);

    [Header ("Movement")]
    [SerializeField] float _speed = 6.24f;
    [SerializeField] private float _groundLinearDrag = 2.65f;
    [SerializeField] private float _airLinearDrag = 1f;
    [SerializeField] private float maxSpeed = 10;
    [SerializeField] private float jumpSpeed = 9f;
    [SerializeField] float gravity = 10f;

    [Header("GroundCheck")]
    [SerializeField] float sphereCastRadius = .1f;
    [SerializeField] float sphereCastDistance = .2f;

    [Header ("Jumping")]
    [SerializeField] private float _fallMultiplier = 8f;
    [SerializeField] private float _jumpVelocityFalloff = 8;

    private InputAction _move;
    private InputAction _jump;


    private void Awake()
    {
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
        MoveCharacter();
        if (isGrounded)
        {
            ApplyGroundLinearDrag();
        }
        else
        {
            ApplyAirLinearDrag();
            FallMultiplier();
        }
    }

    

    private void Update()
    {
        verticalDirection = rbody.velocity.y;
        _currentMovement = rbody.velocity;
        //check if grounded
        isGrounded = Physics.SphereCast(transform.position, sphereCastRadius, Vector3.down, out RaycastHit hitInfo, sphereCastDistance, groundLayer);
        print(isGrounded);

        //set movement vector w/ input system
        _movement = _move.ReadValue<Vector2>();
        
        //jump logic
        if (_jump.triggered && isGrounded)
        {
            Jump();
        }

        //clamp max speed to avoid infinite acceleration
        //if(rbody.velocity.x > maxSpeed)
        //{
        //    rbody.velocity = Vector3.ClampMagnitude(rbody.velocity, maxSpeed);
        //}

        rbody.AddForce(Vector3.down * gravity);
    }
    private void MoveCharacter()
    {
        rbody.AddForce(_movement * _speed, ForceMode.VelocityChange);
        if (Mathf.Abs(rbody.velocity.x) > maxSpeed)
            rbody.velocity = new Vector2(Mathf.Sign(rbody.velocity.x) * maxSpeed, rbody.velocity.y);
        
    }

    private void Jump()
    {
        rbody.velocity = Vector3.up * jumpSpeed;
    }

    private void ApplyGroundLinearDrag()
    {
        if (Mathf.Abs(_movement.x) < 0.4f || _changingDirection)
        {
            rbody.drag = _groundLinearDrag;
        }
        else
        {
            rbody.drag = 0f;
        }
    }

    private void ApplyAirLinearDrag()
    {
        rbody.drag = _airLinearDrag;
    }

    private void FallMultiplier()
    {
        if (rbody.velocity.y < _jumpVelocityFalloff)
        {
            rbody.velocity += _fallMultiplier * Physics.gravity.y * Vector3.up * Time.deltaTime;
        }
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color32(0, 255, 0, 200);
        Gizmos.DrawSphere(transform.position - new Vector3(0, sphereCastDistance, 0), sphereCastRadius);
    }
}
