using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAttack : State
{
    protected Human unit;
    private int zombieThreshold;
    private int unitThreshold;
    public HumanAttack(Human _unit)
    {
        unit = _unit;
    }
    public override IEnumerator Enter()
    {
        Debug.Log("Entering civilian attack state");
        yield return Execute();
    }

    public override IEnumerator Execute()
    {
        while(!unit.isDefendingSanctuary)
        {
            //if nearbyUnits is greater than 0 Foreach zombie in nearbyUnits add to threat Threshold
            

            if(unit.detectionHandler.nearbyUnits.Count > 0)
            {
                //there is a zombie detected
                GetZombieThreshold();
                GetHumanThreshold();
                
            }
            // if nearbyUnits is 0 or greater than human attackThreshold, Walk to sanctuary
            
            

            yield return null;
        }
        
        yield return null;
    }

    private void GetHumanThreshold()
    {
        throw new NotImplementedException();
    }

    private void GetZombieThreshold()
    {
        foreach (Unit zombie in unit.detectionHandler.nearbyUnits)
        {
            zombieThreshold += zombie.threshold;
        }
        Debug.Log("Zombie Threshold is " + zombieThreshold);
    }

    public override IEnumerator Exit()
    {
        yield return null;
    }   
}
