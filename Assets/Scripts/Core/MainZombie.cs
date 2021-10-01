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

    public float cooldown { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public GameObject target { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Start() 
    {
        circleRadius = GetComponent<CircleCollider2D>();
        circleRadius.radius = minionWanderRadius;
    }



    public virtual void PerformQueuedAction()
    {

        Debug.Log(this.gameObject.name + " is performing action");
    }

}
