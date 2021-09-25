using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingpinZombie : MainZombie
{
    public float radius;

    private void Start() 
    {

    }
    public override void PerformQueuedAction()
    {
        Debug.Log("Bitch zombie");
    }
}
