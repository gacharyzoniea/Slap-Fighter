using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    Vector3 _movement;
    Vector3 _gravity = new Vector3(0, -.5f, 0);
    CharacterController _characterController;
    float _speed = .5f;
    public SlapFighter playerMovement; //input object
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float smoothingFactor = 0.1f;

    private InputAction _move;
    private InputAction _jump;


    private void Awake()
    {
        playerMovement = new SlapFighter();
    }

    private void OnEnable()
    {
        _move = playerMovement.Player.Move;
        _move.Enable();
    }

    private void OnDisable()
    {
        _move.Disable();
    }
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        Vector3 currentVelocity = _characterController.velocity;
        Vector3 newVelocity = Vector3.Lerp(currentVelocity, _movement, smoothingFactor);

        if (Mathf.Approximately(_movement.x, 0f))
        {
            newVelocity.x = Mathf.Lerp(currentVelocity.x, _movement.x, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            newVelocity.x = Mathf.Lerp(currentVelocity.x, _movement.x, acceleration * Time.fixedDeltaTime);
        }

        _characterController.Move(newVelocity * _speed);
        _characterController.Move(_gravity);
    }

    private void Update()
    {
        _movement = _move.ReadValue<Vector2>();
    }

}
