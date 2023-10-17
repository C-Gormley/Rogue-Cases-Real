using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

sealed class ProcGen
{
    
    public void GenerateDungeon(int mapWidth, int mapHeight, int roomMaxSize, int roomMinSize, int maxRooms, int maxMonstersPerRoom, List<RectangularRoom> rooms)
    {
        for (int roomNum = 0; roomNum < maxRooms; roomNum++)
        {
            int roomWidth = Random.Range(roomMinSize, roomMaxSize);
            int roomHeight = Random.Range(roomMinSize, roomMaxSize);

            int roomX = Random.Range(0, mapWidth - roomWidth -1);
            int roomY = Random.Range(0, mapHeight - roomHeight -1);

            RectangularRoom newRoom = new RectangularRoom(roomX, roomY, roomWidth, roomHeight);

            if (newRoom.Overlaps(rooms))
            {
                continue;
            }

            for (int x = roomX; x < roomX + roomWidth; x++)
            {
                for(int y = roomY; y < roomY + roomHeight; y++)
                {
                    if(x == roomX || x == roomX + roomWidth - 1 || y == roomY || y == roomY + roomHeight - 1)
                    {
                        if(SetWallTileIfEmpty(new Vector3Int(x, y, 0)))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        SetFloorTile(new Vector3Int(x, y));
                    }
                }
            }
            if(rooms.Count != 0)
            {
                TunnelBetween(rooms[rooms.Count - 1], newRoom);
                
            }
            else
            {
                
            }
            PlaceEntities(newRoom, maxMonstersPerRoom);
            rooms.Add(newRoom);
        }
        //The first room, where the player starts
        MapManager.instance.CreateEntity("Player", rooms[0].Center());
    }

    public void TunnelBetween(RectangularRoom oldRoom, RectangularRoom newRoom)
    {
        Vector2Int oldRoomCenter = oldRoom.Center();
        Vector2Int newRoomCenter = newRoom.Center();
        Vector2Int tunnelCorner;

        if(Random.value < 0.5f)
        {
            tunnelCorner = new Vector2Int(newRoomCenter.x, oldRoomCenter.y);
        }
        else
        {
            tunnelCorner = new Vector2Int(oldRoomCenter.x, newRoomCenter.y);
        }

        List<Vector2Int> tunnelCoords = new List<Vector2Int>();
        BresenhamLine.Compute(oldRoomCenter, tunnelCorner, tunnelCoords);
        BresenhamLine.Compute(tunnelCorner, newRoomCenter, tunnelCoords);

        for (int i = 0; i < tunnelCoords.Count; i++)
        {
            SetFloorTile(new Vector3Int(tunnelCoords[i].x, tunnelCoords[i].y));

            for (int x = tunnelCoords[i].x - 1; x <= tunnelCoords[i].x + 1; x++)
            {
                for(int y = tunnelCoords[i].y - 1; y <= tunnelCoords[i].y + 1; y++)
                {
                    if(SetWallTileIfEmpty(new Vector3Int(x, y, 0)))
                    {
                        continue;
                    }
                }
            }

        }

    }

    private bool SetWallTileIfEmpty(Vector3Int pos)
    {
        if (MapManager.instance.FloorMap.GetTile(pos))
        {
            return true;
        }
        else
        {
            MapManager.instance.ObstacleMap.SetTile(pos, MapManager.instance.WallTile);
            return false;
        }
    }

    private void SetFloorTile(Vector3Int pos)
    {
        if (MapManager.instance.ObstacleMap.GetTile(pos))
        {
            MapManager.instance.ObstacleMap.SetTile(pos, null);
        }
        MapManager.instance.FloorMap.SetTile(pos, MapManager.instance.FloorTile);
    }

    private void PlaceEntities(RectangularRoom newRoom, int maximumMonsters)
    {
        int numberOfMonsters = Random.Range(0, maximumMonsters + 1);
        for (int monster = 0; monster < numberOfMonsters;)
        {
            int x = Random.Range(newRoom.x, newRoom.x + newRoom.width);
            int y = Random.Range(newRoom.y, newRoom.y + newRoom.height);
            if(x == newRoom.x || x == newRoom.x + newRoom.width - 1 || y == newRoom.y || y == newRoom.y + newRoom.height - 1)
            {
                continue;
            }
            for (int entity = 0; entity < GameManager.instance.Entities.Count; entity++)
            {
                Vector3Int pos = MapManager.instance.FloorMap.WorldToCell(GameManager.instance.Entities[entity].transform.position);

                if(pos.x == x && pos.y == y)
                {
                    return;
                }
            }

            if(Random.value < 0.8f)
            {
                MapManager.instance.CreateEntity("Monster", new Vector2(x, y));
            }
            else
            {
                MapManager.instance.CreateEntity("Abom", new Vector2(x, y));
            }
            monster++;
        }
    }

}