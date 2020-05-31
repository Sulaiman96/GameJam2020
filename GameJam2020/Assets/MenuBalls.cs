using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBalls : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 20f;
    private Rigidbody rb;
    private TimeController timeController;
    private float timeDilation = 1;
    private void Awake()
    {
        timeController = GetComponent<TimeController>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        timeController.OnTimeDilationChange += TimeChange;
        rb.velocity = transform.right * initialSpeed * timeDilation;
    }
    

    private void TimeChange(object sender, TimeController.OnTimeDilationChangeEventArgs e)
    {
        timeDilation = e.newTimeDilation;
        rb.velocity = rb.velocity.normalized * initialSpeed * timeDilation;
    }
}
