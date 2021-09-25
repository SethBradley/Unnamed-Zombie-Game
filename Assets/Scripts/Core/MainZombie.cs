using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MainZombie : MonoBehaviour, IPathable, ISelectable, IMovable
{
    public IAction queuedAction{get;set;}

    [Header("Radius")]
    public float minionWanderRadius;
    public CircleCollider2D circleRadius;
    public List<NormalZombie> zombieList;

    private void Start() 
    {
        circleRadius = GetComponent<CircleCollider2D>();
        circleRadius.radius = minionWanderRadius;
    }
    private void Update() 
    {
    }

    public virtual void PerformQueuedAction()
    {

        Debug.Log(this.gameObject.name + " is performing action");
    }

}
