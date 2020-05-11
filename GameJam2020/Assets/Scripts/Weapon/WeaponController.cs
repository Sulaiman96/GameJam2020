using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab = null;

    void Start()
    {

    }

    public void SpawnProjectile(Transform muzzle)
    {
        if (projectilePrefab)
            Instantiate(projectilePrefab, muzzle.position, Quaternion.identity);
        else
            Debug.LogWarning("No projectile prefab ");
    }

    public void Fire(Vector3 target)
    {
    }
}
