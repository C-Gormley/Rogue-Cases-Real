using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGunRefill : Consumable
{
    [SerializeField] int ammoRefill;
    public override bool Activate(Actor consumer)
    {
        if (consumer.Equipment.Weapon.GetComponent<HandGun>())
        {
            consumer.Equipment.Weapon.GetComponent<HandGun>().LoadGun(ammoRefill);
            Consume(consumer);
            return true;
        }
        else { return false; }
    }
}
