using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(TimeController))]
public class WeaponBehaviour : MonoBehaviour
{
  

    enum MouseStates { Valid, Invalid };

    MouseStates currentMouseState;

    [SerializeField] private float turnSpeed = 9f;
    [SerializeField] private float aimingTurnSpeed = 20f;
    [SerializeField] private GameObject startingWeaponPrefab = null;
    [SerializeField] private Transform weaponSpawnPosition = null;
    [SerializeField] private Transform muzzle = null;
    [SerializeField] private Transform hitTransform = null;
    [SerializeField] private float hitRange = 0;

    private WeaponController weapon;
    private GameObject currentWeapon;
    private PlayerMovement playerMovement;
    private TimeController timeController;
    private float timeDelation = 1;
    private bool bIsAiming { get; set; } = false;
    private Vector3 targetLocation = default;
    private Collider[] projectilesHit;
    
    private MouseHandler mouseHandler;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        timeController = GetComponent<TimeController>();
        mouseHandler = GetComponent<MouseHandler>();
    }

    void Start()  
    {
        SpawnWeapon();

        if (timeController)
            timeController.OnTimeDilationChange += OnTimeDilationChange;
        else
            Debug.LogWarning("Can't find time controller in " + GetType());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            weapon.SpawnProjectile(muzzle);
        }

        if (!mouseHandler.GetMouseLocation(ref targetLocation))
            return;

        mouseHandler.SetCursorTexture(targetLocation);

        if (Input.GetMouseButtonDown(0))
        {
            // Check if mouse location is valid 
            if(mouseHandler.IsValidAimDirection(targetLocation))
                 HitProjectile();
        }
        else if (Input.GetMouseButton(1))
        {
            bIsAiming = true;
            RotateToTarget(targetLocation);
        } 
        else
        {

            bIsAiming = false;

            // Don't set the player rotation if the value is zero
            if (playerMovement && playerMovement.GetPlayerDesiredPoisition() != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerMovement.GetPlayerDesiredPoisition()), (turnSpeed * Time.deltaTime) * timeDelation);
                
        }
    }

    private void OnTimeDilationChange(object sender, TimeController.OnTimeDilationChangeEventArgs e)
    {
        timeDelation = e.newTimeDilation;
    }

    private void SpawnWeapon()
    {
        currentWeapon = Instantiate(startingWeaponPrefab, weaponSpawnPosition) as GameObject;
        if (currentWeapon)
        {
            weapon = currentWeapon.GetComponent<WeaponController>();
            if (!weapon)
                return;

            weapon.SetOwner(gameObject);
        }
    }

    private void RotateToTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetLocation - transform.position);
        targetRotation.x = 0;
        targetRotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, (aimingTurnSpeed * Time.deltaTime) * timeDelation);
    }

    private void HitProjectile()
    {
        bIsAiming = false;

        if (!hitTransform)
            return;

        projectilesHit = Physics.OverlapSphere(hitTransform.position, hitRange);

        foreach (var hitObj in projectilesHit)
        {
            var projectileBehaviour = hitObj.GetComponent<ProjectileBehaviour>();
            if (projectileBehaviour)
            {
                projectileBehaviour.LaunchProjectile(targetLocation);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(hitTransform.position, hitRange);
    }

}
