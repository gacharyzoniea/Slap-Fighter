using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAIScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float knockbackForce = 500f;
    public GameObject model;
    public Animator animator;

    private bool movingRight = true;

    private void Update()
    {
        if (movingRight)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }

        if (transform.position.x >= 36f)
        {
            movingRight = false;
        }
        else if (transform.position.x <= -45f)
        {
            movingRight = true;
        }
        turnSean();
    }

    void turnSean()
    {
        if (movingRight)
        {
            model.transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            model.transform.eulerAngles = new Vector3(0, -90, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = other.gameObject.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            }
            animator.SetTrigger("Hit");
        }
    }
}
