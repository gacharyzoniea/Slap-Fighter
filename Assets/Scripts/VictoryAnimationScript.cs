using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryAnimationScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    string[] danceNames = { "Macarena Dance", "Swing Dancing", "Silly Dancing"};
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        int r = Random.Range(0, 2);
        animator = GetComponent<Animator>();
        //animator.Play("Macarena Dance");
        //animator.Play("Swing Dancing");
        animator.Play(danceNames[r]);

    }
}
