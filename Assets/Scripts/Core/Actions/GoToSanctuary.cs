using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToSanctuary : State
{
    protected Human unit;
     protected Transform nearestExit;
    public GoToSanctuary(Human _unit)
    {
        unit = _unit;
                
    }
    public WaitForSeconds buffer = new WaitForSeconds(1f);
    public override IEnumerator Enter()
    {
        Debug.Log("Entering Go To Santuary State");
        nearestExit = WaypointManager.instance.GetNearestExit(unit.transform);
        yield return Execute();
    }

    public override IEnumerator Execute()
    {
        Debug.Log("Pretend unit is walking to santuary");
        unit.locomotionHandler.StopCoroutine("FollowPath");
        unit.locomotionHandler.MoveToTarget(nearestExit.position);

        while(!unit.isDefendingSanctuary)
        {
            if(unit.detectionHandler.nearbyZombies.Count > 0)
            {
                HumanAttack humanAttack = new HumanAttack(unit as Human);
                unit.stateMachine.ChangeState(humanAttack);
                yield return null;
            }


            yield return buffer;
        }
        
        yield return null;
    }

    public override IEnumerator Exit()
    {
        
        yield return null;
    }

}
