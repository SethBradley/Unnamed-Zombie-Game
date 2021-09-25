using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursuit : IState 
{
    protected IPathable unit;
    protected LocomotionHandler locomotionHandler;
    public Pursuit(IPathable _unit, LocomotionHandler _locomotionHandler)
    {
        unit = _unit;
        locomotionHandler = _locomotionHandler;
    }
    
    public void Enter()
    {
        Debug.Log("entering Pursuit State");
    }
    public void Update()
    {
        Debug.Log("Updating Pursuit State");
    }

    public void Exit()
    {
        Debug.Log("exiting Pursuit State");
    }

}
