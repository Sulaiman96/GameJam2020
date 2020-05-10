using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    public GameObject weapon;
    [SerializeField] private float weaponForce = 10f;

    public void DisplayForce()
    {
       // Debug.Log(weaponForce);
    }
}
