using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;
    public float smooth = 0.3f;

    [Tooltip("The distance between the player and camera")]
    public float height;
    public float cameraZOffset = 7.0f;
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        Vector3 position = new Vector3();

        position.x = player.position.x;
        position.z = player.position.z - cameraZOffset;
        position.y = player.position.y + height;
        transform.position = Vector3.SmoothDamp(transform.position, position, ref velocity, smooth);
    }
}
