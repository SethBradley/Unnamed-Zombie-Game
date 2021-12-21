using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanDeath : State
{

    protected Unit unit;

    public HumanDeath(Unit _unit)
    {
        unit = _unit; 
    }
    public override IEnumerator Enter()
    {
        Debug.Log("Entering Human Death State");
        
        unit.isDead = true;
    
        unit.stateMachine.StopStateMachine();
        unit.StopAllCoroutines();
        unit.effectsHandler.ResetEffects();
        unit.locomotionHandler.isMoving = false;
        unit.locomotionHandler.StopCoroutine(unit.locomotionHandler.move);
        unit.locomotionHandler.aiPath.isStopped = true;
        
        yield return null;
    }

    public override IEnumerator Execute()
    {
        //Determine turning 
        yield return null;
    }

    public override IEnumerator Exit()
    {
        yield return null;
    }
}
