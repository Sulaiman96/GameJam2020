using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimeController))]
public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] private bool isHoming = false;
    [SerializeField] private float lifeSpan = 60f;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileDamage = 1f;
    [SerializeField] private int totalBounceAmount = 10;
    [SerializeField] private float rotationForce = 50f; //how fast the projectile rotates.

    private bool bIsDestroyingProjectile = false;
    private Rigidbody rb;
    private TimeController timeController;
    private float timeDelation = 1;
    private int currentBounce = 0;
    private MaterialHandler materialHandler;
    private float currentLifeSpan;
    
    private GameObject owner = null;
    private GameObject player;    

    public ParticleSystem destroyParticle;
    public EventHandler OnProjectileDestroyed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        timeController = GetComponent<TimeController>();
        materialHandler = GetComponent<MaterialHandler>();
        player = GameObject.FindWithTag("Player");

        currentLifeSpan = lifeSpan;
    }

    private void Update()
    {
        currentLifeSpan -= Time.deltaTime;
        if (currentLifeSpan < 0 && bIsDestroyingProjectile == false)
        {
            bIsDestroyingProjectile = true;
            DestroyProjectile();
        }
    }

    private void FixedUpdate()
    {
        //The player null check should be changed to is player dead.
        if (player == null) return;
        if (isHoming)
        {
            Vector3 direction = (GetTargetLocation() - rb.position).normalized;
            Vector3 rotationAmount = Vector3.Cross(transform.forward, direction);

            //TODO BECAUSE THE FORCE IS CONSTANTLY BEING ADDED, IT AFFECTS THE FORCE FIELD, FIND A WAY AROUND.
            rb.angularVelocity = rotationAmount * rotationForce * timeDelation;
            rb.velocity = transform.forward * projectileSpeed * timeDelation;
            
            transform.LookAt(GetTargetLocation());
        }
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
        if(materialHandler && !materialHandler.isActiveMaterial)
          materialHandler.UseActiveMaterial();

        Vector3 directionToLaunch = (targetLocation - transform.position).normalized;
        directionToLaunch.y = 0;
        rb.velocity = Vector3.zero;
        rb.AddForce(directionToLaunch * projectileSpeed * timeDelation, ForceMode.Impulse);

    }

    private void OnCollisionEnter(Collision collision)
    {
        HealthController health;
        if(collision.gameObject.TryGetComponent<HealthController>(out health))
        {
            health.OnTakeDamage(projectileDamage, owner);
        }

        if (++currentBounce >= totalBounceAmount)
            DestroyProjectile();
    }

    private void OnDestroy()
    {
        OnProjectileDestroyed?.Invoke(this.gameObject, EventArgs.Empty);
    }

    private void DestroyProjectile()
    {
        if (destroyParticle)
        {
            var spawnedImpact = Instantiate(destroyParticle.gameObject, transform.position, transform.rotation) as GameObject;
            Destroy(spawnedImpact, destroyParticle.main.duration);
        }
        
        Destroy(gameObject);
    }
    
    public Vector3 GetTargetLocation()
    {
        var enemyController = player.GetComponent<CharacterController>();
        return player.transform.position + Vector3.up * (enemyController.height * 1 / 4);
    }

    public void SetOwner(GameObject newOwner)
    {
        if(owner != newOwner)
            owner = newOwner;
    }

}
