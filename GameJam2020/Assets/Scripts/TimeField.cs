using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeField : MonoBehaviour
{
    [SerializeField]private float timeDilation = 1f;
    private float normalTimeDilation = 1f;

    private void OnTriggerEnter(Collider other)
    {
        TimeController timeController;
        if(other.TryGetComponent<TimeController>(out timeController))
            timeController.ChangeTime(timeDilation);
    }

    private void OnTriggerExit(Collider other)
    {
        TimeController timeController;
        if (other.TryGetComponent<TimeController>(out timeController))
             timeController.ChangeTime(normalTimeDilation);
    }

}
