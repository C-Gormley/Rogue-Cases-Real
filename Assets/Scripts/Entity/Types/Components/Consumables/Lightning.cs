using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Consumable
{
    [SerializeField] private int damage = 20;
    [SerializeField] private int maximumRange;

    public int Damage { get => damage; }
    public int MaximumRange { get => maximumRange; }

    public override bool Activate(Actor consumer)
    {
        consumer.GetComponent<Inventory>().SelectedConsumable = this;
        consumer.GetComponent<Player>().ToggleTargetMode();
        UIManager.instance.AddMessage("Bring down natures wrath on a target.", "#63FFFF");
        return false;
    }


    public override bool Cast(Actor consumer, Actor target)
    {
        UIManager.instance.AddMessage($"A bolt of lightning strikes {target.name} with an earth shattering boom, for {damage} damage.", "#FFFFFF");
        target.GetComponent<Fighter>().Hp -= damage;
        Consume(consumer);
        consumer.GetComponent<Player>().ToggleTargetMode();
        return true;
    }

}
