using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] private float lifeSpan = 15f;
    [SerializeField] private float projectileSpeed = 10f;

    private bool bIsDestroyingProjectile = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void LaunchProjectile(Vector3 targetLocation)
    {

        Vector3 directionToLaunch = (targetLocation - transform.position).normalized;
        directionToLaunch.y = 0;
        rb.velocity = Vector3.zero;
        rb.AddForce(directionToLaunch * projectileSpeed, ForceMode.Impulse);

        if (bIsDestroyingProjectile == true)
            return;

        Destroy(gameObject, lifeSpan);
        bIsDestroyingProjectile = true;
    }

}
