using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanWander : State
{
    protected Unit unit;
    protected WaitForSeconds buffer = new WaitForSeconds(0.5f);
    public HumanWander(Unit _unit)
    {
        unit = _unit;

    }
    public override IEnumerator Enter()
    {
        Transform waypoint = WaypointManager.instance.GetRandomWaypoint();
        unit.locomotionHandler.MoveToTarget(waypoint.position);
        unit.debugHandler.SetDebugText("Wander : Enter");
        yield return Execute();
    }

    public override IEnumerator Execute()
    {
        unit.debugHandler.SetDebugText("Wander : Execute");
        while(!unit.locomotionHandler.aiPath.reachedEndOfPath)
        {
            Unit zombie = unit.detectionHandler.GetClosestUnitInRange();
            if (zombie != null)
            {
                Debug.Log("Zombie found, switching to panic mode");
                Panic panic = new Panic(unit);
                unit.stateMachine.ChangeState(panic);
            }
            //Debug.Log("Human is wandering");
            yield return null;
        }
        yield return buffer;
        yield return Enter();
    }

    public override IEnumerator Exit()
    {
        yield return null;
    }


}
