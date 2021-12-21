using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    public NormalZombie zombie;
    Wander wander;
    public Attack(Unit _zombie)
    {
        zombie = _zombie as NormalZombie;
        wander = new Wander(zombie);
        
    }
    WaitForSeconds buffer = new WaitForSeconds(0.25f);
    

    public override IEnumerator Enter()
    {
        Debug.Log("Entering new attack");
        
        zombie.target = zombie.detectionHandler.GetClosestUnitInRange();
        zombie.locomotionHandler.MoveToTarget(zombie.target.transform.position);        
        yield return Execute();
    }

    public override IEnumerator Execute()
    {
//        Debug.Log("executing new attack");
        
        while(zombie.target != null && zombie.detectionHandler.nearbyHumans.Count > 0)
        {
    
           
            Unit closestUnit = zombie.detectionHandler.GetClosestUnitInRange();
            
            if(closestUnit != zombie.target && closestUnit != null)
            {
                yield return Enter();
                break;
            }
                

            if( !zombie.isOnCooldown && Vector2.Distance(zombie.transform.position, zombie.target.transform.position) <= zombie.attackRange)
            {
                zombie.anim.SetTrigger("MeleeAttack");
                zombie.StartCoroutine(zombie.StartCooldown());
                zombie.target.TakeDamage(zombie.attackDamage, zombie); 
            }

            
            if (closestUnit == null)
            {
                zombie.target = null;
                zombie.stateMachine.ChangeState(wander);
                yield break;
            }
            if(zombie.target.isDead)
                break;
            
                
             zombie.locomotionHandler.MoveToTarget(closestUnit.transform.position);
             yield return buffer;
            
        }
        zombie.target = null;
        
        zombie.stateMachine.ChangeState(wander);
        yield break;
    }


    public override IEnumerator Exit()
    {
        yield return null;
    }
}
