using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Entity
{
    [SerializeField] private Consumable consumable;

    public Consumable Consumable { get => consumable; }


    private void OnValidate()
    {
        if (GetComponent<Consumable>())
        {
            consumable = GetComponent<Consumable>();
        }
    }
    // Start is called before the first frame update
    private void Start() => AddToGameManager();

    public override EntityState SaveState() => new ItemState(
      name: name,
      blocksMovement: BlocksMovement,
      isVisible: MapManager.instance.VisibleTiles.Contains(MapManager.instance.FloorMap.WorldToCell(transform.position)),
      position: transform.position,
      parent: transform.parent != null ? transform.parent.gameObject.name : ""
    );

    public void LoadState(ItemState state)
    {
        if (!state.IsVisible)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }

        if (state.Parent != "")
        {
            GameObject parent = GameObject.Find(state.Parent);
            parent.GetComponent<Inventory>().Add(this);
        }

        transform.position = state.Position;
    }
}

[System.Serializable]
public class ItemState : EntityState
{
    [SerializeField] private string parent;

    public string Parent { get => parent; set => parent = value; }

    public ItemState(EntityType type = EntityType.Item, string name = "", bool blocksMovement = false, bool isVisible = false, Vector3 position = new Vector3(),
     string parent = "") : base(type, name, blocksMovement, isVisible, position)
    {
        this.parent = parent;
    }
}