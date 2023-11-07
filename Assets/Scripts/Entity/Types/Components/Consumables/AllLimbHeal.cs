using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllLimbHeal : Consumable
{
    [SerializeField] private int amount = 0;

    public int Amount { get => amount; }

    public override bool Activate(Actor consumer)
    {
        int amountRecovered = consumer.GetComponent<PFighter>().AllLimbHeal(amount);

        if (amountRecovered > 0)
        {
            UIManager.instance.AddMessage($"You consume the {name}, and recover a total of {amountRecovered} to your limbs!", "#00FF00");
            Consume(consumer);
            return true;
        }
        else
        {
            UIManager.instance.AddMessage("Your health is already full.", "#808080");
            return false;
        }
    }

}
