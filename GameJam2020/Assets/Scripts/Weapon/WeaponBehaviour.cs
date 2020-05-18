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
    [SerializeField, Range(-1,1)] private float aimAngleThreshold = -0.2f;
    [SerializeField] private Texture2D Crosshair = null;
    [SerializeField] private Texture2D invalidCrosshair = null;
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
    private Vector2 cursorOffset;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        timeController = GetComponent<TimeController>();
    }
    void Start()  
    {
        SpawnWeapon();

        if (timeController)
            timeController.OnTimeDilationChange += OnTimeDilationChange;
        else
            Debug.LogWarning("Can't find time controller in " + GetType());

        cursorOffset = new Vector2(16, 16);
        currentMouseState = MouseStates.Valid;
        if (Crosshair)
           Cursor.SetCursor(Crosshair, cursorOffset, CursorMode.Auto);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            weapon.SpawnProjectile(muzzle);
        }

        if (!GetMouseLocation(ref targetLocation))
            return;

        SetPlayerCursor();

        if (Input.GetMouseButtonDown(0))
        {
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
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, (aimingTurnSpeed * Time.deltaTime) * timeDelation);
    }

    private bool ValidAimDirection()
    {
        Vector3 direction = targetLocation - transform.position;
        float dotValue = Vector3.Dot(transform.forward.normalized, direction.normalized);

        // Return false if mouse is behind the player
        if (dotValue < aimAngleThreshold) 
            return false;
        
        return true;
    }

    private void SetPlayerCursor()
    {
        bool validMouseAim = ValidAimDirection();

        if (validMouseAim && currentMouseState != MouseStates.Valid)
        {
            if (!Crosshair)
                return;

            Cursor.SetCursor(Crosshair, cursorOffset, CursorMode.Auto);
            currentMouseState = MouseStates.Valid;
        }
        else if(!validMouseAim && currentMouseState != MouseStates.Invalid)
        {
            if (!invalidCrosshair)
                return;

            Cursor.SetCursor(invalidCrosshair, cursorOffset, CursorMode.Auto);
            currentMouseState = MouseStates.Invalid;
        }
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
