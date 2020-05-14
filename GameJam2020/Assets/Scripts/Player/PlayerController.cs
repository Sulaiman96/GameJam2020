using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    WeaponBehaviour WeaponBehaviour = null;

    private void Awake()
    {
        WeaponBehaviour = GetComponent<WeaponBehaviour>();
    }

}
