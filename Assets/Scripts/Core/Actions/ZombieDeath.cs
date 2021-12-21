using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDeath : State
{
    protected Unit unit; 
    public ZombieDeath(Unit _unit)
    {
        unit = _unit; 
    }
    public override IEnumerator Enter()
    {
        Debug.Log("Entering Zombie Death State");
        
        unit.isDead = true;
        
        unit.stateMachine.StopStateMachine();
        unit.StopAllCoroutines();
        unit.effectsHandler.ResetEffects();
        unit.locomotionHandler.isMoving = false;
        unit.locomotionHandler.StopCoroutine(unit.locomotionHandler.move);
        unit.locomotionHandler.aiPath.isStopped = true;
        

        yield return Execute();
    }

    public override IEnumerator Execute()
    {
        yield return null;
    }

    public override IEnumerator Exit()
    {
        yield return null;
    }

}
