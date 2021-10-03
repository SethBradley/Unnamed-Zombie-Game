using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MainZombie : Unit, ISelectable, IMovable
{
    //public IAction queuedAction{get;set;}

    [Header("Radius")]
    public float minionWanderRadius;
    public CircleCollider2D circleRadius;
    public List<NormalZombie> zombiesInRadiusList = new List<NormalZombie>();


    private void Awake() 
    {
        circleRadius = GetComponent<CircleCollider2D>();
        circleRadius.radius = minionWanderRadius;
    }



    public virtual void PerformQueuedAction()
    {

        Debug.Log(this.gameObject.name + " is performing action");
    }

}
