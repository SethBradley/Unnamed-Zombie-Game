using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalZombie : MonoBehaviour, ISelectable, IPathable
{
    public IAction queuedAction {get;set;}
    
    [Header("Locomotion")]
    public LocomotionHandler locomotionHandler;
    [Header("targeting")]
    public float detectionRadius;
    public GameObject target;
    public LayerMask humanLayerMask;
    [Header("Debugging")]
    public bool showDetectionRadius;

    bool isInMeleeRange {get {return Vector2.Distance(transform.position, target.transform.position) <= 2f ? true : false;}}


    public void PerformQueuedAction()
    {
        Debug.Log("Normal Zombie got to destination");
    }

    private void Start() 
    {
        locomotionHandler = GetComponent<LocomotionHandler>();
    }

    private void Update() 
    {
        target = DetectNearbyHumans();
        
        if(target != null && !isInMeleeRange)
        {
            locomotionHandler.MoveToTarget(target.transform.position);
        }

    }


    private GameObject DetectNearbyHumans()
    {
        Collider2D[] nearbyHumans = Physics2D.OverlapCircleAll(transform.position, detectionRadius,humanLayerMask);
        if (nearbyHumans.Length == 0)
            return null;
        
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
