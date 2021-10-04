using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianWander : State
{
    protected Unit unit;
    protected WaitForSeconds buffer = new WaitForSeconds(0.5f);
    public CivilianWander(Unit _unit)
    {
        unit = _unit;

    }
    public override IEnumerator Enter()
    {
        Transform waypoint = WaypointManager.instance.GetRandomWaypoint();
        unit.locomotionHandler.MoveToTarget(waypoint.position);
        yield return Execute();
    }

    public override IEnumerator Execute()
    {
        while(unit.locomotionHandler.isMoving)
        {
            Unit zombie = unit.detectionHandler.GetClosestUnitInRange();
            if (zombie != null)
            {
                Debug.Log("Zombie found, switching to panic mode");
                Panic panic = new Panic(unit);
                unit.stateMachine.ChangeState(panic);
            }
            Debug.Log("Human is wandering");
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
