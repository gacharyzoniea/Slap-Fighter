using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerAttackScript : MonoBehaviour
{

    private PlayerMovementFixed _pm;
    private PlayerDashScript _ds;
    Animator animator;
    public bool moveLag = false;
    public HealthBarScript healthBar;
    public int maxHealth = 100;

    private InputAction _attack;
    private InputAction _special;
    private InputAction _move;
    private InputActionAsset inputActions;
    private InputActionMap player;
    private float moveVal;

    [SerializeField] private Collider[] attackList;

    private void Awake()
    {
        //playerMovement = new SlapFighter();
        inputActions = this.GetComponent<PlayerInput>().actions;
        player = inputActions.FindActionMap("Player");
    }
    private void OnEnable()
    {
        _special = player.FindAction("Specials");
        _attack = player.FindAction("Normals");
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
        if (!moveLag && _attack.triggered && _move.ReadValue<Vector2>().magnitude <= .05 && _pm.isGrounded)
        {
            jab();
        }
        else if(!moveLag && _attack.triggered && _move.ReadValue<Vector2>().y < 0 && _pm.isGrounded)
        {
            sweep();
        }
    }

    private void jab()
    {
        animator.SetTrigger("Jab");
        StartCoroutine(attackBox(attackList[0] ,.35f, 3));
        StartCoroutine(endLag(.5f));
    }

    private void sweep()
    {
        animator.SetTrigger("Sweep");
        StartCoroutine(attackBox(attackList[1], .4f, 2));
        StartCoroutine(endLag(.7f));
    }

    IEnumerator endLag (float endlag)
    {
        moveLag = true;
        yield return new WaitForSeconds(endlag);
        moveLag = false;
    }

    IEnumerator attackBox(Collider col, float timeToActivate, int damage)
    {
        yield return new WaitForSeconds(timeToActivate);
        launchAttack(col, damage);
    }

    private void launchAttack (Collider col, int damage)
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
          
        }
    }
}
