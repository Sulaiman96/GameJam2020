using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] private float lifeSpan = 15f;
    [SerializeField] private float projectileSpeed = 10f;
    private bool bIsPendingDestroy = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchProjectile(Vector3 targetLocation)
    {

        Vector3 directionToLaunch = (targetLocation - transform.position).normalized;
        directionToLaunch.y = 0;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().AddForce(directionToLaunch * projectileSpeed, ForceMode.Impulse);

        if (bIsPendingDestroy == true)
            return;

        Destroy(gameObject, lifeSpan);
        bIsPendingDestroy = true;
    }

}
