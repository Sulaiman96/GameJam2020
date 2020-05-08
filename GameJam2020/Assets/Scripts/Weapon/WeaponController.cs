using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab = null;
    private GameObject projectile = null;

    void Start()
    {
    }

    public void SpawnProjectile(Transform muzzle)
    {
        if (projectilePrefab)
            projectile = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity) as GameObject;
        else
            Debug.LogWarning("No projectile prefab ");
    }

    public void Fire(Vector3 target)
    {
        if(projectile)
        {
            Vector3 dir = target - projectile.transform.position;
            dir.y = 0;
            projectile.GetComponent<Rigidbody>().AddForce(dir.normalized * 10f, ForceMode.Impulse);
        }
    }
}
