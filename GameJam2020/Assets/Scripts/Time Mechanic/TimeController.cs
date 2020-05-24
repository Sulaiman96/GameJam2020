using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private float timeDilation { get; set; }
    private Animator anim;

    public event EventHandler<OnTimeDilationChangeEventArgs> OnTimeDilationChange;
    public class OnTimeDilationChangeEventArgs : EventArgs
    {
        public float newTimeDilation;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ChangeTime(float timeChange)
    {
        // Make sure the TimeChange value is a valid number
        timeDilation = timeChange;
        if (timeChange > 2f)
            timeDilation = 2f;
        else if (timeChange < 0f)
            timeDilation = 0f;

        // Change playback speed for all animations if game object has animator 
        if(anim)
            anim.speed = timeDilation;

        OnTimeDilationChange?.Invoke(this, new OnTimeDilationChangeEventArgs { newTimeDilation = timeDilation });
    }
}
