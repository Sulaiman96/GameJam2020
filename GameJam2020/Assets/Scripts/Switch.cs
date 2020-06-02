using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    [SerializeField] private float floatStrength = 0.2f;
    [SerializeField] private bool resetSwitch = false;
    [SerializeField] private float resetTimer = 0f;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private UnityEvent onActivate = default;
    [SerializeField] private UnityEvent onDeactivate = default;

    private bool isSwitchable = true;
    private Renderer ren;
    private Material defMaterial;
    private Vector3 startingPos;

    private void Awake()
    {
        if (transform.GetChild(0))
        {
            ren = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        }
    }

    void Start()
    {
        if (ren)
         defMaterial = ren.material;

        startingPos = transform.position;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, startingPos.y + (Mathf.Sin(Time.time) * floatStrength), transform.position.z);
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
