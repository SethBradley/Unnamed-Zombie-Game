using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingpinZombie : MonoBehaviour, IPathable, ISelectable
{
    public void PerformQueuedAction()
    {
        Debug.Log(this.gameObject.name + " is performing action");
    }
}
