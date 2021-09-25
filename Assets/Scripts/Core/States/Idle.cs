using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : IState
{
    public void Enter()
    {
        Debug.Log("entering Idle State");
    }
    public void Update()
    {
        Debug.Log("Updating Idle State");
    }

    public void Exit()
    {
        Debug.Log("exiting Idle State");
    }


}
