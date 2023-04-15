using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerAttackScript : MonoBehaviour
{

    public PlayerMovementFixed _pm;
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

    public AudioClip attackLight;
    public AudioClip attackHeavy;
    public AudioClip whiff;
    
    private AudioSource _source;

    [SerializeField] private Collider[] attackList;
    private GameObject[] healthBarList;

    private void Awake()
    {
        healthBarList = GameObject.FindGameObjectsWithTag("Right");
        print(healthBarList.Length);
        _source = this.GetComponent<AudioSource>();
        //playerMovement = new SlapFighter();
        //if (healthBar == null)
        //{
        //    healthBar = GameObject.FindGameObjectWithTag("Right").GetComponent<HealthBarScript>();
        //}
    }


    // Start is called before the first frame update
    void Start()
    {
        Physics.SyncTransforms();
        _pm = GetComponent<PlayerMovementFixed>();
        _ds = GetComponent<PlayerDashScript>();
        animator = _pm.animator;
        if(healthBar == null)
        {
            foreach(GameObject bar in healthBarList)
            {
                if (bar.GetComponent<HealthBarScript>().assigned == false)
                {
                    print("first");
                    bar.GetComponent<HealthBarScript>().assign();
                    healthBar = bar.GetComponent<HealthBarScript>();
                    break;
                }
            }
        }
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        moveVal = _pm._movement.magnitude;
        
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
        #region oldAttacks
        //jab
        //if (!moveLag && _attack.triggered && _move.ReadValue<Vector2>().magnitude <= .05f && _pm.isGrounded)
        //{
        //    jab();
        //}
        //else if(!moveLag && _pm.isGrounded && _attack.triggered && _move.ReadValue<Vector2>().y < 0 && _pm.isGrounded)
        //{
        //    sweep();
        //}
        //else if (!moveLag && _attack.triggered && Mathf.Abs(_move.ReadValue<Vector2>().x) > .5f && _pm.isGrounded)
        //{
        //    roundhouse();
        //}
        //else if (!moveLag && _attack.triggered && _move.ReadValue<Vector2>().y > 0 && _pm.isGrounded)
        //{
        //    uppercut();
        //}
        //else if (!_pm.isGrounded && _attack.triggered && Mathf.Abs(_move.ReadValue<Vector2>().x) > .5f)
        //{
        //    fair();
        //}
        //else if (!_pm.isGrounded && _attack.triggered && _move.ReadValue<Vector2>().y > .5f)
        //{
        //    uair();
        //}
        //else if (!_pm.isGrounded && _attack.triggered && _move.ReadValue<Vector2>().y < -.5f)
        //{
        //    dair();
        //}
        #endregion
    }


    public void jab()
    {
        animator.SetTrigger("Jab");
        Debug.Log("here");
        StartCoroutine(attackBox(attackList[0] ,.35f, 3, 4, attackLight));
        StartCoroutine(SwooshSound(0.35f));
        StartCoroutine(endLag(.4f));
    }

    public void sweep()
    {
        animator.SetTrigger("Sweep");
        StartCoroutine(attackBox(attackList[1], attackLength, 2, 7, attackLight));
        StartCoroutine(SwooshSound(attackLength));
        StartCoroutine(endLag(.55f));
    }

    public void roundhouse()
    {
        animator.SetTrigger("Roundhouse");
        StartCoroutine(attackBox(attackList[2], .3f, 2, 10, attackHeavy));
        StartCoroutine(endLag(.4f));
    }

    public void uppercut()
    {
        animator.SetTrigger("Uppercut");
        StartCoroutine(attackBox(attackList[3], .3f, 5, 12, new Vector3(0, 1, 0), attackHeavy));
        StartCoroutine(endLag(.35f));
    }

    public void uair()
    {
        animator.SetTrigger("Uair");
        StartCoroutine(attackBox(attackList[4], .4f, 2, 11, new Vector3(0, 1, 0), attackLight));
        StartCoroutine(endLag(.75f));
    }

    public void fair()
    {
        animator.SetTrigger("Fair");
        StartCoroutine(attackBox(attackList[5], .35f, 2, 10, new Vector3(.1f, 1, 0), attackLight));
        StartCoroutine(endLag(.4f));
    }
    public void dair()
    {
        animator.SetTrigger("Dair");
        StartCoroutine(attackBox(attackList[6], .35f, 2, 10, new Vector3(0, -1, 0), attackHeavy));
        StartCoroutine(endLag(.75f));
    }


    IEnumerator endLag (float endlag)
    {
        moveLag = true;
        yield return new WaitForSeconds(endlag);
        moveLag = false;
    }

    //implied knockback
    IEnumerator attackBox(Collider col, float timeToActivate, int damage, float force)
    {
        Debug.Log(col.transform.position);
        yield return new WaitForSeconds(timeToActivate);
        launchAttack(col, damage, force);
    }

    //implied knockback (with audio)
    IEnumerator attackBox(Collider col, float timeToActivate, int damage, float force, AudioClip audio)
    {
        yield return new WaitForSeconds(timeToActivate);
        _source.PlayOneShot(audio);
        launchAttack(col, damage, force);
    }

    //manual knockback
    IEnumerator attackBox(Collider col, float timeToActivate, int damage, float force, Vector3 dir)
    {
        yield return new WaitForSeconds(timeToActivate);
        launchAttack(col, damage, force, dir);
    }

    //manual knockback (with audio)
    IEnumerator attackBox(Collider col, float timeToActivate, int damage, float force, Vector3 dir, AudioClip audio)
    {
        yield return new WaitForSeconds(timeToActivate);
        _source.PlayOneShot(audio);
        launchAttack(col, damage, force, dir);
    }

    //implied knockback
    private void launchAttack (Collider col, int damage, float force)
    {
        Debug.Log("here");
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox", "Shield"));
        Debug.Log(cols.Length);
        foreach (Collider c in cols)
        {
            Debug.Log("here");
            if (c.transform.gameObject.layer == 13) //if collision with shield, attack blocked
            {
                Debug.Log("here");
                return;
            }
        }

        foreach (Collider c in cols)
        {
            Debug.Log("here");
            if (c.transform.root == transform)
            {
                Debug.Log("here");
                continue;
            }
            Debug.Log("here");
            c.transform.root.GetComponent<playerAttackScript>().healthBar.takeHealth(damage);
            Debug.Log("here");
            Knockback(c.transform.position - col.transform.position, force, c.transform.root.GetComponent<Rigidbody>());
            Debug.Log("here");

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

    IEnumerator SwooshSound(float wait)
    {
        yield return new WaitForSeconds(wait);
        _source.PlayOneShot(whiff);
    }

}
