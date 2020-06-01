using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Min(0)][SerializeField]private float moveAmountZAxis = 5f;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    public void OpenDoor()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, moveAmountZAxis * -1);
    }

    public void CloseDoor()
    {
        transform.position = startPosition;
    }
}
