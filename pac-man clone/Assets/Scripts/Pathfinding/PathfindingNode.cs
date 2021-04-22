using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode : IHeapItem<PathfindingNode>
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX, gridY;

    public int gCost, hCost, HeapIndex, movementPenalty;

    public PathfindingNode parent;

    public PathfindingNode(bool walkable, Vector3 worldPosition, int gridX, int gridY, int penalty)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
        movementPenalty = penalty;
    }

    public int fCost { get { return gCost + hCost; } }

    public int heapIndex { get { return HeapIndex; } set { HeapIndex = value; } }

    public int CompareTo(PathfindingNode nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
