using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject projectile = null;

    public void Shoot(Transform muzzle)
    {
        Instantiate(projectile, muzzle.position, Quaternion.identity);
    }
}
