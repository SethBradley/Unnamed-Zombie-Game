using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingpinZombie : LeaderZombie, ISelectable, IMovable
{
    [Header("Radius")]
    public float minionWanderRadius;
    public CircleCollider2D circleRadius;
    public List<NormalZombie> zombiesInCommandList = new List<NormalZombie>();


    private void Awake() 
    {
        base.Awake();
        circleRadius = GetComponent<CircleCollider2D>();
        circleRadius.radius = minionWanderRadius;

        //stateMachine = new StateMachine(this);
        
    }    
    private void Start() 
    {
        
    }
}
