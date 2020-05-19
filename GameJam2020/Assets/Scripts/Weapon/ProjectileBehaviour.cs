using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimeController))]
public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] private float lifeSpan = 60f;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private int totalBounceAmount = 10;

    private bool bIsDestroyingProjectile = false;
    private Rigidbody rb;
    private TimeController timeController;
    private float timeDelation = 1;
    private int currentBounce = 0;

    public EventHandler OnProjectileDestroyed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        timeController = GetComponent<TimeController>();
    }

    private void Start()
    {
        if (timeController)
            timeController.OnTimeDilationChange += OnTimeDilationChange;
        else
            Debug.LogWarning("Can't find time controller in " + GetType());
    }

    private void OnTimeDilationChange(object sender, TimeController.OnTimeDilationChangeEventArgs e)
    {
        timeDelation = e.newTimeDilation;
      
        Vector3 prefVelocity = rb.velocity;
        rb.velocity = Vector3.zero;
        rb.AddForce((prefVelocity).normalized * projectileSpeed * timeDelation, ForceMode.Impulse);
    }

    public void LaunchProjectile(Vector3 targetLocation)
    {

        Vector3 directionToLaunch = (targetLocation - transform.position).normalized;
        directionToLaunch.y = 0;
        rb.velocity = Vector3.zero;
        rb.AddForce(directionToLaunch * projectileSpeed * timeDelation, ForceMode.Impulse);

        if (bIsDestroyingProjectile == true)
            return;

        Destroy(gameObject, lifeSpan);
        bIsDestroyingProjectile = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (++currentBounce >= totalBounceAmount)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnProjectileDestroyed?.Invoke(this.gameObject, EventArgs.Empty);
    }

}
