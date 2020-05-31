using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    public GameObject projectilePrefab = null;
    public int maxAmountOfProjectiles;
    
}
