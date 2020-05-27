using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{

    public Slider slider;

    private float maxHealth = 1;

    public void SetMaxHealth(float health)
    {
        maxHealth = health;
        float healthVal = NormalizeHealthRange(health, maxHealth);
        
        slider.maxValue = healthVal;
        slider.value = healthVal;
    }

    public void SetHealth(float health)
    {
        float healthVal = NormalizeHealthRange(health, maxHealth);
        slider.value = healthVal;
    }

    private float NormalizeHealthRange(float health, float maxHealth)
    {
        // return a number between 0 and 1
        return ((health - 0) / (maxHealth - 0));
    }
}
