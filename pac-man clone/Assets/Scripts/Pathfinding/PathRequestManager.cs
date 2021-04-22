using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{
    readonly Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    Pathfinding pathfinding;

    bool isProcessingPath;

    private void Awake() { instance = this; pathfinding = GetComponent<Pathfinding>(); }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) 
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    private void TryProcessNext()
    {
        if (isProcessingPath || pathRequestQueue.Count <= 0) return;
        currentPathRequest = pathRequestQueue.Dequeue();
        isProcessingPath = true;
        pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    readonly struct PathRequest
    {
        public readonly Vector3 pathStart;
        public readonly Vector3 pathEnd;
        public readonly Action<Vector3[], bool> callback;

        public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> callback)
        {
            pathStart = start;
            pathEnd = end;
            this.callback = callback;
        }
    }
}
