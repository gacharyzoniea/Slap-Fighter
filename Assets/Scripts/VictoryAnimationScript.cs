using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryAnimationScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    string[] danceNames = { "Macarena Dance", "Swing Dancing", "Silly Dancing", "Macarena Dance", "Swing Dancing", "Silly Dancing" };

    public Material mainColor;
    public Material altColor;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        
        animator = GetComponent<Animator>();
        //animator.Play("Macarena Dance");
        //animator.Play("Swing Dancing");
        animator.Play(danceNames[Random.Range(0, 5)]);

    }
}
