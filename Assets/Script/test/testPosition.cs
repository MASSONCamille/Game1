using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class testPosition : MonoBehaviour
{
    [SerializeField]
    private Tilemap tileMap;

    private Vector3Int cellSelectGreen;
    private Vector3Int cellSelectRed;
    private bool flagRedSelect;
    private bool flagGreenSelect;
    private bool flagMSG;

    // Start is called before the first frame update
    void Start()
    {
        flagGreenSelect = false;
        flagRedSelect = false;
    }

    private Vector3Int offsetToAxial(Vector3Int pos)
    {
        int q = pos.y;
        int r = pos.x - (pos.y + (pos.y & 1)) / 2; 

        return new Vector3Int(q, r, 0);
    }

    private int axialDistance(Vector3Int pos1, Vector3Int pos2)
    {
        return (
            Mathf.Abs(pos1.x - pos2.x)
            + Mathf.Abs(pos1.x + pos1.y - pos2.x - pos2.y)
            + Mathf.Abs(pos1.y - pos2.y)
            ) / 2;
    }

    // Update is called once per frame
    void Update()
    {
        flagMSG = false;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tileMap.WorldToCell(worldPos);
            gridPos.z = 0;

            var tile = tileMap.GetTile(gridPos);

            if (tile && !gridPos.Equals(cellSelectGreen) && !gridPos.Equals(cellSelectRed))
            {
                tileMap.SetTileFlags(gridPos, TileFlags.LockColor);
                tileMap.SetColor(cellSelectRed, Color.white);

                tileMap.SetTileFlags(gridPos, TileFlags.None);
                tileMap.SetColor(gridPos, Color.red);

                cellSelectRed = gridPos;
                flagRedSelect = true;

                flagMSG = true;

                //Debug.Log("x=" + gridPos.x.ToString() + " : y=" + gridPos.y.ToString());

                //Vector3Int axialPos = offsetToAxial(gridPos);
                //Debug.Log("q=" + axialPos.x.ToString() + " : r=" + axialPos.y.ToString());
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tileMap.WorldToCell(worldPos);
            gridPos.z = 0;

            var tile = tileMap.GetTile(gridPos);

            if (tile && !gridPos.Equals(cellSelectGreen) && !gridPos.Equals(cellSelectRed))
            {
                tileMap.SetColor(cellSelectGreen, Color.white);
                tileMap.SetTileFlags(cellSelectGreen, TileFlags.LockColor);

                cellSelectGreen = gridPos;
                flagGreenSelect = true;

                tileMap.SetTileFlags(cellSelectGreen, TileFlags.None);
                tileMap.SetColor(cellSelectGreen, Color.green);

                flagMSG = true;
            }
        }

        if (flagGreenSelect && flagRedSelect && flagMSG)
        {
            int dist = axialDistance(offsetToAxial(cellSelectGreen), offsetToAxial(cellSelectRed));

            Debug.Log(dist);
        }
    }
}
