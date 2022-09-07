using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private MapManager mapManager;

    private List<Vector3Int> lastPaint;

    private Vector3Int cellPosStart = Vector3Int.one;
    private Vector3Int cellPosEnd = Vector3Int.one;
    private bool flagChange;


    private void Start()
    {
        lastPaint = new List<Vector3Int>();
    }

    // Update is called once per frame
    void Update()
    {
        flagChange = false;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = mapManager.terrain.WorldToCell(worldPos);
            cellPos.z = 0;
            if(mapManager.tileExist(cellPos) && !cellPos.Equals(cellPosStart) && !cellPos.Equals(cellPosEnd))
            {
                mapManager.color(cellPosStart, Color.white);
                cellPosStart = cellPos;
                mapManager.color(cellPosStart, Color.green);
                flagChange = true;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = mapManager.terrain.WorldToCell(worldPos);
            cellPos.z = 0;
            if (mapManager.tileExist(cellPos) && !cellPos.Equals(cellPosStart) && !cellPos.Equals(cellPosEnd))
            {
                mapManager.color(cellPosEnd, Color.white);
                cellPosEnd = cellPos;
                mapManager.color(cellPosEnd, Color.red);
                flagChange = true;
            }
        }


        if ((cellPosEnd.z == 0) && (cellPosStart.z == 0) && flagChange)
        {
            List<Vector3Int> path = mapManager.GetPath(cellPosStart, cellPosEnd);
            path.Remove(cellPosStart);
            path.Remove(cellPosEnd);

            mapManager.colorList(lastPaint, Color.white);
            lastPaint = path;

            mapManager.colorList(path, Color.blue);
            mapManager.color(cellPosStart, Color.green);
            mapManager.color(cellPosEnd, Color.red);
        }
    }
}
