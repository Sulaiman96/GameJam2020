using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public GameObject camera;

    // Update is called once per frame
    void Update()
    {
        PlayerMovment();
    }

    void PlayerMovment()
    {
        float moveVertical = Input.GetAxisRaw("Vertical");
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        Vector3 newPosition = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(newPosition * movementSpeed * Time.deltaTime, Space.World);

        PlayerRotation(newPosition);
    }

    void PlayerRotation(Vector3 playerTranslation)
    {
        // When the player holds right mouse button we aim at the mouse location
        if (Input.GetMouseButton(1))
        {
            Plane PlayerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitDist = 0.0f;

            if (PlayerPlane.Raycast(ray, out hitDist))
            {
                Vector3 targetPoint = ray.GetPoint(hitDist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                targetRotation.x = 0;
                targetRotation.z = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7f * Time.deltaTime);
            }
        }
        else
        {
            // Don't set the player rotation if the value is zero
            if (playerTranslation != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerTranslation), 7f * Time.deltaTime);
        }
    }
}
