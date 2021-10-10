using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LeaderZombie : Unit, ISelectable, IMovable
{
    //public IAction queuedAction{get;set;}

    [Header("Radius")]
    public float minionWanderRadius;
    public CircleCollider2D circleRadius;
    public List<NormalZombie> zombiesInCommandList = new List<NormalZombie>();


    private void Awake() 
    {
        circleRadius = GetComponent<CircleCollider2D>();
        circleRadius.radius = minionWanderRadius;

        
    }

}
