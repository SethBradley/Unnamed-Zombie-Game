using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State currentState;
    public Unit unit;
    public StateMachine(Unit _unit)
    {
        unit = _unit;
    }
    
    public void ChangeState(State newState)
    {
        if(currentState != null)
            {
                
                //Debug.Log("Stopping all coroutines on " + unit.name);
                unit.StopAllCoroutines();
                unit.effectsHandler.ResetEffects();
                unit.StartCoroutine(currentState.Exit());
                currentState = null;
            }
        
        
        currentState = newState;
        unit.StartCoroutine(currentState.Enter());
    }

    public void StopStateMachine()
    {
        //unit.StartCoroutine(currentState.Exit());
        if(currentState != null)
        {
            unit.StartCoroutine(currentState.Exit());
            
        }
            
        currentState = null;
    }
    
}
