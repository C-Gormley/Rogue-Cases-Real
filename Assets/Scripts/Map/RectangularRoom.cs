using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RectangularRoom
{
    [SerializeField] private int x, y, width, height;

    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }


    public RectangularRoom(int x, int y, int width, int height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    public Vector2Int Center() => new Vector2Int(x + width / 2, y + height / 2);

    public Vector2Int RandomPoint() => new Vector2Int(Random.Range(x + 1, x + width -1), Random.Range(y + 1, y + height -1));

    // return area of this room as a bounds
    public Bounds GetBounds() => new Bounds(new Vector3(x, y, 0), new Vector3(width, height, 0));

    //return the area of this room as BoundsInt
    public BoundsInt GetBoundsInt() => new BoundsInt(new Vector3Int(x,y, 0), new Vector3Int(width, height, 0));

    //return true if this room overlaps another

    public bool Overlaps(List<RectangularRoom> otherRooms)
    {
        foreach(RectangularRoom otherRoom in otherRooms)
        {
            if (GetBounds().Intersects(otherRoom.GetBounds()))
            {
                return true;
            }
        }
        return false;
    }

}
