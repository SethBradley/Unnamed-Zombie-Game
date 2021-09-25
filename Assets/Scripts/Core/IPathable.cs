using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathable
{
    IAction queuedAction {get;set;}
    void PerformQueuedAction();

}
