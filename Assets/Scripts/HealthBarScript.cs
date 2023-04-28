using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using TMPro;

public class HealthBarScript : MonoBehaviour
{
    public Image _bg;
    
    public int healthValue = 0;
    private float healthDiv = 0;
    private int healthmax = 999;
    public bool assigned = false;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] private Color fullColor;
    [SerializeField] private Color lowColor;


    void Update()
    {
        setColor();
        healthText.text = healthValue + "%";
    }

    private void setColor()
    {
        healthDiv = (float)healthValue / 999;
        _bg.color = Color.Lerp( fullColor, lowColor, healthDiv);
    }

    public void assign()
    {
        assigned = true;
    }

    public void setHealth(int health)
    {
        healthValue = health;
    }

    public void takeDamage(int damage)
    {
        if(healthValue < healthmax)
        {
            healthValue += damage;
        }
        else
        {
            healthValue = 999;
        }
    }
}
