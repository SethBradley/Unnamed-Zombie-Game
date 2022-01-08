using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAttack : State
{
    protected Human unit;
    private int zombieThreshold;
    private int unitThreshold;
    public bool isShooting; 
    public HumanAttack(Human _unit)
    {
        unit = _unit;
    }
    WaitForSeconds buffer = new WaitForSeconds(0.5f);
    public override IEnumerator Enter()
    {
        Debug.Log("Entering civilian attack state");
        unit.target = unit.detectionHandler.nearbyZombies[0];
        yield return Execute();
    }

    public override IEnumerator Execute()
    {
        while(unit.target != null)
        {
            if(unit.detectionHandler.nearbyZombies.Count <= 0)
            {
                unit.target = null;
                Debug.Log("All zombies gone, returning to previous task");
                GoToSanctuary();

                yield return null;
            }
            UpdateThresholds();
            if(unitThreshold >= zombieThreshold)
                {
#region Ranged
                   if(unit.weaponHandler.heldWeapon.isRanged)
                   {
                       Debug.Log("BIG IRON");

                       while(!unit.isOnCooldown)
                       {
                           unit.locomotionHandler.DisableMovement(); 
                           unit.weaponHandler.Shoot();
                           yield return new WaitForSeconds(0.5f);
                           float shootAgainChance = UnityEngine.Random.Range(0f,1f);
                           if(shootAgainChance > 0.5f)
                           {
                               Debug.Log("Shoot again " + shootAgainChance);
                               yield return null;
                           }
                           else
                           {
                               unit.locomotionHandler.EnableMovement();
                               unit.StartCoroutine(unit.StartCooldown());   
                           }

                           yield return null;
                       }
                       
                       unit.effectsHandler.spriteRenderer.flipX = false;
                       
                       //first - shoot and go on cooldown
                       //check distance of all nearby zombies
                        //if zombies are closer than say 2? 
                            //(while)then pace away from opp direction of zombie til human threshold is large AND no zombies are close
                                //If pacing for longer than 3 seconds head for exit 
                   }
#endregion
                   else
#region Melee
                   {
                        while(Vector3.Distance(unit.transform.position, unit.target.transform.position) > unit.weaponHandler.heldWeapon.attackRange + 0.25f)
                        {
                            unit.locomotionHandler.MoveToTarget(unit.target.transform.position);
                            if(unit.target.isDead)
                            {
                                try
                                {
                                    unit.target = unit.detectionHandler.nearbyZombies[0];
                                }
                                catch
                                {
                                    Debug.Log("Enemy died while walking, continuing");
                                }
                            }
                            else if(!unit.detectionHandler.nearbyZombies.Contains((Zombie)unit.target))
                            {
                                GoToSanctuary();
                            }

                    
                            Debug.Log("Current distance is " + Vector3.Distance(unit.transform.position, unit.target.transform.position) + " Must be under " + unit.weaponHandler.heldWeapon.attackRange + 0.25f ); 
                        
                            
                            yield return buffer;
                        }
                
                        Debug.Log("SWING");
                        unit.StartCoroutine(unit.locomotionHandler.FreezeMovementForSeconds(unit.weaponHandler.heldWeapon.cooldown/2));
                        unit.weaponHandler.UseMeleeWeapon();
                    }
#endregion
                        yield return buffer;
                }
                        else
                        {
                        GoToSanctuary();
                        }   

            if(unit.detectionHandler.nearbyZombies.Count > 0)
            {
                unit.target = null;
                yield return Enter();
            }   
            else if(unit.detectionHandler.nearbyZombies.Count <= 0)
            {
                unit.target = null;
                Debug.Log("All zombies gone, returning to previous task");
                GoToSanctuary();
            }
        yield return Exit();
        }
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
    
        yield return null;
    }   
}
