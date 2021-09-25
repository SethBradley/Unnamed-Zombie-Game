using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MainZombie : MonoBehaviour, IPathable, ISelectable, IMovable
{
    public IAction queuedAction{get;set;}

    public virtual void PerformQueuedAction()
    {

        Debug.Log(this.gameObject.name + " is performing action");
    }
}
