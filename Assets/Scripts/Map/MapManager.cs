using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    private GameObject player;

    [Header("Map Settings")]
    [SerializeField] private int width = 80;
    [SerializeField] private int height = 45;
    [SerializeField] private int roomMaxSize = 10;
    [SerializeField] private int roomMinSize = 6;
    [SerializeField] private int maxRooms = 30;
    [SerializeField] private int maxMonstersPerRoom = 2;

    [Header("Tiles")]
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private TileBase fogTile;

    [Header("Tilemaps")]
    [SerializeField] private Tilemap floorMap;
    [SerializeField] private Tilemap obstacleMap;
    [SerializeField] private Tilemap fogMap;

    [Header("Features")]
    [SerializeField] private List<RectangularRoom> rooms = new List<RectangularRoom>();
    [SerializeField] private List<Vector3Int> visibleTiles = new List<Vector3Int>();
    [SerializeField] private Dictionary<Vector3Int, TileData> tiles = new Dictionary<Vector3Int, TileData>();

    public TileBase FloorTile { get => floorTile; }
    public TileBase WallTile { get => wallTile; }

    public Tilemap FloorMap { get => floorMap; }
    public Tilemap ObstacleMap { get => obstacleMap; }
    public Tilemap FogMap { get => fogMap; }

    public List<RectangularRoom> Rooms { get => rooms; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ProcGen procGen = new ProcGen();
        procGen.GenerateDungeon(width, height, roomMaxSize, roomMinSize, maxRooms, maxMonstersPerRoom, rooms);

        AddTileMapToDictionary(floorMap);
        AddTileMapToDictionary(obstacleMap);

        SetupFogMap();

        //Camera.main.transform.position = new Vector3(40, 20.25f, -10);
        Camera.main.orthographicSize = 7;
    }

    public bool InBounds(int x, int y) => 0 <= x && x < width && 0 <= y && y < height;

    public void CreateEntity(string entity, Vector2 position)
    {
        switch (entity)
        {
            case "Player":
                Instantiate(Resources.Load<GameObject>("Player"), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity).name = "Player";
                Camera.main.transform.position = new Vector3(position.x, position.y, -10);
                Camera.main.transform.parent = FindObjectOfType<Player>().transform;
                break;
            case "Monster":
                Instantiate(Resources.Load<GameObject>("Monster"), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity).name = "Monster";
                break;
            case "Abom":
                Instantiate(Resources.Load<GameObject>("Abom"), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity).name = "Abom";
                break;
            default:
                Debug.Log("Entity not found");
                break;
        }
        
    }

    public void UpdateFogMap(List<Vector3Int> playerFOV)
    {
        foreach(Vector3Int pos in visibleTiles)
        {
            if (!tiles[pos].isExplored)
            {
                tiles[pos].isExplored = true; ;
            }
            tiles[pos].isVisible = false;
            fogMap.SetColor(pos, new Color(1.0f, 1.0f, 1.0f, 0.5f));
        }

        visibleTiles.Clear();

        foreach(Vector3Int pos in playerFOV)
        {
            tiles[pos].isVisible = true;
            fogMap.SetColor(pos, Color.clear);
            visibleTiles.Add(pos);
        }
    }

    public void SetEntitiesVisibilities()
    {
        foreach (Entity entity in GameManager.instance.Entities)
        {
            if (entity.GetComponent<Player>())
            {
                continue;
            }

            Vector3Int entityPosition = floorMap.WorldToCell(entity.transform.position);
            //change back to just GetComponent Later if this doesn't work.
            if (visibleTiles.Contains(entityPosition))
            {
                entity.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                entity.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    private void AddTileMapToDictionary(Tilemap tilemap)
    {
        foreach(Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos))
            {
                continue;
            }

            TileData tile = new TileData();
            tiles.Add(pos, tile);
        }
    }

    private void SetupFogMap()
    {
        foreach(Vector3Int pos in tiles.Keys)
        {
            fogMap.SetTile(pos, fogTile);
            fogMap.SetTileFlags(pos, TileFlags.None);
        }
    }

}