using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    public NormalZombie zombie;
    public Unit target;
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
        target = newTarget;
        zombie.locomotionHandler.MoveToTarget(target.transform.position);        
        yield return Execute();
    }

    public override IEnumerator Execute()
    {
        Debug.Log("executing new attack");
        
        while(target != null)
        {
            yield return buffer;
            Unit closestUnit = zombie.detectionHandler.GetClosestUnitInRange();

            if(closestUnit != target && closestUnit != null)
                yield return Enter();
            
            if( !zombie.isOnCooldown && Vector3.Distance(zombie.transform.position, target.transform.position) <= zombie.attackRange)
            {
                MeleeAttackHuman();
            }

            zombie.locomotionHandler.UpdatePathToTarget(target.transform);
            if (closestUnit == null || target.isDead)
            {
                target = null;
                zombie.stateMachine.ChangeState(wander);
                yield break;
            }

            yield return null;
            
        }

        yield return null;
    }

    private void MeleeAttackHuman()
    {
        Debug.Log("CHOMP");
        //play attack anim
        float damageAmount = zombie.attackDamage; //+ some other modifiers?
        target.TakeDamage(damageAmount);
        zombie.StartCoroutine(zombie.StartCooldown());
    }

    public override IEnumerator Exit()
    {
        yield return null;
    }
}
