using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 13f;
    private Vector3 desiredPosition = default;
    private float moveHorizontal = 0;
    private float moveVertical = 0;

    public CharacterController characterController;

    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        desiredPosition = new Vector3(moveHorizontal, 0.0f, moveVertical);

        PlayerMovment();
       
    }

    void PlayerMovment()
    {
        characterController.Move(desiredPosition * movementSpeed * Time.deltaTime);
    }

    public Vector3 GetPlayerDesiredPoisition()
    {
        return desiredPosition;
    }

}
