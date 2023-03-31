using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShieldScript : MonoBehaviour
{
    [Header("References")]
    private PlayerMovementFixed _pm;
    public GameObject shieldObject;

    [Header("Shield options")]
    public float shieldLength;
    private bool _isShielding;
    public float shieldMin;
    private float cdTimer;


    [Header("inputStuff")]
    //public SlapFighter playerMovement;
    private InputAction _shield;
    private InputActionAsset inputActions;
    private InputActionMap player;

    private void Awake()
    {
        inputActions = this.GetComponent<PlayerInput>().actions;
        player = inputActions.FindActionMap("Player");
    }
    private void OnEnable()
    {
        _shield = player.FindAction("Shield");
        player.Enable();
    }
    private void OnDisable()
    {
        player.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        cdTimer = 2f;
        _pm = GetComponent<PlayerMovementFixed>();
        _isShielding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_shield.ReadValue<float>() > 0 && _pm.isGrounded)
        {
            _isShielding = true;
        }
        else
        {
            _isShielding=false;
        }
        if (_isShielding)
        {
            Shield();
        }
        else
        {
            _isShielding = false;
            _pm._canMove = true;
            shieldObject.SetActive(false);
        }
        if (cdTimer > 0)
        {
            cdTimer -= Time.deltaTime;
        }
    }

    private void Shield()
    {
        _pm._canMove = false;
        shieldObject.SetActive(true);
    }

    private void ShieldReset()
    {
        _pm._canMove = true;
        if(cdTimer < 2)
        {
            cdTimer += Time.deltaTime;
        }
        
    }
}
