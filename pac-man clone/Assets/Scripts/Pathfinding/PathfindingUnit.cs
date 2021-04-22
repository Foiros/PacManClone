using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUnit : MonoBehaviour
{
    public float speed;

    private Vector3[] path;
    private int targetIndex;
    private Vector3 _target;

    protected void DetermineTarget(Vector3 myPosition, Vector3 target)
    {
        _target = target;
        PathRequestManager.RequestPath(myPosition, _target, OnPathFound);
    }

    protected void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine(nameof(FollowPath));
            StartCoroutine(nameof(FollowPath));
        }
    }

    protected IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if(transform.position == currentWaypoint)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

            yield return null;
        }
    }
}
