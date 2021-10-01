using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Unit, IMovable, ISelectable
{
    public float cooldown { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public GameObject target { get;set; }

    //public IAction queuedAction {get; set;}

    public void PerformQueuedAction()
    {
        //Debug.Log(" Human perofrming action");
    }
}
