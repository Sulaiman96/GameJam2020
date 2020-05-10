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
    private PlayerMovement playerMovement;
    private bool bIsAiming { get; set; } = false;
    private Vector3 targetLocation = default;

    void Start()  
    {
        Instantiate(weaponToSpawn.weapon, weaponSpawnPosition);
        weapon = weaponToSpawn.weapon.GetComponent<WeaponController>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            weapon.SpawnProjectile(muzzle);
            weaponToSpawn.DisplayForce();
        }

        // When the player holds right mouse button we aim at the mouse location
        if (Input.GetMouseButtonDown(0))
        {
            bIsAiming = false;
            if (GetMouseLocation(ref targetLocation))
            {
                RotateToTarget(targetLocation);
                weapon.Fire(targetLocation);
            }
        }
        else if (Input.GetMouseButton(1))
        {
            bIsAiming = true;
            if (GetMouseLocation(ref targetLocation))
            {
                RotateToTarget(targetLocation);
            }
        } 
        else
        {
            bIsAiming = false;
            // Don't set the player rotation if the value is zero
            if (playerMovement && playerMovement.GetPlayerDesiredPoisition() != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerMovement.GetPlayerDesiredPoisition()), 7f * Time.deltaTime);
        }
    }

    private bool GetMouseLocation(ref Vector3 targetLocation)
    {
        Plane PlayerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDist = 0.0f;
        if (PlayerPlane.Raycast(ray, out hitDist))
        {
            targetLocation = ray.GetPoint(hitDist);
            return true;
        }

        return false;
    }

    private void RotateToTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetLocation - transform.position);
        targetRotation.x = 0;
        targetRotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7f * Time.deltaTime);
    }

}
