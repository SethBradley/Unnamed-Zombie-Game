//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Sirenix.OdinInspector;
public class NormalZombie : Unit, ISelectable
{
    [Header("Stats")]
    public float attackRange;
    public float cooldown;
    public MainZombie leader;
    [Header("Locomotion")]
    public LocomotionHandler locomotionHandler;
    [Header("Targeting/Detection")]
    public LayerMask unwalkableMask;
    public DetectionHandler detectionHandler;
    public LayerMask humanLayerMask;
    [Header("Debugging")]
    public bool showDetectionRadius;

    

    public bool isInsideLeaderRadius {get {return leader.circleRadius.bounds.Contains(this.transform.position);}}
    
  
    public bool moveToTarget;

    private void Start() 
    {
        Wander wander = new Wander(this);
        locomotionHandler = GetComponent<LocomotionHandler>();
        detectionHandler = GetComponent<DetectionHandler>();
        stateMachine = new StateMachine(this);

        stateMachine.ChangeState(wander);
    }
}
