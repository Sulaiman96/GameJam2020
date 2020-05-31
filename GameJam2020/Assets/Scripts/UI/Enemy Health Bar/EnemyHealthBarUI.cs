using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField] private HealthController healthController = null;
    [SerializeField] private RectTransform foreground = null;
    [SerializeField] private Canvas rootCanvas = null;
    
    
    void Update()
    {
        if(Mathf.Approximately(healthController.GetHealthFraction(), 0)
        || Mathf.Approximately(healthController.GetHealthFraction(), 1))
        {
            rootCanvas.enabled = false;
            return;
        }
        rootCanvas.enabled = true;
        foreground.localScale = new Vector3(healthController.GetHealthFraction(), 1,1 );
    }
}
