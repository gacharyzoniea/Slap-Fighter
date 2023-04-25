using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerInputScript : MonoBehaviour
{
    private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField] private List<Transform> startingPoints;
    [SerializeField] private List<LayerMask> playerLayers;
    [SerializeField] List<GameObject> prefabs = new List<GameObject>();
    GameObject newPlayer;
    PlayerMovementFixed movement;
    playerAttackScript atk;
    PlayerShieldScript shield;
    PlayerDashScript dash;
    bool paused = false;


    void Start()
    {
        int tospawn = Random.Range(0, prefabs.Count);
        newPlayer = GameObject.Instantiate(prefabs[tospawn], prefabs[tospawn].transform.position, transform.rotation);
        movement = newPlayer.GetComponent<PlayerMovementFixed>();
        atk = newPlayer.GetComponent<playerAttackScript>();
        shield = newPlayer.GetComponent<PlayerShieldScript>();
        dash = newPlayer.GetComponent<PlayerDashScript>();
    }
    
    void LateUpdate() {
        if (movement.pauseScript)
        {
            paused = movement.pauseScript.GameIsPaused;
        }
    }


    public void Move(InputAction.CallbackContext context)
    {
        if (movement && !paused)
        {
            movement.setMovement(context.ReadValue<Vector2>());
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if(movement)
            movement.Pause();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (movement && context.started && movement._canJump && !paused && !shield._isShielding)
        {
            if(movement.isGrounded)
            {
                movement.Jump();
            }
            else if (movement.canDouble)
            {
                movement.canDouble = false;
                movement.Jump();
            }
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (dash && context.started && !paused && !shield._isShielding)
        {
            dash.Dash();
        }
    }

    public void Shield(InputAction.CallbackContext context)
    {
        if (shield && !paused)
        {
                if(context.ReadValue<float>() > 0 && shield._pm.isGrounded)
                {
                    shield._isShielding = true;
                }
                else
                {
                    shield._isShielding = false;
                }
        }
        
    }

    public void Attack(InputAction.CallbackContext attack)
    {
        if (atk && !paused && !shield._isShielding)
        {
            if (!atk.moveLag && attack.started && movement._movement.magnitude <= .05f && atk._pm.isGrounded)
            {
                atk.jab();
            }
            else if (!atk.moveLag && attack.started && movement._movement.y < 0 && atk._pm.isGrounded)
            {
                atk.sweep();
            }
            else if (!atk.moveLag && attack.started && Mathf.Abs(movement._movement.x) > .5f && atk._pm.isGrounded)
            {
                atk.roundhouse();
            }
            else if (!atk.moveLag && attack.started && movement._movement.y > 0 && atk._pm.isGrounded)
            {
                atk.uppercut();
            }
            else if (!atk.moveLag && !atk._pm.isGrounded && attack.started && Mathf.Abs(movement._movement.x) > .5f)
            {
                atk.fair();
            }
            else if (!atk.moveLag && !atk._pm.isGrounded && attack.started && movement._movement.y > .5f)
            {
                atk.uair();
            }
            else if (!atk.moveLag && !atk._pm.isGrounded && attack.started && movement._movement.y < -.5f)
            {
                atk.dair();
            }
        }
    }

    public void AttackStick(InputAction.CallbackContext direction)
    {
        Vector2 dir = direction.ReadValue<Vector2>();
        if (atk && !paused && !shield._isShielding)
        {
            if (!atk.moveLag && dir.y < -.3f && atk._pm.isGrounded)
            {
                atk.sweep();
            }
            else if (!atk.moveLag && Mathf.Abs(dir.x) > .5f && atk._pm.isGrounded)
            {
                atk.roundhouse();
            }
            else if (!atk.moveLag && dir.y > .3f && atk._pm.isGrounded)
            {
                atk.uppercut();
            }
            else if (!atk.moveLag && !atk._pm.isGrounded && Mathf.Abs(dir.x) > .3f)
            {
                atk.fair();
            }
            else if (!atk.moveLag && !atk._pm.isGrounded && dir.y > .5f)
            {
                atk.uair();
            }
            else if (!atk.moveLag && !atk._pm.isGrounded && dir.y < -.5f)
            {
                atk.dair();
            }
        }
    }
}
