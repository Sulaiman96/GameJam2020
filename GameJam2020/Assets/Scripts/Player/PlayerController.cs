﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    WeaponBehaviour WeaponBehaviour = null;

    void Start()
    {
        WeaponBehaviour = GetComponent<WeaponBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
