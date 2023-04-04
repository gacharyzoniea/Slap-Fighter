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
    private Vector3 shieldSize;

    [Header("Shield options")]
    public float shieldLength;
    private bool _isShielding;
    public float shieldMin;
    private float shieldHealth = 100;


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
        shieldSize = shieldObject.gameObject.transform.localScale;
        _pm = GetComponent<PlayerMovementFixed>();
        _isShielding = false;
        StartCoroutine(addShield());
        StartCoroutine(removeShield());
    }

    // Update is called once per frame
    void Update()
    {
        if(_shield.ReadValue<float>() > 0 && _pm.isGrounded)
        {
            _isShielding = true;
            _pm.animator.SetBool("Blocking", true);
        }
        else
        {
            _isShielding=false;
            _pm.animator.SetBool("Blocking", false);
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

        Mathf.Clamp(shieldHealth, 0, 100);
    }

    private void Shield()
    {
        _pm._canMove = false;
        shieldObject.SetActive(true);
    }

    IEnumerator addShield()
    {
        while (true)
        {
            if(shieldHealth < 100 && !_isShielding)
            {
                shieldHealth++;
                shieldSize = new Vector3((shieldSize.x + (shieldSize.x * (shieldHealth * .0001f))),
                    (shieldSize.y + (shieldSize.y * (shieldHealth * .0001f))),
                    (shieldSize.z + (shieldSize.z * (shieldHealth * .0001f))));
                shieldObject.transform.localScale = shieldSize;
                yield return new WaitForSeconds(.1f);
            }
            else
            {
                yield return null;
            }
        }   
    }

    IEnumerator removeShield()
    {
        while (true)
        {
            if (shieldHealth > 0 && _isShielding)
            {
                shieldHealth--;
                shieldSize = new Vector3((shieldSize.x-(shieldSize.x * (shieldHealth * .0001f))), 
                    (shieldSize.y-(shieldSize.y * (shieldHealth * .0001f))), 
                    (shieldSize.z-(shieldSize.z * (shieldHealth * .0001f))));
                shieldObject.transform.localScale = shieldSize;
                yield return new WaitForSeconds(.1f);
            }
            else
            {
                yield return null;
            }
        }
    }
}
