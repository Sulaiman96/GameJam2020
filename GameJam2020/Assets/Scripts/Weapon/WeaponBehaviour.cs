using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject weaponToSpawn = null;
    [SerializeField] private Transform weaponSpawnPosition = null;
    [SerializeField] private Transform muzzle = null;
    private WeaponController weapon;
    void Start()
    {
        Instantiate(weaponToSpawn, weaponSpawnPosition);
        weapon = weaponToSpawn.GetComponent<WeaponController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("HERE");
            weapon.Shoot(muzzle);
        }
    }
}
