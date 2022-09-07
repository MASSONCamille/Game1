using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class testTileData : MonoBehaviour
{

    [SerializeField]
    private Tilemap tileMap;

    [SerializeField]
    private List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTiles;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tileMap.WorldToCell(worldPos);
            gridPos.z = 0;

            TileBase tile = tileMap.GetTile(gridPos);

            if (tile)
            {
                Debug.Log(dataFromTiles[tile].ToString());
            }
        }
    }
}
