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
    private Vector3 delayedForceToApply;
    bool canDash = true;

    [Header("Cooldown")]
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
        if (dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;
        }
        if(pm.isGrounded && dashCdTimer > .2f)
        {
            dashCdTimer = .1f;
        }
    }
    private void Dash()
    {
        if (!pm._isDashing && dashCdTimer <= 0)
        {
            if (!pm.isGrounded)
            {
                dashCdTimer = 2f;
            }
            else
            {
                dashCdTimer = .75f;
            }
            StartCoroutine(Trail());
            canDash = false;
            pm._isDashing = true;
            rbody.velocity = new Vector3(rbody.velocity.x, 0, rbody.velocity.z);
            Vector3 forceToApply =
            new Vector3(orientation.normalized.x * dashForce, orientation.normalized.y * dashUpForce, 0f);
            delayedForceToApply = forceToApply;
            Invoke(nameof(DelayedDashForce), .0025f);
            Invoke(nameof(ResetDash), dashDuration);
        }
        else
        {
            return;
        }
    }

    private void DelayedDashForce()
    {
        rbody.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm._isDashing = false;
    }
    IEnumerator Trail()
    {
        tr.enabled = true;
        yield return new WaitForSecondsRealtime(.15f);
        tr.enabled = false;
    }
}
