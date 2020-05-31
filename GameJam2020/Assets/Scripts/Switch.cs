using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    [SerializeField] private bool resetSwitch = false;
    [SerializeField] private float resetTimer = 0f;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private UnityEvent onActivate = default;
    [SerializeField] private UnityEvent onDeactivate = default;

    private bool isSwitchable = true;
    private Renderer ren;
    private Material defMaterial;

    private void Awake()
    {
        ren = GetComponent<Renderer>();
    }

    void Start()
    {
        if(ren)
         defMaterial = ren.material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isSwitchable == false)
            return;

        ProjectileBehaviour pb;
        if (collision.gameObject.TryGetComponent<ProjectileBehaviour>(out pb))
        {
            isSwitchable = false;
            onActivate?.Invoke();
            if (ren)
                ren.material = activeMaterial;

            if(resetSwitch)
            {
              StartCoroutine(ResetSwitch());
            }
        }
    }

    IEnumerator ResetSwitch()
    {
        yield return new WaitForSeconds(resetTimer);
        isSwitchable = true;
        onDeactivate?.Invoke();
        if (ren)
            ren.material = defMaterial;
    }

}
