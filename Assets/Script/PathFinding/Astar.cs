using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Astar : Pathfinder
{
    private Func<Vector3Int, List<Vector3Int>> FindNeighborsFunc;
    private Func<Vector3Int, Vector3Int, int> GetDistanceFunc;

    public Astar(MapManager mm, Func<Vector3Int, List<Vector3Int>> nf, Func<Vector3Int, Vector3Int, int> df) : base(mm)
    {
        FindNeighborsFunc = nf;
        GetDistanceFunc = df;
    }
    public void SetNF(Func<Vector3Int, List<Vector3Int>> func) => FindNeighborsFunc = func;
    public void SetDF(Func<Vector3Int, Vector3Int, int> func) => GetDistanceFunc = func;

    public override List<Vector3Int> FindPath(Vector3Int startPos, Vector3Int targetPos) // probleme
    {
        Node startNode = new Node(startPos, null, 0, GetDistanceFunc(startPos, targetPos));
        Node targetNode = new Node(targetPos, null, 0, 0);
        var toSearch = new List<Node>() { startNode };
        var processed = new List<Node>();

        while (toSearch.Any())
        {
            var current = toSearch[0];
            foreach (var t in toSearch)
                if (t.F < current.F || t.F == current.F && t.H < current.H)
                    current = t;

            processed.Add(current);
            toSearch.Remove(current);

            if (current.Equals(targetNode))
            {
                var currentPathTile = current;
                var path = new List<Vector3Int>();
                while (!currentPathTile.Equals(startNode))
                {
                    path.Insert(0, currentPathTile.pos);
                    currentPathTile = currentPathTile.Direction;
                }
                path.Insert(0, startPos);
                return path;
            }

            List<Node> neighbors = GetListNeighbors(current, targetNode);
            neighbors.RemoveAll(n => processed.Contains(n));
            foreach (Node neighbor in neighbors)
            {
                if (toSearch.Contains(neighbor))
                {
                    Node inList = toSearch[toSearch.IndexOf(neighbor)];
                    if (neighbor.G >= inList.G)
                        continue;
                    toSearch.Remove(inList);
                }
                toSearch.Add(neighbor);
            }
        }
        return null;
    }

    private List<Node> GetListNeighbors(Node n, Node targetNode)
    {
        List<Node> res = new List<Node>();
        List<Vector3Int> listPos = FindNeighborsFunc(n.pos);

        foreach (Vector3Int pos in listPos)
        {
            res.Add(new Node(pos, n, n.G + GetDistanceFunc(pos, n.pos), GetDistanceFunc(pos, targetNode.pos)));
        }

        return res;
    }
}