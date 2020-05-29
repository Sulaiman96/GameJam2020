using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimeController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 13f;

    private Vector3 desiredPosition = default;
    private float moveHorizontal = 0;
    private float moveVertical = 0;
    private TimeController timeController;
    private float timeDelation = 1;
    private CharacterController characterController;
    private Animator animController;
    private float gravity = 0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        timeController = GetComponent<TimeController>();
        animController = GetComponent<Animator>();
    }

    private void Start()
    {
        if (timeController)
            timeController.OnTimeDilationChange += OnTimeDilationChange;
        else
            Debug.LogWarning("Can't find time controller in " + GetType());

        animController.applyRootMotion = false;

        gravity = Physics.gravity.y;
    }

    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        desiredPosition = new Vector3(moveHorizontal, 0.0f, moveVertical);
        UpdateMovementAnimation(desiredPosition);
        PlayerMovment();
    }

    private void OnTimeDilationChange(object sender, TimeController.OnTimeDilationChangeEventArgs e)
    {
        timeDelation = e.newTimeDilation;
    }

    void PlayerMovment()
    {
        Vector3 moveDirection = (desiredPosition * movementSpeed * Time.deltaTime) * timeDelation;
        moveDirection.y += gravity * Time.deltaTime;
        characterController.Move(moveDirection);
    }

    public Vector3 GetPlayerDesiredPoisition()
    {
        return desiredPosition;
    }

    private void UpdateMovementAnimation(Vector3 direction)
    {

        // Play animation in the player relative facing direction
        if (animController)
        {
           
            Vector3 dir = direction;
            if (direction.magnitude > 1.0f)
            {
                dir = direction.normalized;
            }

            dir = transform.InverseTransformDirection(dir);

            animController.SetFloat("VelX", dir.x, 0.05f, Time.deltaTime);
            animController.SetFloat("VelY", dir.z, 0.05f, Time.deltaTime);
        }
    }
        

}
