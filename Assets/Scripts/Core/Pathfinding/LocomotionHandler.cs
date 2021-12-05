using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class LocomotionHandler : MonoBehaviour
{
  public float movespeed;
    public Transform target;
    [Header("Pathfinding")]
    int targetIndex = 1;
    public bool isMoving;
    Unit unit;
    [Header("Knockback Collision")]
    public bool isGettingKnockedBack;
    public Vector3 collisionSize;
    public Vector3 collisionPosOffset;
    public AnimationCurve knockbackSpeedCurve;
    public bool isCheckingForOutOfBounds; 
    public Vector3 knockbackLocation;
    public Vector2 OOBDetectionSize;
    public Vector2 targetDestination;
    Seeker seeker;
    public Path path;
    int currentWaypoint;
    float nextWaypointDistance = 3f;
    public AIPath aiPath;

    private void Start() 
    {
        
        unit = GetComponent<Unit>();
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>(); 
        
    }

    WaitForSeconds buffer = new WaitForSeconds(0.2f);

    public IEnumerator MoveToLocation(Vector2 location)
    {
        
        aiPath.isStopped = false;
        seeker.StartPath(transform.position, location, OnPathGenerationCompleted);
        while(path == null)
            yield return null;
        
        isMoving = true;
        while(!aiPath.reachedEndOfPath)
        {
            
            unit.effectsHandler.StartRunningAnimation();
            Debug.Log("Walking to path");
            UpdatePath(location);
            float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if(distance < aiPath.pickNextWaypointDist)
                currentWaypoint++;
            if(currentWaypoint >= path.vectorPath.Count)
                yield break;
            yield return buffer;
        }
        EndPathing();
    }

    public void EndPathing()
    {
        Debug.Log("reached end of path");
        isMoving = false;
        aiPath.isStopped = true;
        unit.effectsHandler.StopRunningAnimation();
    }
    void UpdatePath(Vector2 targetDestination)
    {
        if(seeker == null)
        {
            return;
        }
        if(seeker.IsDone())
        {
            Debug.Log("Requesting new path");
            seeker.StartPath(this.transform.position, targetDestination, OnPathGenerationCompleted);
        }
    }
    void OnPathGenerationCompleted(Path p)
    {
        if(!p.error)
        {
            path = p;
            Debug.Log("Set the path --------------");
            currentWaypoint = 0 ;
        }
    }

    public void MoveToTarget(Vector2 pos)
    {
        if(isMoving)
        {
            StopCoroutine("MoveToLocation");
            isMoving = false;
        }
        StartCoroutine(MoveToLocation(pos));
    }
    public void GetKnockedBack(float x, Vector3 y)
    {

    }
}
    /*private IEnumerator CheckForOutOfBounds()
    {
        while(isCheckingForOutOfBounds)
        {
            Debug.Log("Checking for OOB");
            Collider2D[] wallCollisionsArray= Physics2D.OverlapBoxAll(transform.position,OOBDetectionSize,0f, 64);
            if(wallCollisionsArray.Length > 0)
            {
                Vector3 entryDirection = wallCollisionsArray[0].transform.position - transform.position; 
                    //wallCollisionsArray = Physics2D.OverlapBoxAll(transform.position,OOBDetectionSize,0f, 64);
                    Debug.Log("Fixing player position");
                    transform.position = (transform.position - (entryDirection.normalized / 3) );

            }
            yield return new WaitForSeconds(0.25f);
        }
        


    }

    private void UnstuckUnit(Collider2D collider)
    {
        Debug.Log(collider.ClosestPoint(transform.position));
        
        isMoving = false;

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
            unit.effectsHandler.StartRunningAnimation();
           //Debug.Log(targetIndex + " " + path.Length);
            if (transform.position == currentWaypoint)
            {
                targetIndex ++;
                if (targetIndex >= path.Length)
                {
                    isMoving = false;
                    unit.effectsHandler.StopRunningAnimation();
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
        Vector3 newPos = new Vector3(UnityEngine.Random.Range(targetPosition.x -0.5f , targetPosition.x + 0.5f), UnityEngine.Random.Range(targetPosition.y -0.5f , targetPosition.y + 0.5f), 0f);
        PathRequestManager.RequestPath(transform.position, newPos, OnPathFound);
        isMoving = true;
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

public IEnumerator GetKnockedBack(float knockbackAmount, Vector3 attackOrigin)
    {
        StopCoroutine(FollowPath());

        /*if(!isCheckingForOutOfBounds)
        {
            isCheckingForOutOfBounds = true;
            StartCoroutine(CheckForOutOfBounds());
        }*/
/*
        isGettingKnockedBack = true;
        Vector3 unitPos = transform.position;
        Vector3 knockbackDirection = (unitPos - attackOrigin ).normalized;
        knockbackLocation = transform.position + (knockbackDirection * knockbackAmount); 
        Debug.Log("knockback location " + knockbackLocation);
        float timeElapsed = 0;
        float lerpDuration = 1f;

        while(timeElapsed < lerpDuration)
        {
            Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position + collisionPosOffset, collisionSize, 0f); 
            

             for (int i = 0; i < collisions.Length; i++)
             {
                 if(collisions[i].gameObject.layer == 6)
                 {  
                    Vector3 entryDirection = collisions[0].transform.position - transform.position; 
                    Debug.Log("Fixing player position");
                    transform.position = (transform.position - (entryDirection.normalized / 3) );
                     //unitPos = transform.position;
                     //knockbackLocation = Vector3.Reflect(knockbackLocation, unitPos).normalized + (unitPos);// + new Vector3(knockbackLocation.x - knockbackSpeedCurve.Evaluate(timeElapsed), knockbackLocation.y - knockbackSpeedCurve.Evaluate(timeElapsed), 0f);
                     CollideWithWall();
                     //StartCoroutine(FollowPath());
                     yield break;
                 }
             }
            Vector3 newUnitpos = Vector3.Lerp(unitPos, knockbackLocation, knockbackSpeedCurve.Evaluate(timeElapsed));
            transform.position = newUnitpos;

            timeElapsed += Time.deltaTime;



            yield return null;
        }
        //StartCoroutine(FollowPath());
        isGettingKnockedBack = false;
        
       /* if(isCheckingForOutOfBounds)
        {
            StopCoroutine(CheckForOutOfBounds());
            isCheckingForOutOfBounds = false;
        }*/
  /*     unit.isAgainstWall = false; 
        Debug.Log("Done getting knocked back");
        yield return null;
    }
    private void CollideWithWall()
    {
        Debug.Log("HIT WALL");
        unit.isAgainstWall = true;
        isGettingKnockedBack = false;
        
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(this.transform.position,knockbackLocation);
        Gizmos.DrawWireCube(transform.position, OOBDetectionSize);
        Gizmos.DrawWireCube(transform.position + collisionPosOffset, collisionSize);
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
*/