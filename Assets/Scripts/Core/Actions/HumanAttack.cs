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
            

            if(unit.detectionHandler.nearbyZombies.Count > 0)
            {
                //there is a zombie detected
                UpdateThresholds();
                if(unitThreshold >= zombieThreshold)
                {
                    yield return EngageInCombat();
                    yield break;
                }

                
                //GetHumanThreshold();
                
            }
            // if nearbyUnits is 0 or greater than human attackThreshold, Walk to sanctuary
            
            

            yield return null;
        }
        
        yield return null;
    }
    public IEnumerator EngageInCombat()
    {
        float chanceToFlee;
        Debug.Log("Engaging in combat");
        while(unit.detectionHandler.nearbyZombies.Count > 0)
        {
            UpdateThresholds();
            if(zombieThreshold > unitThreshold)
            {
                chanceToFlee = UnityEngine.Random.Range(0f, 1f);
                if (chanceToFlee >= 0.75f)
                {
                    yield return Execute();
                    yield break;
                }
            }

            else
            {
                //actually fight
            }
        
        }
        yield return null;
    }

    private void UpdateThresholds()
    {
        unitThreshold += unit.threshold;
        foreach (var unit in unit.detectionHandler.nearbyUnits)
        {
            Debug.Log(unit.gameObject.layer);
            if (unit.gameObject.layer == 7)
            {
                unitThreshold += unit.threshold;
            }

            else if(unit.gameObject.layer == 8)
            {
                zombieThreshold += unit.threshold;
            }
        }

        Debug.Log($"Human threshold is {unitThreshold} and Zombie threshold is {zombieThreshold}");
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
