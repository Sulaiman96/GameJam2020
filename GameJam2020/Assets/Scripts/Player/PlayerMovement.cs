using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimeController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float movementSpeed = 13f;

    private Vector3 desiredPosition = default;
    private float moveHorizontal = 0;
    private float moveVertical = 0;
    private TimeController timeController;
    private float timeDelation = 1;
    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        timeController = GetComponent<TimeController>();
    }

    private void Start()
    {
        if (timeController)
            timeController.OnTimeDilationChange += OnTimeDilationChange;
        else
            Debug.LogWarning("Can't find time controller in " + GetType());
    }

    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        desiredPosition = new Vector3(moveHorizontal, 0.0f, moveVertical);

        PlayerMovment();
    }

    private void OnTimeDilationChange(object sender, TimeController.OnTimeDilationChangeEventArgs e)
    {
        timeDelation = e.newTimeDilation;
    }

    void PlayerMovment()
    {
        characterController.Move((desiredPosition * movementSpeed * Time.deltaTime) * timeDelation);
    }

    public Vector3 GetPlayerDesiredPoisition()
    {
        return desiredPosition;
    }

}
