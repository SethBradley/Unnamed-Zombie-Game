using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour, IPathable, IMovable, ISelectable
{
    public IAction queuedAction {get; set;}

    public void PerformQueuedAction()
    {
        Debug.Log(" Human perofrming action");
    }
}
