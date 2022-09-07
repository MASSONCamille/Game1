using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    public Tilemap terrain;

    [SerializeField]
    private List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTiles;

    private Pathfinder pathfinder;

    //----------> Convertion Part

    public static Vector3Int offsetToAxial(Vector3Int pos)
    {
        int q = pos.y;
        int r = pos.x - (pos.y + (pos.y & 1)) / 2;

        return new Vector3Int(q, r, pos.z);
    }
    public static Vector3Int axialToOffset(Vector3Int pos)
    {
        int q = pos.x;
        int r = pos.y;
        int y = q;
        int x = r - (q + (q & 1)) / 2;

        return new Vector3Int(x, y, pos.z);
    }

    //----------> Neighbor Part

    private Vector3Int[][] neighbors =
    {
        new Vector3Int[] { new Vector3Int(1,0,0), new Vector3Int(-1,0,0) , new Vector3Int(-1,-1,0) , new Vector3Int(0,-1,0) , new Vector3Int(0,1,0) , new Vector3Int(-1,1,0) },
        new Vector3Int[] {new Vector3Int(1,0,0), new Vector3Int(-1,0,0) , new Vector3Int(1,1,0) , new Vector3Int(0,-1,0) , new Vector3Int(0,1,0) , new Vector3Int(1,-1,0) }
    };
    public List<Vector3Int> findNeighbors(Vector3Int pos)
    {
        List<Vector3Int> res = new List<Vector3Int>();
        int parity = pos.y & 1;
        foreach (Vector3Int diff in neighbors[parity])
        {
            Vector3Int posN = pos + diff;
            if (terrain.HasTile(posN))
                res.Add(posN);
        }
        return res;
    }
    public List<Vector3Int> findWalkableNeighbors(Vector3Int pos) //TODO: verif si case occuper
    {
        List<Vector3Int> res = new List<Vector3Int>();
        List<Vector3Int> buffer = findNeighbors(pos);
        foreach (Vector3Int posNeighbor in buffer)
        {
            TileBase tile = terrain.GetTile(posNeighbor);
            if (dataFromTiles[tile].walkable)
            {
                res.Add(posNeighbor);
            }
        }
        return res;
    }
    public List<Vector3Int> findFlyableNeighbors(Vector3Int pos)
    {
        List<Vector3Int> res = new List<Vector3Int>();
        List<Vector3Int> buffer = findNeighbors(pos);
        foreach (Vector3Int posNeighbor in buffer)
        {
            TileBase tile = terrain.GetTile(posNeighbor);
            if (dataFromTiles[tile].walkable)
            {
                res.Add(posNeighbor);
            }
        }
        return res;
    }

    //----------> Distance Part

    public static int axialDistance(Vector3Int pos1, Vector3Int pos2)
    {
        return (
            Mathf.Abs(pos1.x - pos2.x)
            + Mathf.Abs(pos1.x + pos1.y - pos2.x - pos2.y)
            + Mathf.Abs(pos1.y - pos2.y)
            ) / 2;
    }
    
    public static int distance(Vector3Int pos1, Vector3Int pos2)
    {
        Vector3Int axiPos1 = offsetToAxial(pos1);
        Vector3Int axiPos2 = offsetToAxial(pos2);
        return axialDistance(axiPos1, axiPos2);
    }

    //----------> Pathfinding

    public List<Vector3Int> GetPath(Vector3Int pos1, Vector3Int pos2)
    {
        // ajouter d'eventuellement changement pour les fonction heuristique et de voisinage
        return pathfinder.FindPath(pos1, pos2);
    }

    //----------> Test Part

    public void test()
    {
        Node Ntest1 = new Node(0, 0, null, 13, 15);
        Node Ntest2 = new Node(1, 0, null, 13, 10);
        Node Ntest3 = new Node(1, 1, null, 13, 11);

        Node Ntest4 = new Node(0, 0, null, 0, 0);
        Node Ntest5 = new Node(1, 0, null, 0, 0);
        Node Ntest6 = new Node(2, 2, null, 0, 0);

        var list1 = new List<Node>() {Ntest1, Ntest2, Ntest3};
        var list2 = new List<Node>() {Ntest4, Ntest5, Ntest6};
        Debug.Log(list1.Count);
        list1.RemoveAll(l => list2.Contains(l));
        Debug.Log(list1.Count);
    }

    public void colorList(List<Vector3Int> list, Color color)
    {
        foreach (Vector3Int pos in list)
        {
            terrain.SetTileFlags(pos, TileFlags.None);
            terrain.SetColor(pos, color);
            terrain.SetTileFlags(pos, TileFlags.LockColor);
        }
    }

    public void color(Vector3Int pos, Color color)
    {
        terrain.SetTileFlags(pos, TileFlags.None);
        terrain.SetColor(pos, color);
        terrain.SetTileFlags(pos, TileFlags.LockColor);
    }

    public bool tileExist(Vector3Int pos)
    {
        return terrain.HasTile(pos);
    }

    //----------> Loop Part

    void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tiledData in tileDatas)
        {
            foreach (var tile in tiledData.tiles)
            {
                dataFromTiles.Add(tile, tiledData);
            }
        }

        pathfinder = new Astar(this, findWalkableNeighbors, distance);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
