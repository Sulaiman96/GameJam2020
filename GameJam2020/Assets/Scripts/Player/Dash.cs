using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Dash : MonoBehaviour
{
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.8f;

    private CharacterController controller;
    private Vector3 currentPosition;
    private Vector3 previousPosition;
    private bool letPlayerDash = true;
    private bool isDashing = false;
    private TrailRenderer dashEffect;

    public Vector3 GetMovementDirection
    {
        get { return (currentPosition - previousPosition).normalized; }
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        dashEffect = GetComponent<TrailRenderer>();
    }

    private void Start()
    {
        if(dashEffect)
            dashEffect.emitting = false;
    }

    private void Update()
    {
        previousPosition = currentPosition;
        currentPosition = transform.position;
        if (Input.GetKeyDown(KeyCode.Space) && letPlayerDash == true)
        {
            letPlayerDash = false;
            isDashing = true;

            if (dashEffect)
                dashEffect.emitting = true;

            StartCoroutine(DashWaitTime());
        }

        if (isDashing == true ) 
            Dashing();
    }

    private void Dashing()
    {
        controller.Move(GetMovementDirection * dashSpeed * Time.deltaTime);
    }

    IEnumerator DashWaitTime()
    {
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        if (dashEffect)
            dashEffect.emitting = false;

        yield return new WaitForSeconds(dashCooldown);
        letPlayerDash = true;
    }
}
