using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed class Sword : Equippable
{
    public Sword()
    {
        EquipmentType = EquipmentType.Melee;
        PowerBonus = 4;
    }

    private void OnValidate()
    {
        if (gameObject.transform.parent)
        {
            gameObject.transform.parent.GetComponent<Equipment>().Weapon = this;
        }
    }
}
