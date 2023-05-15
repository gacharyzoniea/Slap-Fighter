using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerAttackScript : MonoBehaviour
{

    public PlayerMovementFixed _pm;
    private PlayerDashScript _ds;
    Animator animator;
    public bool moveLag = false;
    bool canAttack = true;
    public bool aerialLag = false;
    public HealthBarScript healthBar;
    public LivesManager stock;
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

    //for death
    public AudioClip death;
    public AudioClip deathsplode;
    public GameObject deathsplosion;
    
    private AudioSource _source;

    public GameObject[] hitEffects;

    [SerializeField] private Collider[] attackList;
    private GameObject[] healthBarList;
    private GameObject[] scoreList;

    public Material mainColor;
    public Material altColor;
    private HealthBarScript otherHealth;
    private playerAttackScript otherAttack;
    private PlayerDashScript otherDash;
    private PlayerMovementFixed otherMovement;

    public Vector3 respawnPoint;


    private void Awake()
    {
        healthBarList = GameObject.FindGameObjectsWithTag("Right");
        scoreList = GameObject.FindGameObjectsWithTag("ScoreRight");
        _source = this.GetComponent<AudioSource>();
    }


    // Start is called before the first frame update
    void Start()
    {
        Physics.SyncTransforms();
        _pm = GetComponent<PlayerMovementFixed>();
        _ds = GetComponent<PlayerDashScript>();
        animator = _pm.animator;
        findStockScore();
        findHealthBar();
    }
    private void LateUpdate()
    {
        if (!otherAttack)
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (g.transform == transform)
                {
                    continue;
                }
                else
                {
                    otherHealth = g.GetComponent<playerAttackScript>().healthBar;
                    otherAttack = g.GetComponent<playerAttackScript>();
                    otherMovement = g.GetComponent<PlayerMovementFixed>();
                    otherDash = g.GetComponent<PlayerDashScript>();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveVal = _pm._movement.magnitude;
        
        if (moveLag || aerialLag)
        {
            _pm._canMoveLag = false;
            _pm._canJump = false;
            _ds.canDash = false;
        }
        else
        {
            _pm._canMoveLag = true;
                if (!_pm._inHitstun)
                {
                    _pm._canJump = true;
                    _ds.canDash = true;
                }
        }

        if (_pm.isGrounded && aerialLag)
        {
            StartCoroutine(landingLag());
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

    //attacks region (expand to view)
    #region Attacks
    public void jab()
    {
        if (canAttack)
        {
            canAttack = false;
            Invoke(nameof(setCanAttack), .4f);
            animator.SetTrigger("Jab");
            StartCoroutine(attackBox(attackList[0], .35f, 3, 4, attackLight));
            StartCoroutine(SwooshSound(0.35f));
            StartCoroutine(endLag(.4f));
        }
    }
    public void sweep()
    {
        if (canAttack)
        {
            canAttack = false;
            Invoke(nameof(setCanAttack), .55f);
            animator.SetTrigger("Sweep");
            StartCoroutine(attackBox(attackList[1], attackLength, 2, 7, attackLight));
            StartCoroutine(SwooshSound(attackLength));
            StartCoroutine(endLag(.55f));
        }
    }
    public void roundhouse()
    {
        if (canAttack)
        {
            canAttack = false;
            Invoke(nameof(setCanAttack), .4f);
            animator.SetTrigger("Roundhouse");
            StartCoroutine(attackBox(attackList[2], .3f, 2, 10, attackHeavy));
            StartCoroutine(endLag(.4f));
        }
    }
    public void uppercut()
    {
        if (canAttack)
        {
            canAttack = false;
            Invoke(nameof(setCanAttack), .35f);
            animator.SetTrigger("Uppercut");
            StartCoroutine(attackBox(attackList[3], .3f, 5, 12, new Vector3(0, 1, 0), attackHeavy));
            StartCoroutine(endLag(.35f));
        }
    }
    public void uair()
    {
        if (canAttack)
        {
            canAttack = false;
            Invoke(nameof(setCanAttack), .75f);
            animator.SetTrigger("Uair");
            StartCoroutine(attackBox(attackList[4], .4f, 2, 11, new Vector3(0, 1, 0), attackLight));
            StartCoroutine(endLagAerial(.75f));
        }
    }
    public void fair()
    {
        if (canAttack)
        {
            canAttack = false;
            Invoke(nameof(setCanAttack), .4f);
            animator.SetTrigger("Fair");
            StartCoroutine(attackBox(attackList[5], .35f, 2, 10, new Vector3(.1f, 1, 0), attackLight));
            StartCoroutine(endLagAerial(.4f));
        }
    }
    public void dair()
    {
        if (canAttack)
        {
            canAttack = false;
            Invoke(nameof(setCanAttack), .75f);
            animator.SetTrigger("Dair");
            StartCoroutine(attackBox(attackList[6], .35f, 2, 10, new Vector3(0, -1, 0), attackHeavy));
            StartCoroutine(endLagAerial(.75f));
        }
    }
    #endregion

    IEnumerator endLag (float endlag)
    {
        moveLag = true;
        yield return new WaitForSeconds(endlag);
        moveLag = false;
    }

    private void setCanAttack()
    {
        canAttack = true;
    }

    public void HitStun()
    {
        Debug.Log("in hitstun");
        setCanAttack();
        _pm._inHitstun = false;
        _ds.canDash = true;
    }
    public void HitStunJump()
    {
        Debug.Log("in hitstun");
        setCanAttack();
        _pm._inHitstun = false;
        if (_pm.isGrounded)
        {
            _pm._canJump = true;
            _pm.canDouble = true;
        }
        else
        {
            _pm.canDouble = true;
        }
        _ds.canDash = true;
    }

    IEnumerator endLagAerial(float endlag)
    {
        //moveLag = true;
        aerialLag = true;
        if (_pm.isGrounded)
        {
            StartCoroutine(landingLag());
        }
        yield return new WaitForSeconds(endlag);
        aerialLag = false;
        //moveLag = false;
    }

    IEnumerator landingLag()
    {
        //opAllCoroutines();
        animator.ResetTrigger("Uair");
        animator.ResetTrigger("Fair");
        animator.ResetTrigger("Dair");
        animator.SetTrigger("Landing");
        yield return new WaitForSeconds(0.2f);
        aerialLag = false;
        moveLag = false;
        animator.ResetTrigger("Landing");
    }

    IEnumerator landingLag(Coroutine aerial)
    {
        StopCoroutine(aerial);
        animator.SetTrigger("Landing");
        yield return new WaitForSeconds(0.2f);
        aerialLag = false;
        moveLag = false;
    }

    //hitbox coroutines
    #region attackHitboxActivators
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
    #endregion

    void playHitEffect(Vector3 hitVector)
    {
        print("effect triggered!");
        int r = UnityEngine.Random.Range(0, 3);
        ParticleSystem[] effects =
            hitEffects[r].GetComponentsInChildren<ParticleSystem>();
        

        //itEffects[].GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem p in effects)
        {
            print("!");
            p.transform.position = hitVector;
            p.Play();
        }
    }

    //implied knockback
    private void launchAttack (Collider col, int damage, float force)
    {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox", "Shield", "Player1", "Player2"));
        Debug.Log(cols.Length);
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
            {
                continue;
            }
            otherAttack.canAttack = false;
            otherMovement._inHitstun = true;
            otherDash.canDash = false;
            if (otherMovement._canJump || otherMovement.canDouble)
            {
                otherMovement._canJump = false;
                otherMovement.canDouble = false;
                otherAttack.Invoke(nameof(HitStunJump), (float)damage/7);
            }
            else
            {
                otherMovement._canJump = false;
                otherMovement.canDouble = false;
                otherAttack.Invoke(nameof(HitStun), (float)damage / 7);
            }
            otherHealth.takeDamage(damage);
            playHitEffect(col.bounds.center);
            Knockback(c.transform.position - col.transform.position, force, c.transform.root.GetComponent<Rigidbody>());

        }
    }

    //manual knockback
    private void launchAttack(Collider col, int damage, float force, Vector3 dir)
    {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox", "Shield", "Player1", "Player2"));
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
            {
                continue;
            }
                
            otherAttack.canAttack = false;
            otherMovement._inHitstun = true;
            otherDash.canDash = false;
            if (otherMovement._canJump || otherMovement.canDouble)
            {
                otherMovement._canJump = false;
                otherMovement.canDouble = false;
                otherAttack.Invoke(nameof(HitStunJump), (float)damage / 7);
            }
            else
            {
                otherMovement._canJump = false;
                otherMovement.canDouble = false;
                otherAttack.Invoke(nameof(HitStun), (float)damage / 7);
            }
            otherHealth.takeDamage(damage);
            playHitEffect(col.bounds.center);
            Knockback(dir, force, c.transform.root.GetComponent<Rigidbody>());

        }
    }

    private void Knockback(Vector3 dir, float force, Rigidbody rbody)
    {
            float kb;
            rbody.velocity = Vector3.zero;
            if (otherHealth.healthValue > 350) { 
            kb = force * otherHealth.healthValue/300;
            }
            else
            {
            kb = force;
            }
            rbody.AddForce(dir * kb, ForceMode.VelocityChange);
    }

    IEnumerator SwooshSound(float wait)
    {
        yield return new WaitForSeconds(wait);
        _source.PlayOneShot(whiff);
    }
    IEnumerator Respawn()
    {
        //this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        //_pm.enabled = false;
        //yield return new WaitForSeconds(0.5f);
        //transform.position = new Vector3(8, 45, 0);
        yield return new WaitForSeconds(0.5f);
        _pm.playerModel.SetActive(true);
        //this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        _pm.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("BlastZone") && stock.stocks > 0)
        {
            Destroy(Instantiate(deathsplosion, transform.position, transform.rotation), 2f);
            _pm.enabled = false;
            _pm.playerModel.SetActive(false);
            //_pm.moveSpeed = 0;
            _pm._movement = new Vector3(0, 0, 0);
            transform.position = stock.respawnPoint.position;
            
            
            stock.stocks--;
            healthBar.healthValue = 0;
            _source.PlayOneShot(death);
            _source.PlayOneShot(deathsplode, .5f);
            if (stock.stocks > 0)
            {
                StartCoroutine(Respawn());
            }
        }
    }

    private void findHealthBar()
    {
        if (healthBar == null)
        {
            foreach (GameObject bar in healthBarList)
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
    }

    private void findStockScore()
    {
        if (stock == null)
        {
            foreach (GameObject score in scoreList)
            {
                if (score.GetComponent<LivesManager>().assigned == false)
                {
                    print("first stock score");
                    score.GetComponent<LivesManager>().assign();
                    stock = score.GetComponent<LivesManager>();
                    stock._player = this.gameObject;
                    stock._playerInput = this.gameObject.GetComponent<PlayerInput>();
                    break;
                }
            }
        }
    }

}
