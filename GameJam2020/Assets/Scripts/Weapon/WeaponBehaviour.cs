using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    [SerializeField] private Weapon weaponToSpawn = null;
    [SerializeField] private Transform weaponSpawnPosition = null;
    [SerializeField] private Transform muzzle = null;
    private WeaponController weapon;
    void Start()  
    {
        Instantiate(weaponToSpawn.weapon, weaponSpawnPosition);
        weapon = weaponToSpawn.weapon.GetComponent<WeaponController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            weapon.Shoot(muzzle);
            weaponToSpawn.DisplayForce();
        }
    }
}
