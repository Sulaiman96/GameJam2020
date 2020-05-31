using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHUD : MonoBehaviour
{
    [HideInInspector]public WeaponController currentWeapon;

    private Text text;
    private int ammoDisplay = 0;
    private int maxAmmoDisplay = 0;

    private void Awake()
    {
        var childTransform = transform.GetChild(0);
        text = childTransform.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateProjectilesUI();
    }

    private void UpdateProjectilesUI()
    {
        if (!currentWeapon)
            return;

        maxAmmoDisplay = currentWeapon.weapon.maxAmountOfProjectiles;
        ammoDisplay = maxAmmoDisplay - currentWeapon.projectiles.Count;
        text.text = string.Format("{0}/{1}", ammoDisplay, maxAmmoDisplay);
    }

}
