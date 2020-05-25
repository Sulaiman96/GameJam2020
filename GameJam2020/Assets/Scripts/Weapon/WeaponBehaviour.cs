﻿using System;
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

    public bool isSwinging { get; set; }

    private WeaponController weapon;
    private GameObject currentWeapon;
    private PlayerMovement playerMovement;
    private TimeController timeController;
    private float timeDelation = 1;
    private Vector3 targetLocation = default;
    private List<ProjectileBehaviour> projectilesHit = new List<ProjectileBehaviour>();
    private Animator animController;
    private MouseHandler mouseHandler;
    private bool isHitting { get; set; } = false;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        timeController = GetComponent<TimeController>();
        mouseHandler = GetComponent<MouseHandler>();
        animController = GetComponent<Animator>();
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
            if (!mouseHandler.IsValidAimDirection(targetLocation))
                return;

            StartAttack(); 
        }
        else if (Input.GetMouseButton(1))
        {
            RotateToTarget(targetLocation);
        } 
        else
        {

            // Don't set the player rotation if the value is zero
            if (playerMovement && playerMovement.GetPlayerDesiredPoisition() != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerMovement.GetPlayerDesiredPoisition()), (turnSpeed * Time.deltaTime) * timeDelation);
                
        }

        if(isHitting)
            HitProjectile();
    }

    private void OnTimeDilationChange(object sender, TimeController.OnTimeDilationChangeEventArgs e)
    {
        timeDelation = e.newTimeDilation;
    }

    private void StartAttack()
    {
        if (isSwinging)
            return;

        // Check if we are not in the swing state
        if(animController && !animController.GetCurrentAnimatorStateInfo(0).IsName("Swing"))
        {
            isSwinging = true;
            animController.SetBool("IsSwinging", true);
            // we set is swinging back false when animation has finish in the swinging state machine class 
        }

    }

    public void StopAttacking()
    {
        // this is called when the animation has transistion from the swing animation state
        isSwinging = false;
        if(animController)
            animController.SetBool("IsSwinging", false); isSwinging = true;
    }

    public void BeginSwing()
    {
        projectilesHit.Clear();
        isHitting = true;
    }

    public void EndSwing()
    {
        isHitting = false;
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
        if (!hitTransform)
            return;

       Collider[] objectsHit = Physics.OverlapSphere(hitTransform.position, hitRange);

        foreach (var hitObj in objectsHit)
        {
            var projectileBehaviour = hitObj.GetComponent<ProjectileBehaviour>();
            if (projectileBehaviour && !projectilesHit.Contains(projectileBehaviour))
            {
                projectilesHit.Add(projectileBehaviour);
                projectileBehaviour.LaunchProjectile(targetLocation);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(hitTransform.position, hitRange);
    }

  

}
