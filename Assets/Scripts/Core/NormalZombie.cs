//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Sirenix.OdinInspector;
public class NormalZombie : Unit, ISelectable
{
    [Header("Stats")]
    public float attackRange;

    public MainZombie leader;
    [Header("Locomotion")]
    public LocomotionHandler locomotionHandler;
    [Header("Targeting/Detection")]
    public LayerMask unwalkableMask;
    public DetectionHandler detectionHandler;
    public LayerMask humanLayerMask;

    private void Start() 
    {
        Wander wander = new Wander(this);
        locomotionHandler = GetComponent<LocomotionHandler>();
        detectionHandler = GetComponent<DetectionHandler>();
        stateMachine = new StateMachine(this);


        stateMachine.ChangeState(wander);
    }
    //TakeDamage();
    //StartCooldown();


}
