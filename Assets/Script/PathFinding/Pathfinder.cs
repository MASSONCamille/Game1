using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pathfinder
{
    private MapManager mapManager;

    protected Pathfinder(MapManager mm) => mapManager = mm;

    public abstract List<Vector3Int> FindPath(Vector3Int startPos, Vector3Int targetPos);
}
