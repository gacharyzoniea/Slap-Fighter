using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTwoAttackScript : MonoBehaviour
{

    private PlayerMovementFixed _pm;
    private PlayerDashScript _ds;
    Animator animator;
    public bool moveLag = false;
    public HealthBarScript healthBar;
    public int maxHealth = 100;

    private InputAction _attack;
    private InputAction _attackStick;
    private InputAction _special;
    private InputAction _move;
    private InputActionAsset inputActions;
    private InputActionMap player;
    private float moveVal;
    public float attackLength = .4f;
    public float attackhit = .4f;

    [SerializeField] private Collider[] attackList;

    private void Awake()
    {
        //playerMovement = new SlapFighter();
        inputActions = this.GetComponent<PlayerInput>().actions;
        player = inputActions.FindActionMap("Player");
        if(healthBar == null)
        {
            Debug.Log("works");
        }
        Debug.Log(healthBar);
        healthBar = GameObject.FindGameObjectWithTag("Right").GetComponent<HealthBarScript>();
        Debug.Log(healthBar);
    }
    private void OnEnable()
    {
        _special = player.FindAction("Specials");
        _attack = player.FindAction("Normals");
        _attackStick = player.FindAction("NormalStick");
        _move = player.FindAction("Move");
        player.Enable();
    }
    private void OnDisable()
    {
        player.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetMaxHealth(maxHealth);
        _pm = GetComponent<PlayerMovementFixed>();
        _ds = GetComponent<PlayerDashScript>();
        animator = _pm.animator;
    }

    // Update is called once per frame
    void Update()
    {
        moveVal = _move.ReadValue<Vector2>().magnitude;

        if (moveLag)
        {
            _pm._canMoveLag = false;
            _pm._canJump = false;
            _ds.canDash = false;
        }
        else
        {
            _pm._canMoveLag = true;
            _pm._canJump = true;
            _ds.canDash = true;
        }
        /**
         * ATTACKS (normals)
         */
        //jab
        if (!moveLag && _attack.triggered && _move.ReadValue<Vector2>().magnitude <= .05f && _pm.isGrounded)
        {
            jab();
        }
        else if (!moveLag && _pm.isGrounded && _attack.triggered && _move.ReadValue<Vector2>().y < 0 && _pm.isGrounded)
        {
            sweep();
        }
        else if (!moveLag && _attack.triggered && Mathf.Abs(_move.ReadValue<Vector2>().x) > .5f && _pm.isGrounded)
        {
            roundhouse();
        }
        else if (!moveLag && _attack.triggered && _move.ReadValue<Vector2>().y > 0 && _pm.isGrounded)
        {
            uppercut();
        }
        else if (!_pm.isGrounded && _attack.triggered && Mathf.Abs(_move.ReadValue<Vector2>().x) > .5f)
        {
            fair();
        }
        else if (!_pm.isGrounded && _attack.triggered && _move.ReadValue<Vector2>().y > .5f)
        {
            uair();
        }
        else if (!_pm.isGrounded && _attack.triggered && _move.ReadValue<Vector2>().y < -.5f)
        {
            dair();
        }
    }


    private void jab()
    {
        animator.SetTrigger("Jab");
        StartCoroutine(attackBox(attackList[0], .35f, 3, 4));
        StartCoroutine(endLag(.4f));
    }

    private void sweep()
    {
        animator.SetTrigger("Sweep");
        StartCoroutine(attackBox(attackList[1], attackLength, 2, 7));
        StartCoroutine(endLag(.55f));
    }

    private void roundhouse()
    {
        animator.SetTrigger("Roundhouse");
        StartCoroutine(attackBox(attackList[2], .3f, 2, 10));
        StartCoroutine(endLag(.4f));
    }

    private void uppercut()
    {
        animator.SetTrigger("Uppercut");
        StartCoroutine(attackBox(attackList[3], .3f, 5, 12, new Vector3(0, 1, 0)));
        StartCoroutine(endLag(.35f));
    }

    private void uair()
    {
        animator.SetTrigger("Uair");
        StartCoroutine(attackBox(attackList[4], .4f, 2, 11, new Vector3(0, 1, 0)));
        StartCoroutine(endLag(.75f));
    }

    private void fair()
    {
        animator.SetTrigger("Fair");
        StartCoroutine(attackBox(attackList[5], .35f, 2, 10, new Vector3(.1f, 1, 0)));
        StartCoroutine(endLag(.4f));
    }
    private void dair()
    {
        animator.SetTrigger("Dair");
        StartCoroutine(attackBox(attackList[6], .35f, 2, 10, new Vector3(0, -1, 0)));
        StartCoroutine(endLag(.75f));
    }
    IEnumerator endLag(float endlag)
    {
        moveLag = true;
        yield return new WaitForSeconds(endlag);
        moveLag = false;
    }

    //implied knockback
    IEnumerator attackBox(Collider col, float timeToActivate, int damage, float force)
    {
        yield return new WaitForSeconds(timeToActivate);
        launchAttack(col, damage, force);
    }

    //manual knockback
    IEnumerator attackBox(Collider col, float timeToActivate, int damage, float force, Vector3 dir)
    {
        yield return new WaitForSeconds(timeToActivate);
        launchAttack(col, damage, force, dir);
    }

    //implied knockback
    private void launchAttack(Collider col, int damage, float force)
    {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox", "Shield"));
        foreach (Collider c in cols)
        {
            if (c.transform.gameObject.layer == 13) //if collision with shield, attack blocked
            {
                return;
            }
        }

        foreach (Collider c in cols)
        {
            if (c.transform.root == transform)
                continue;

            c.transform.root.GetComponent<playerAttackScript>().healthBar.takeHealth(damage);
            Knockback(c.transform.position - col.transform.position, force, c.transform.root.GetComponent<Rigidbody>());


        }
    }

    //manual knockback
    private void launchAttack(Collider col, int damage, float force, Vector3 dir)
    {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox", "Shield"));
        foreach (Collider c in cols)
        {
            if (c.transform.gameObject.layer == 13) //if collision with shield, attack blocked
            {
                return;
            }
        }

        foreach (Collider c in cols)
        {
            if (c.transform.root == transform)
                continue;

            c.transform.root.GetComponent<playerAttackScript>().healthBar.takeHealth(damage);
            Knockback(dir, force, c.transform.root.GetComponent<Rigidbody>());

        }
    }

    private void Knockback(Vector3 dir, float force, Rigidbody rbody)
    {
        rbody.velocity = Vector3.zero;
        rbody.AddForce(dir * force, ForceMode.VelocityChange);
    }

}
