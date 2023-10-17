using UnityEngine;

public class Action
{
   static public void EscapeAction()
    {
        Debug.Log("Quit");
        //Application.Quit();
    }

    static public bool BumpAction(Entity entity, Vector2 direction)
    {
        Entity target = GameManager.instance.GetBlockingEntityAtLocation(entity.transform.position + (Vector3)direction);

        if (target)
        {
            MeleeAction(target);
            return false;
        }
        else
        {
            MovementAction(entity, direction);
            return true;
        }
    }

    static public void MeleeAction(Entity target)
    {
        Debug.Log($"You Kick the {target.name}, ouch!");
        GameManager.instance.EndTurn();
    }

    static public void MovementAction(Entity entity, Vector2 direction)
    {
        //Debug.Log($"{entity.name} moves {direction]!");
        entity.Move(direction);
        entity.UpdateFieldOfView();
        GameManager.instance.EndTurn();
    }

    static public void SkipAction(Entity entity)
    {
        if (entity.GetComponent<Player>())
        {
            //Debug.Log("You chose to skip your turn.");
        }
        else
        {
            //Debug.Log($"The {entity.name} doesnt' know what to do.");
        }
        GameManager.instance.EndTurn();
    }
}
