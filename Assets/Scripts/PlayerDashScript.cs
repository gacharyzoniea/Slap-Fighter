using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashScript : MonoBehaviour
{
    [Header("References")]
    private Vector3 orientation;
    public Rigidbody rbody;
    private PlayerMovementFixed pm;
    private TrailRenderer tr;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpForce;
    public float dashDuration;
    bool canDash = true;

    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    [Header("inputStuff")]
    public SlapFighter playerMovement;
    private InputAction _dash;
    private InputAction _move;

    private void Awake()
    {
        playerMovement = new SlapFighter();
    }
    private void OnEnable()
    {
        _move = playerMovement.Player.Move;
        _dash = playerMovement.Player.Dash;
        _dash.Enable();
        _move.Enable();
    }
    private void OnDisable()
    {
        _dash.Disable();
        _move.Disable();
    }

    private void Start()
    {
        rbody = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementFixed>();
        tr = GetComponent<TrailRenderer>();
        tr.enabled = false;
        canDash = true;
    }
    private void Update()
    {
        orientation = _move.ReadValue<Vector2>();
        if (_dash.triggered)
        {
            Dash();
        }
    }
    private void Dash()
    {
        if (!pm._isDashing)
        {
            canDash = false;
            pm._isDashing = true;
            Vector3 forceToApply =
            new Vector3(orientation.normalized.x * dashForce, orientation.normalized.y * dashUpForce, 0f);
            StartCoroutine(Trail());
            rbody.AddForce(forceToApply, ForceMode.Impulse);
            Invoke(nameof(ResetDash), dashDuration);
        }
        else
        {
            return;
        }
    }

    private void ResetDash()
    {
        pm._isDashing = false;
    }
    IEnumerator Trail()
    {
        tr.enabled = true;
        yield return new WaitForSecondsRealtime(.3f);
        tr.enabled = false;
    }
}
