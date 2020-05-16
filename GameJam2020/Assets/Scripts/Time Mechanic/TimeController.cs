using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private float timeDilation { get; set; }

    public event EventHandler<OnTimeDilationChangeEventArgs> OnTimeDilationChange;
    public class OnTimeDilationChangeEventArgs : EventArgs
    {
        public float newTimeDilation;
    }

    public void ChangeTime(float timeChange)
    {
        //Make sure the TimeChange value is a valid number
        timeDilation = timeChange;
        if (timeChange > 2f)
            timeDilation = 2f;
        else if (timeChange < 0f)
            timeDilation = 0f;

        OnTimeDilationChange?.Invoke(this, new OnTimeDilationChangeEventArgs { newTimeDilation = timeDilation });
    }
}
