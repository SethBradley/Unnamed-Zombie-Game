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
        Unit newTarget = zombie.detectionHandler.GetClosestUnitInRange();
        zombie.target = newTarget;
        zombie.locomotionHandler.MoveToTarget(zombie.target.transform.position);        
        yield return Execute();
    }

    public override IEnumerator Execute()
    {
//        Debug.Log("executing new attack");
        
        while(zombie.target != null)
        {
            yield return buffer;
            Unit closestUnit = zombie.detectionHandler.GetClosestUnitInRange();

            if(closestUnit != zombie.target && closestUnit != null)
                yield return Enter();

            if( !zombie.isOnCooldown && Vector3.Distance(zombie.transform.position, zombie.target.transform.position) <= zombie.attackRange)
            {
                zombie.anim.SetTrigger("MeleeAttack");
            }

            zombie.locomotionHandler.UpdatePathToTarget(zombie.target.transform);
            if (closestUnit == null || zombie.target.isDead)
            {
                zombie.target = null;
                zombie.stateMachine.ChangeState(wander);
                yield break;
            }

            yield return null;
            
        }

        yield return null;
    }

    public override IEnumerator Exit()
    {
        yield return null;
    }
}
