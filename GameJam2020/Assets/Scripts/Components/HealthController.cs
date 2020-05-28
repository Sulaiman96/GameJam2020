using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    
    [SerializeField] private float maxHealth = 10f;

    public byte TeamNumber = 0;
    [HideInInspector] public HealthBarUI healthUI;

    private float health = 10f;
    private bool bIsDead = false;

    public event EventHandler<OnHealthChangeEventArgs> OnHealthChange;
    public class OnHealthChangeEventArgs : EventArgs
    {
        public float damageAmount;
        public float currentHealth;
        public GameObject owner;
        public GameObject instigator;
    }

    void Start()
    {
        health = maxHealth;
        OnHealthChange += HealthChange;

        if (healthUI)
            healthUI.SetMaxHealth(maxHealth);
    }

    public void OnTakeDamage(float damage, GameObject _instigator)
    {
        HealthController instigatorHealthController;
        if (_instigator.TryGetComponent<HealthController>(out instigatorHealthController))
        {
            if (instigatorHealthController.TeamNumber == TeamNumber)
                return;
        }

        health = health - damage;
        
        OnHealthChange?.Invoke(this, new OnHealthChangeEventArgs { damageAmount = damage, currentHealth = health, owner = gameObject, instigator = _instigator });

        if (health <= 0)
        {
            bIsDead = true;
            OnHealthChange -= HealthChange;
        }
    }

    private void HealthChange(object sender, HealthController.OnHealthChangeEventArgs e)
    {
        if (healthUI)
            healthUI.SetHealth(e.currentHealth);
    }

}
