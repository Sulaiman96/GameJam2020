using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TimeController))]
public class ProjectileBehaviour : MonoBehaviour
{
    #region Variables
    [Header("Homing Properties")]
    [SerializeField] private bool isHoming = false;
    [SerializeField] Color blastRadiusColour = Color.green;
    [SerializeField] Color DamageRadiusColour = Color.red;
    [SerializeField] private float rotationForce = 50f;
    [SerializeField] private float explosionBlastRadius = 7f;
    [SerializeField] private float explosionDamageRadius = 3f;
    [SerializeField] private float explosionForce = 500f;
    [SerializeField] private float explosionDamage = 5f;
    [SerializeField] private float explosionWindUpTime = 1f;
    [Range(0,1)]
    [SerializeField] private float windupProjectilePercentageSpeed = 0.5f;
    [SerializeField] private ParticleSystem explosionParticleEffect = null;
    [SerializeField] private AudioClip onExplosionClip;
    [SerializeField] private UnityEvent onWindUp;
    

    private bool triggerExplosion = false;
    
    [Header("Projectile Properties")]
    [SerializeField] private int totalBounceAmount = 10;
    [SerializeField] private float lifeSpan = 60f;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileDamage = 1f;
    [SerializeField] private ParticleSystem destroyParticle;
    [SerializeField] private UnityEvent onImpact;

    public EventHandler OnProjectileDestroyed;
    
    private bool bIsDestroyingProjectile = false;
    private Rigidbody rb;
    private TimeController timeController;
    private float timeDelation = 1;
    private int currentBounce = 0;
    private MaterialHandler materialHandler;
    private float currentLifeSpan;
    private GameObject owner = null;
    private GameObject player;
    private TrailRenderer trailRenderer;
    
    #endregion
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        timeController = GetComponent<TimeController>();
        materialHandler = GetComponent<MaterialHandler>();
        trailRenderer = GetComponent<TrailRenderer>();
        player = GameObject.FindWithTag("Player");
        
    }
    
    private void Start()
    {
        if (timeController)
            timeController.OnTimeDilationChange += OnTimeDilationChange;
        else
            Debug.LogWarning("Can't find time controller in " + GetType());

       currentLifeSpan = lifeSpan;
    }

    private void Update()
    {
        currentLifeSpan -= Time.deltaTime;
        if (currentLifeSpan <= 0f && !bIsDestroyingProjectile)
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
            if (triggerExplosion)
            {
                rb.angularVelocity = rotationAmount * rotationForce * windupProjectilePercentageSpeed;
                rb.velocity = transform.forward * projectileSpeed * windupProjectilePercentageSpeed;
            }
            else
            {
                //TODO BECAUSE THE FORCE IS CONSTANTLY BEING ADDED, IT AFFECTS THE FORCE FIELD, FIND A WAY AROUND.
                rb.angularVelocity = rotationAmount * rotationForce;
                rb.velocity = transform.forward * projectileSpeed;
            }
            
            transform.LookAt(GetTargetLocation());
        }
        
    }

    #region Private Methods
    private void OnTimeDilationChange(object sender, TimeController.OnTimeDilationChangeEventArgs e)
    {
        timeDelation = e.newTimeDilation;
      
        Vector3 prefVelocity = rb.velocity;
        rb.velocity = Vector3.zero;
        rb.AddForce((prefVelocity).normalized * projectileSpeed * timeDelation, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggerExplosion)
        {
            triggerExplosion = true;
            onWindUp.Invoke();
            //TODO Wait for a few seconds (during this time play a bomb timer and some effect that shows its about to blow up)
            StartCoroutine(ExplosionWindUp());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isHoming)
        {
            DestroyProjectile();
        }
        
        if(collision.gameObject.TryGetComponent(out HealthController health))
        {
            health.OnTakeDamage(projectileDamage, owner);
        }

        if (++currentBounce >= totalBounceAmount)
        {
            DestroyProjectile();
            return;
        }

        onImpact.Invoke();
    }

    private void OnDestroy()
    {
        OnProjectileDestroyed?.Invoke(this.gameObject, EventArgs.Empty);
    }

    private void DestroyProjectile()
    {
        if (isHoming)
        {
            if (explosionParticleEffect)
            {
                //TODO FIX THE CAMERA SHAKE 
                //StartCoroutine(cameraShake.Shake(0.5f, .4f));
                var explosion = Instantiate(explosionParticleEffect.gameObject, transform.position, transform.rotation);
                Destroy(explosion, explosionParticleEffect.main.duration);
            }
            ExplosionBlast();
            ExplosionDamage();
        }
        else
        {
            if (destroyParticle)
            {
                var spawnedImpact = Instantiate(destroyParticle.gameObject, transform.position, transform.rotation);
                Destroy(spawnedImpact, destroyParticle.main.duration);
            }
        }
        Destroy(gameObject);
    }

    private void ExplosionDamage()
    { 
        AudioSource.PlayClipAtPoint(onExplosionClip, gameObject.transform.position);
        var colliders = Physics.OverlapSphere(transform.position, explosionDamageRadius);
              foreach (Collider nearByObjects in colliders)
              {
                  HealthController hc = nearByObjects.GetComponent<HealthController>();
                  if (hc != null)
                  {
                      hc.OnTakeDamage(explosionDamage, owner);
                  }
              }
    }

    public void SetTrailColour(Gradient trailColor)
    {
        trailRenderer.colorGradient = trailColor;
    }

    private void ExplosionBlast()
    {
        var colliders = Physics.OverlapSphere(transform.position, explosionBlastRadius);
        foreach (Collider nearByObjects in colliders)
        {
            Rigidbody rb = nearByObjects.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //BUG THE PLAYER DOES NOT GET PUSHED BACK BECAUSE OF CHARACTER CONTROLLER.
                rb.AddExplosionForce(explosionForce, transform.position, explosionBlastRadius);
            }
        }
    }

    IEnumerator ExplosionWindUp()
    {
        yield return new WaitForSeconds(explosionWindUpTime);
        bIsDestroyingProjectile = true;
        DestroyProjectile();
    }
    
    #endregion
    
    #region Public Methods
    public void LaunchProjectile(Vector3 targetLocation)
    {
        Vector3 directionToLaunch = (targetLocation - transform.position).normalized;
        directionToLaunch.y = 0;
        rb.velocity = Vector3.zero;
        rb.AddForce(directionToLaunch * projectileSpeed * timeDelation, ForceMode.Impulse);

    }
    
    public Vector3 GetTargetLocation()
    {
        var enemyController = player.GetComponent<CharacterController>();
        return player.transform.position + Vector3.up * (enemyController.height * 1 / 4);
    }

    public void SetProjectileLayer(string layer)
    {
        gameObject.layer = LayerMask.NameToLayer(layer);
    }

    public void SetOwner(GameObject newOwner)
    {
        if(owner != newOwner)
            owner = newOwner;
    }

    public void SetActiveMaterial(Material activeMaterial)
    {
        if(materialHandler)
            materialHandler.UseActiveMaterial(activeMaterial);
    }

    public bool IsHoming => isHoming;

    #endregion

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = blastRadiusColour;
        Gizmos.DrawWireSphere(transform.position, explosionBlastRadius);
        Gizmos.color = DamageRadiusColour;
        Gizmos.DrawWireSphere(transform.position, explosionDamageRadius);
    }
    #endregion
}
