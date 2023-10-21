using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private bool blocksMovement;


    AdamMilVisibility algorithm;

    public bool BlocksMovement { get => blocksMovement; set => blocksMovement = value; }

    public void AddToGameManager()
    {
        GameManager.instance.AddEntity(this);
    }

    public void Move(Vector2 direction)
    {
        if (MapManager.instance.IsValidPosition(transform.position + (Vector3)direction))
        {
            transform.position += (Vector3)direction;
        }
    }

 
}
