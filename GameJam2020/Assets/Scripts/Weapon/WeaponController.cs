using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private GameObject owner;
    public List<GameObject> projectiles { get; private set; }
    
    public Weapon weapon;

    private void Awake()
    {
        projectiles = new List<GameObject>();
    }

    public void SetOwner(GameObject newOwner)
    {
        if (owner != newOwner)
            owner = newOwner;
    }

    public void SpawnProjectile(Transform muzzle)
    {
        if(projectiles.Count >= weapon.maxAmountOfProjectiles)
        {
            return;
        }
        
        if (weapon.projectilePrefab)
        {
            GameObject projectile = Instantiate(weapon.projectilePrefab, muzzle.position, Quaternion.identity) as GameObject;
            ProjectileBehaviour projectileBih = projectile.GetComponent<ProjectileBehaviour>();

            projectileBih.SetOwner(owner);
            projectileBih.OnProjectileDestroyed += RemoveProjectile;
            projectiles.Add(projectile);
        }
        else
            Debug.LogWarning("No projectile prefab ");
    }

    private void RemoveProjectile(object sender, EventArgs e)
    {
       GameObject projectile = sender as GameObject;
       projectile.GetComponent<ProjectileBehaviour>().OnProjectileDestroyed -= RemoveProjectile;
       projectiles.Remove(projectile);
    }

    public void Fire(Vector3 target)
    {
    }
}
