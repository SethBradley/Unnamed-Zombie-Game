using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panic : State
{
    protected Human unit;
    protected Unit nearestZombie;
    protected Transform farthestExit;
    public Panic(Unit _unit)
    {
        unit = _unit as Human;        
    }
    public override IEnumerator Enter()
    {
        Debug.Log("Entering Panic state");
        farthestExit = WaypointManager.instance.GetFarthestExit(unit);
        nearestZombie = unit.detectionHandler.GetClosestUnitInRange();
        PanicAwayFromNearestZombie();
        yield return Execute();
    }

    public override IEnumerator Execute()
    {
        //If (equippedWeapon == null)
         /*   //Seek
        
         
         */
        while(nearestZombie != null && !unit.isArmed)
        {
            //run for the exit
            yield return null;
        }        
        yield return null;
    }


    public override IEnumerator Exit()
    {
        yield return null;
    }
    private void PanicAwayFromNearestZombie()
    {
        
        
        GetDirectionAwayFromZombie();
        
    }

    private void GetDirectionAwayFromZombie()
    {
    }
}
