using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionHandler : MonoBehaviour
{
  public float movespeed;
    public Transform target;
    [Header("Pathfinding")]
    public Vector3[] path;
    int targetIndex = 1;
    public bool isMoving;
    Unit movingUnit;



    private void Start() 
    {
        movingUnit = GetComponent<Unit>();

    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            Unit unit = this.GetComponent<Unit>();
            target =  unit.target != null ? unit.target.transform : null;
			path = newPath;
			targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    public IEnumerator FollowPath()
    {
        //Debug.Log("Path follow started");
        Vector3 currentWaypoint = path.Length >= 1 ? path[0] : transform.position;
        isMoving = true;
        
        while (isMoving)
        {

           //Debug.Log(targetIndex + " " + path.Length);
            if (transform.position == currentWaypoint)
            {
                targetIndex ++;
                if (targetIndex >= path.Length)
                {
                    isMoving = false;
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, movespeed * Time.deltaTime);
                

            yield return null;
        }
        
    }

    public void MoveToMouseLocation(Vector3 mousePos)
    {
        PathRequestManager.RequestPath(transform.position, mousePos, OnPathFound);
    }
    public void MoveToTarget(Vector3 targetPosition)
    {
        //Debug.Log("Moving to target");
        PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
    }

    public void UpdatePathToTarget(Transform _target)
        {
            Vector3 targetPos = path.Length > 0 ? path[path.Length - 1] : _target.transform.position;  
            Vector3 newTargetPos = _target.position;          
            if(Vector3.Distance(targetPos, newTargetPos) >= 1.5f)
            {
                //StopCoroutine(FollowPath());
//                Debug.Log("Updating path DO NOT WANT TO SEE ME OFTEN");
                MoveToTarget(_target.position);
                //StartCoroutine(FollowPath());
            }
            return; 
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
