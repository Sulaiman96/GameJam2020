using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillVolume : MonoBehaviour
{
     private const float maxDamage = 9999f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // destroy everything that enters it but

        HealthController hc;
        if(other.TryGetComponent<HealthController>(out hc))
        {
            hc.OnTakeDamage(maxDamage, gameObject);
        }

        Destroy(other.transform.root.gameObject);
    }
}
