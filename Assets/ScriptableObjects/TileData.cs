using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(fileName = "TileData", menuName = "ScriptableObjects/TileData")]
public class TileData : ScriptableObject
{
    public TileBase[] tiles;

    public bool walkable;
    public bool flyable;
}
