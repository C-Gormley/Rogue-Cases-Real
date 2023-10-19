using UnityEngine;

public class Action
{
   static public void EscapeAction()
    {
        Debug.Log("Quit");
        //Application.Quit();
    }

    static public bool BumpAction(Actor actor, Vector2 direction)
    {
        Actor target = GameManager.instance.GetBlockingActorAtLocation(actor.transform.position + (Vector3)direction);

        if (target)
        {
            MeleeAction(actor, target);
            return false;
        }
        else
        {
            MovementAction(actor, direction);
            return true;
        }
    }

    static public void MeleeAction(Actor actor, Actor target)
    {
        int damage = actor.GetComponent<Fighter>().Power - target.GetComponent<Fighter>().Defense;
        string attackDesc = $"{actor.name} attacks {target.name}";

        if (damage > 0)
        {
            Debug.Log($"{attackDesc} for {damage} hit points.");
            target.GetComponent<Fighter>().Hp -= damage;
        }
        else
        {
            Debug.Log($"{attackDesc} but does no damage.");
        }
        GameManager.instance.EndTurn();
    }

    static public void MovementAction(Actor actor, Vector2 direction)
    {
        //Debug.Log($"{actor.name} moves {direction]!");
        actor.Move(direction);
        actor.UpdateFieldOfView();
        GameManager.instance.EndTurn();
    }

    static public void SkipAction()
    {
        GameManager.instance.EndTurn();
    }
}
