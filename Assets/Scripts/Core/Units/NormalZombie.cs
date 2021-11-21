//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Sirenix.OdinInspector;
public class NormalZombie : Zombie, ISelectable
{
    [Header("Stats")]
    public float attackRange;

    public KingpinZombie leader;
    [Header("Targeting/Detection")]
    public LayerMask unwalkableMask;
    public LayerMask humanLayerMask;

    private void Start() 
    {
        threshold = 1;
        Wander wander = new Wander(this);
        stateMachine = new StateMachine(this);


        stateMachine.ChangeState(wander);
    }
    //TakeDamage();
    //StartCooldown();


}
