using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    
    [SerializeField] private float maxHealth = 10f;

    public byte TeamNumber = 0;  

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
        
        if(health <= 0)
        {
            bIsDead = true;
        }

        OnHealthChange?.Invoke(this, new OnHealthChangeEventArgs { damageAmount = damage, currentHealth = health, owner = gameObject, instigator = _instigator });
    }
}
