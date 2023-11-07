using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class HealLimb : Consumable
{
    [SerializeField] private int amount = 0;

    public int Amount { get => amount; }

    public override bool Activate(Actor consumer)
    {
        bool canToggleMenu = true;
        foreach (BodyPart part in consumer.GetComponent<PFighter>().BodyParts)
        {
            int fullHealthLimbs = 0;
            if(part.LimbHP == part.LimbMaxHP)
            {
                fullHealthLimbs++;
            }
            if (fullHealthLimbs >= 6)
            {
                UIManager.instance.AddMessage($"Your body is in perfect health", "#808080");
                canToggleMenu = false;
                return false;
            }
        }
        if (canToggleMenu)
        {
            UIManager.instance.ToggleRecoveryScreenMenu(consumer.GetComponent<PFighter>().BodyParts, consumer, amount);
            Consume(consumer);
        }
        return true;
    }
}
