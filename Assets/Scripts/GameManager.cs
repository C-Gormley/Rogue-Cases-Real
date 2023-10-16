using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.Licensing;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] private float time = 0.1f;
    [SerializeField] private bool isPlayerTurn = true;

    [SerializeField] private int entitytNum = 0;
    [SerializeField] private List<Entity> entities = new List<Entity>();

    public bool IsPlayerTurn { get => isPlayerTurn; }

    public List<Entity> Entities { get => entities; }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void StartTurn()
    {
        //Debug.Log($"(entities[entityNum].name starts its turn!");
        if (entities[entitytNum].GetComponent<Player>())
        {
            isPlayerTurn = true;
        }
        else if (entities[entitytNum].IsSentient)
        {
            Action.SkipAction(entities[entitytNum]);
        }
    }

    public void EndTurn()
    {
        if (entities[entitytNum].GetComponent<Player>())
        {
            isPlayerTurn = false;
        }

        if(entitytNum == entities.Count - 1)
        {
            entitytNum = 0;
        }
        else
        {
            entitytNum++;
        }

        StartCoroutine(TurnDelay());
    }

    private IEnumerator TurnDelay()
    {
        yield return new WaitForSeconds(time);
        StartTurn();
    }

    public void AddEntity(Entity entity)
    {
        entities.Add(entity);
    }

    public void InsertEntity(Entity entity, int index)
    {
        entities.Insert(index, entity);
    }
}
