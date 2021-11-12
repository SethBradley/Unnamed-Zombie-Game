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
    WaitForSeconds buffer = new WaitForSeconds(0.5f);
    public override IEnumerator Enter()
    {
        Debug.Log("Entering civilian attack state");
        unit.isInCombat = true;
        unit.target = unit.detectionHandler.nearbyZombies[0];
        yield return Execute();
    }

    public override IEnumerator Execute()
    {
        while(unit.isInCombat)
        {
            if(unit.detectionHandler.nearbyZombies.Count <= 0)
            {
                unit.isInCombat = false;
                Debug.Log("All zombies gone, returning to previous task");
                GoToSanctuary();

                yield return null;
                break;
            }

            UpdateThresholds();
            if(unitThreshold >= zombieThreshold)
                {
                    unit.locomotionHandler.MoveToTarget(unit.target.transform.position);
                    
                    while(Vector3.Distance(unit.transform.position, unit.target.transform.position) > unit.weaponHandler.heldWeapon.attackRange)
                    {
                        if(unit.target.isDead)
                        {
                            unit.target = unit.detectionHandler.nearbyZombies[0];
                        }
                        else if(!unit.detectionHandler.nearbyZombies.Contains((Zombie)unit.target))
                        {
                            GoToSanctuary();
                        }

                    
                            
                        
                        unit.locomotionHandler.UpdatePathToTarget(unit.target.transform);
                        yield return buffer;
                    }
                
                    Debug.Log("SWING");
                    unit.StartCoroutine(unit.weaponHandler.UseMeleeWeapon(unit.target));
                }
            else
            {
                GoToSanctuary();
            }


            
            yield return buffer;
        }
        
        yield return null;
    }

    private void GoToSanctuary()
    {
        GoToSanctuary goToSanctuary = new GoToSanctuary(unit);
        unit.stateMachine.ChangeState(goToSanctuary);
    }
    private void UpdateThresholds()
    {
        unitThreshold = 0;
        zombieThreshold = 0;
        unitThreshold += unit.threshold;
        foreach (var unit in unit.detectionHandler.nearbyUnits)
        {
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

    public override IEnumerator Exit()
    {
        unit.isInCombat = false;
        yield return null;
    }   
}
