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
    public bool isSlowed;
    public Vector3 collisionSize;
    public Vector3 collisionPosOffset;
    public AnimationCurve knockbackSpeedCurve;
    public bool isCheckingForOutOfBounds; 
    public Vector3 knockbackLocation;
    public Vector2 OOBDetectionSize;
    public Vector2 targetDestination;
    public Seeker seeker;
    public Path path;
    public int currentWaypoint;
    float nextWaypointDistance = 3f;
    public AIPath aiPath;
    public Vector2 targetLocation;
    public Rigidbody2D rb;
    [Header("Coroutines")]
    public IEnumerator move;
    public IEnumerator knockBack_Co;
    public IEnumerator slow_Co;
    public LayerMask wallLayer;
    private void Awake() 
    {
        
        unit = GetComponent<Unit>();
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>(); 
        rb = GetComponent<Rigidbody2D>();
        move = null;
        knockBack_Co = null;
        slow_Co = null;
        
        aiPath.maxSpeed = movespeed;
    }

    WaitForSeconds buffer = new WaitForSeconds(0.2f);

    public IEnumerator MoveToLocation(Vector2 location)
    {
        aiPath.canMove = true;
        seeker.StartPath(transform.position, location, OnPathGenerationCompleted);
        while(path == null)
            yield return null;
        
        unit.effectsHandler.StartRunningAnimation();
        isMoving = true;
        while(isMoving)
        {
            
            Debug.Log("Walking to path " + location);
            UpdatePath(location);
            if(currentWaypoint < 0)
                yield return null;

            
            float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * movespeed * Time.deltaTime;
            rb.AddForce(force);

            if(Vector2.Distance(rb.position, location) < aiPath.endReachedDistance)
                EndPathing();
                
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
        StopCoroutine(move);
        aiPath.canMove = false;
        unit.effectsHandler.StopRunningAnimation();
    }
    public void DisableMovement()
    {
        //aiPath.canMove = false;
        //isMoving = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        if(unit.anim.GetBool("Moving"))
            unit.effectsHandler.StopRunningAnimation();
    }
    public void EnableMovement()
    {
        //aiPath.canMove = true;
        //isMoving = true;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        unit.effectsHandler.StartRunningAnimation();
    }

    void UpdatePath(Vector2 targetDestination)
    {
        if(seeker == null)
        {
            return;
        }
        if(seeker.IsDone())
        {
     //       Debug.Log("Requesting new path");
            seeker.StartPath(this.transform.position, targetDestination, OnPathGenerationCompleted);
        }
    }
    void OnPathGenerationCompleted(Path p)
    {
        if(!p.error)
        {
            
            path = p;
            ValidatePath(p);
//            Debug.Log("Set the path --------------");
            currentWaypoint = 0 ;
        }
        else
        Debug.Log("Error in path generation");
    }

    public void MoveToTarget(Vector2 newPos)
    {
        aiPath.canMove = true;
        if(move == null)
        {
            move = MoveToLocation(newPos);
            StartCoroutine(move);
        }
        else
        {
            isMoving = false;
            StopCoroutine(move);
            move = MoveToLocation(newPos);
            StartCoroutine(move);
        }
    }
    public bool ValidatePath(Path p)
    {
        if(p.vectorPath.Count <= 2)
        {
            Debug.Log("Checking for wall");
            Vector2 direction = p.vectorPath[currentWaypoint] - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized,10f, wallLayer);
            if(hit.collider != null)
            {
                Debug.Log("path crossing a wall - ending path");
                EndPathing();
            }

        }
//         foreach(Vector3 waypoint in p.vectorPath)
//         {
// //            Debug.Log(waypoint + " Vector path count " + p.vectorPath.Count);
//         }
        return true;

    }
public IEnumerator GetKnockedBack(float knockbackAmount, Vector3 attackOrigin)
    {
        if(isGettingKnockedBack)
            yield break;
        
        unit.isAgainstWall = false;
        aiPath.canMove = false;
        isGettingKnockedBack = true;
        Vector3 unitPos = transform.position;
        Vector3 knockbackDirection = (unitPos - attackOrigin ).normalized;
        knockbackLocation = transform.position + (knockbackDirection * knockbackAmount); 
        Debug.Log("knockback location " + knockbackLocation);
        float timeElapsed = 0;
        float lerpDuration = 1f;

        while(timeElapsed < lerpDuration)
        {
            if(unit.isAgainstWall)
            {
                StopCoroutine(knockBack_Co);
                StartCoroutine(FreezeMovementForSeconds(0.5f));
            }
            Vector3 newUnitpos = Vector3.Lerp(unitPos, knockbackLocation, knockbackSpeedCurve.Evaluate(timeElapsed));
            rb.MovePosition(newUnitpos);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        aiPath.canMove = true;
        isGettingKnockedBack = false;
        Debug.Log("Done getting knocked back");
        yield return null;
    }
    private void CollideWithWall()
    {
        Debug.Log("HIT WALL");
        unit.isAgainstWall = true;
        isGettingKnockedBack = false;

        
    }
    IEnumerator FreezeMovementForSeconds(float waitTime)
    {
        WaitForSeconds buffer = new WaitForSeconds(waitTime); 
        float timeElapsed = 0;
        while(timeElapsed < waitTime)
        {
            isMoving = false;
            aiPath.canMove = false;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        isMoving = true;
        aiPath.canMove = true;
    }

    public IEnumerator SlowMovement()
    {
        //if unit is already slowed
        if(isSlowed)
            yield break;

        isSlowed = true;
        while(isSlowed)
        {
            aiPath.maxSpeed = 1f;
            yield return new WaitForSeconds(2f);
            isSlowed = false;
        }
        aiPath.maxSpeed = movespeed;
    }
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.layer == 6 && isGettingKnockedBack)
        {
            CollideWithWall();
        }
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