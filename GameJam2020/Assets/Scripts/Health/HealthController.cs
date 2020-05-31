using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private byte TeamNumber = 0;
    [SerializeField] private float immunityAfterDamageTimer = 1.5f;
    [SerializeField] private Material flashMaterial = null;
    [SerializeField] private Renderer rendererBody = null;
    [SerializeField] private TakeDamageEvent onDamageTextSpawn = null;
    [HideInInspector] public HealthBarUI healthUI;

    private float health = 10f;
    private bool bIsDead = false;
    private float lastDamageTime = 0f;
    private Material[] defaultMaterials;
    private Material[] flashingMaterials;
    public event EventHandler<OnHealthChangeEventArgs> OnHealthChange;

    //This class just allows me to use a variable with the UnityEvent. I just need the type.
    [Serializable]
    public class TakeDamageEvent : UnityEvent<float>
    {
    }
    
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

        if(rendererBody && flashMaterial)
        {
            defaultMaterials = rendererBody.materials;
            flashingMaterials = MakeMaterialArray(flashMaterial);
        }
       
        lastDamageTime = (-1 * immunityAfterDamageTimer);
    }

    public void OnTakeDamage(float damage, GameObject _instigator)
    {
        if (_instigator && _instigator.TryGetComponent<HealthController>(out var instigatorHealthController))
        {
            if (instigatorHealthController.TeamNumber == TeamNumber)
                return;
        }

        if (lastDamageTime + immunityAfterDamageTimer > Time.time)
            return;

        onDamageTextSpawn.Invoke(damage);
        
        health -= damage;
        
        OnHealthChange?.Invoke(this, new OnHealthChangeEventArgs { damageAmount = damage, currentHealth = health, owner = gameObject, instigator = _instigator });

        if (health <= 0)
        {
            bIsDead = true;
            OnHealthChange -= HealthChange;
            return;
        }

        //Don't play the blinking when the gameobject is dead
        lastDamageTime = Time.time;
        if(flashMaterial)
            StartCoroutine(Blink());
    }

    public float GetHealthFraction()
    {
        return health / maxHealth;
    } 
        
    //updates UI
    private void HealthChange(object sender, HealthController.OnHealthChangeEventArgs e)
    {
        if (healthUI)
            healthUI.SetHealth(e.currentHealth);
    }

    private IEnumerator Blink()
    {
        float endTime = Time.time + immunityAfterDamageTimer;
        while (Time.time <= endTime)
        {
            rendererBody.materials = flashingMaterials;
            yield return new WaitForSeconds(0.1f);
           
            rendererBody.materials = defaultMaterials;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private Material[] MakeMaterialArray(Material mat)
    {
        List<Material> materials = new List<Material>();
       
        foreach (var defMats in defaultMaterials)
        {
            materials.Add(flashMaterial);
        }

        return materials.ToArray();
    }
}
