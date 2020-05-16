using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Dash : MonoBehaviour
{
    [SerializeField] float dashForce = 10f;
    [SerializeField] private float dashWaitTime = 0.5f;
    private CharacterController controller;
    private Vector3 currentPosition;
    private Vector3 previousPosition;
    private float smoothDash = 1f;
    private bool letPlayerDash = true;
    private bool isDashing = false;
    private bool dashCompleted = true;

    public Vector3 GetMovementDirection
    {
        get { return (currentPosition - previousPosition).normalized; }
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        previousPosition = currentPosition;
        currentPosition = transform.position;
        if (Input.GetKeyDown(KeyCode.Space) && letPlayerDash == true)
        {
            letPlayerDash = false;
            isDashing = true;
            StartCoroutine(DashWaitTime(dashWaitTime));
        }
        //TODO stop the player from being able to move whilst the dash is going on.
        if (isDashing == true ) 
        {
            DoTheDash();
        }

    }

    private void DoTheDash()
    {
        controller.Move(GetMovementDirection * dashForce * Time.deltaTime);
    }

    IEnumerator DashWaitTime(float time)
    {
        yield return new WaitForSeconds(time);
        letPlayerDash = true;
        isDashing = false;
    }
}
