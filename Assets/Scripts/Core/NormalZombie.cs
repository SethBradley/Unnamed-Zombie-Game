using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalZombie : MonoBehaviour, ISelectable, IPathable
{
    public IAction queuedAction {get;set;}
    public MainZombie leader;
    [Header("Locomotion")]
    public LocomotionHandler locomotionHandler;
    [Header("targeting")]
    public float detectionRadius;
    public GameObject target;
    public LayerMask humanLayerMask;
    [Header("Debugging")]
    public bool showDetectionRadius;

    public bool isInMeleeRange {get {return Vector2.Distance(transform.position, target.transform.position) <= 2f ? true : false;}}
    public bool isMovingToTarget;

    public void PerformQueuedAction()
    {
        //Debug.Log("Normal Zombie got to destination");
    }

    private void Start() 
    {
        locomotionHandler = GetComponent<LocomotionHandler>();
        isMovingToTarget = false;
    }

    private void Update() 
    {

        target = DetectNearbyHumans();
        
        
        if(target != null && !isMovingToTarget)
        {
            
            isMovingToTarget = true;      
            locomotionHandler.MoveToTarget(target.transform.position);
        }
        if(target != null && TargetHasMoved())
            locomotionHandler.MoveToTarget(target.transform.position);

        if (target == null && !locomotionHandler.isMoving)
            WanderAroundLeader();

    }

    private bool TargetHasMoved()
    {
        Vector3 targetPos = locomotionHandler.path.Length > 0 ? locomotionHandler.path[locomotionHandler.path.Length - 1] : target.transform.position;
        Vector3 newTargetPos = target.transform.position;
        
         if(Vector3.Distance(targetPos, newTargetPos) >= 2f)
         {
             Debug.Log(Vector3.Distance(targetPos , newTargetPos));
             return true;
         }
         return false;
    }

    private void WanderAroundLeader()
    {
        float xCoord = UnityEngine.Random.Range((leader.transform.position.x - leader.circleRadius.bounds.extents.x), (leader.transform.position.x + leader.circleRadius.bounds.extents.x));
        float yCoord = UnityEngine.Random.Range((leader.transform.position.y - leader.circleRadius.bounds.extents.y), (leader.transform.position.y + leader.circleRadius.bounds.extents.y));
        Vector3 randomPosInLeaderRadius = new Vector3(xCoord,yCoord, 0f);

        locomotionHandler.MoveToTarget(randomPosInLeaderRadius);
    }

    private GameObject DetectNearbyHumans()
    {
        Collider2D[] nearbyHumans = Physics2D.OverlapCircleAll(transform.position, detectionRadius,humanLayerMask);
        if (nearbyHumans.Length == 0)
        {
            isMovingToTarget = false;
            return null;
        }
            
        
        else if (nearbyHumans.Length == 1)
            return nearbyHumans[0].gameObject;
        
        
        for (int i = 0; i < nearbyHumans.Length; i++)
        {
           float distanceFromTarget = Vector2.Distance(transform.position, target.transform.position);
           float distanceFromNearbyHuman = Vector2.Distance(transform.position, nearbyHumans[i].transform.position);
            
            if (distanceFromNearbyHuman <= distanceFromTarget)
                return nearbyHumans[i].gameObject;
        }
        return target;
    }

    private void OnDrawGizmos() {
        if (showDetectionRadius)
            Gizmos.DrawSphere(transform.position, detectionRadius);
    }
}
