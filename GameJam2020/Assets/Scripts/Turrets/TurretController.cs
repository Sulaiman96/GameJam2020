using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

public class TurretController : MonoBehaviour
{
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private Color colour = Color.black;
    [SerializeField] private GameObject projectile = null;
    [SerializeField] private float timeBetweenAttack = 2f;
    [SerializeField] private Transform turretBarrel = null;
    [SerializeField] private Material turretActiveMaterial = null;
    [SerializeField] private UnityEvent OnFire = default;

    private HealthController healthController;
    private GameObject player;    
    private float timeSinceLastAttack = 0f;

    private void Awake()
    {
        healthController = GetComponent<HealthController>();
        player = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        timeSinceLastAttack = timeBetweenAttack;
        healthController.OnHealthChange += HealthChange;
    }

    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack > timeBetweenAttack && InAttackRange())
        {
            Attack();
            timeSinceLastAttack = 0;
        }
        transform.LookAt(player.transform);
    }
    
    private void Attack()
    {
        GameObject p = Instantiate(projectile, turretBarrel.position, Quaternion.identity);
        var projectileBehaviourComponent = p.GetComponent<ProjectileBehaviour>();
        projectileBehaviourComponent.SetOwner(gameObject);
        projectileBehaviourComponent.SetActiveMaterial(turretActiveMaterial);
        projectileBehaviourComponent.LaunchProjectile(projectileBehaviourComponent.GetTargetLocation());
        OnFire.Invoke();
    }
    
    private bool InAttackRange()
    {
        return Vector3.Distance(player.transform.position, transform.position) < attackRange;
    }

    private void HealthChange(object sender, HealthController.OnHealthChangeEventArgs e)
    {
        if (e.currentHealth <= 0)
        {
            healthController.OnHealthChange -= HealthChange;
            Destroy(gameObject);
        }
    }

    #region Gizmos
    //Called by Unity to draw gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = colour;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    #endregion
}
