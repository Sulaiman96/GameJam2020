using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{

    private MouseHandler mouseHandler;
    private Vector3 target;
    private GameObject arrow;

    private void Awake()
    {
        mouseHandler = gameObject.GetComponentInParent<MouseHandler>();
        arrow = gameObject.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        UpdateTargetLocation();
    }

    private void UpdateTargetLocation()
    {
        if (!mouseHandler)
            return;

        if (!mouseHandler.GetMouseLocation(ref target))
            return;

        if(mouseHandler.IsValidAimDirection(target) && arrow)
        {
            arrow.SetActive(true);
            transform.LookAt(target, Vector3.up);
        }
        else
            arrow.SetActive(false);
    }
}
