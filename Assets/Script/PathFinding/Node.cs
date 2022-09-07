using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Node
{
    public Vector3Int pos { get; private set; }
    public Node Direction { get; private set; }
    public int G { get; private set; }
    public int H { get; private set; }
    public int F => G + H;

    
    public Node(Vector3Int _pos, Node dir, int g, int h)
    {
        pos = _pos;
        Direction = dir;
        G = g;
        H = h;
    }
    public Node(): this(Vector3Int.zero, null, 0, 0) {}
    public Node(int x, int y, Node dir, int g, int h) : this(new Vector3Int(x, y, 0), dir, g, h) {}
    public void SetDirection(Node dir) => Direction = dir;
    public void SetG(int g) => G = g;
    public void SetH(int h) => G = h;

    public override bool Equals(System.Object obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Node n = (Node)obj;
            return (pos.x == n.pos.x) && (pos.y == n.pos.y) && (pos.z == n.pos.z);
        }
    }

    public override int GetHashCode()
    {
        int hashCode = -1153592655;
        hashCode = hashCode * -1521134295 + pos.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<Node>.Default.GetHashCode(Direction);
        hashCode = hashCode * -1521134295 + G.GetHashCode();
        hashCode = hashCode * -1521134295 + H.GetHashCode();
        hashCode = hashCode * -1521134295 + F.GetHashCode();
        return hashCode;
    }
}