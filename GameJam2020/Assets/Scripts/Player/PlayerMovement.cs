using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public GameObject camera;
    private Vector3 desiredPosition = default;

    // Update is called once per frame
    void Update()
    {
        PlayerMovment();
    }

    void PlayerMovment()
    {
        float moveVertical = Input.GetAxisRaw("Vertical");
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        desiredPosition = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(desiredPosition * movementSpeed * Time.deltaTime, Space.World);
    }

    public Vector3 GetPlayerDesiredPoisition()
    {
        return desiredPosition;
    }
}
