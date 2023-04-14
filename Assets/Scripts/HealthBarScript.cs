using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;
    public bool assigned = false;

    public void assign()
    {
        assigned = true;
    }
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void setHealth(int health)
    {
        slider.value = health;
    }

    public void takeHealth(int damage)
    {
        slider.value -= damage;
    }
}
