using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackScript : MonoBehaviour
{

    float p; //percentage
    float d; //damage
    float b; //attack base knockback
    float s; //attack knockback scaling
    float w; //weight
    float angle; //angle of the attack

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CalculateKnockback()
    {
        float knockback = ((p / 10) + ((p * d) / 20)) * (200/(w+100)) * 1.4f + 18 * s + b;
    }
}
