using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Consumable
{
    [SerializeField] private int damage = 12;
    [SerializeField] private int radius = 3;

    public int Damage { get => damage; }
    public int Radius { get => radius; }

    public override bool Activate(Actor consumer)
    {
        consumer.GetComponent<Inventory>().SelectedConsumable = this;
        consumer.GetComponent<Player>().ToggleTargetMode(true, radius);
        UIManager.instance.AddMessage("Burn 'em all", "#63FFFF");
        return false;
    }

    public override bool Cast(Actor consumer, List<Actor> targets)
    {
        foreach(Actor target in targets)
        {
            UIManager.instance.AddMessage($"The {target.name} has it's 'flesh' scortched for {damage} damage.", "#FF0000");
            target.GetComponent<Fighter>().Hp -= damage;
        }

        Consume(consumer);
        consumer.GetComponent<Player>().ToggleTargetMode();
        return true;
    }
}
