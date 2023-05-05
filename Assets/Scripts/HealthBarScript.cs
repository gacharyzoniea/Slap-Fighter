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


    public int stocks = 2;
    public int healthValue = 0;
    private float healthDiv = 0;
    private int healthmax = 999;
    public bool assigned = false;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] private Color fullColor;
    [SerializeField] private Color lowColor;

    public GameObject stockScore;


    private void Start()
    {
        
    }

    void Update()
    {
        setColor();
        healthText.text = healthValue + "%";

        if (stockScore != null)
        {
            Transform stockTransform = stockScore.transform.GetChild(0);
            Transform stock1Transform = stockScore.transform.GetChild(1);
            GameObject stockGO = stockTransform.gameObject;
            GameObject stock1GO = stock1Transform.gameObject;

            if (stocks >= 2)
            {
                stock1GO.SetActive(true);
                stockGO.SetActive(true);

            }
            if (stocks <= 1)
            {
                stockGO.SetActive(true);
            }
            
        }
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
