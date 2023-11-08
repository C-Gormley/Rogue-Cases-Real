using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

sealed class HandGun : Equippable
{
    public int ammo, maxAmmo;
    public HandGun()
    {
        EquipmentType = EquipmentType.Firearm;
        PowerBonus = 2;
        ammo = 6;
        maxAmmo = 6;
    }

    public void LoadGun(int pack)
    {
        int newAmmoAmount = ammo + pack;
        if (ammo == maxAmmo)
        {
             newAmmoAmount = 0;
        }
        if(newAmmoAmount > maxAmmo)
        {
            newAmmoAmount = maxAmmo;
        }

        ammo = newAmmoAmount;
    }
    private void OnValidate()
    {
        if (gameObject.transform.parent)
        {
            gameObject.transform.parent.GetComponent<Equipment>().Weapon = this;
        }
    }
}
