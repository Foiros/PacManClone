using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;
    Grid grid;

    void Awake() { grid = GetComponent<Grid>(); requestManager = GetComponent<PathRequestManager>(); }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos) { StartCoroutine(FindPath(startPos, targetPos)); }

    IEnumerator FindPath(Vector3 startpos, Vector3 targetPos)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        PathfindingNode startNode = grid.NodeFromWorldPoint(startpos);
        PathfindingNode targetNode = grid.NodeFromWorldPoint(targetPos);

        if(startNode.walkable && targetNode.walkable)
        {
            Heap<PathfindingNode> openSet = new Heap<PathfindingNode>(grid.MaxSize);
            HashSet<PathfindingNode> closedSet = new HashSet<PathfindingNode>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                PathfindingNode currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (PathfindingNode neighbour in grid.GetNeighbors(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
                    
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour)) { openSet.Add(neighbour); }
                        else { openSet.UpdateItem(neighbour); }
                    }
                }
            }
        }

        yield return null;
        if (pathSuccess) { waypoints = RetracePath(startNode, targetNode); }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    Vector3[] RetracePath(PathfindingNode startNode, PathfindingNode endNode)
    {
        List<PathfindingNode> path = new List<PathfindingNode>();
        PathfindingNode currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        path.Reverse();
        return waypoints;
    }

    Vector3[] SimplifyPath(List<PathfindingNode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i -1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }

            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }

    int GetDistance(PathfindingNode nodeA, PathfindingNode nodeB)
    {
        int disX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int disY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(disX > disY) 
            return 14 * disY + 10 * (disX - disY);
        return 14 * disX + 10 * (disY - disX);
    }
}
