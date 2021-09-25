using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionHandler : MonoBehaviour
{
  public float movespeed;
    [Header("Pathfinding")]
    public Transform target;
    Vector3[] path;
    int targetIndex = 1;
    public bool isMovingToTarget;
    IPathable movingUnit;

    private void Start() 
    {
        isMovingToTarget = false;
        movingUnit = GetComponent<IPathable>();
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
			path = newPath;
			targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        
        while (!isMovingToTarget)
        {

           Debug.Log(targetIndex + " " + path.Length);
            if (transform.position == currentWaypoint)
            {
                targetIndex ++;
                if (targetIndex >= path.Length)
                {
                    isMovingToTarget = true;
                    movingUnit.PerformQueuedAction();
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, movespeed);
            yield return null;
        }
        isMovingToTarget = false;
    }

    public void MoveToMouseLocation(Vector3 mousePos)
    {
        PathRequestManager.RequestPath(transform.position, mousePos, OnPathFound);
    }
    
    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i-1], path[i]);
                }
            }
        }
    }
}
