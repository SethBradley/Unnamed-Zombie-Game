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
        GameObject newTarget = zombie.detectionHandler.GetClosestUnitInRange();
        zombie.target = newTarget;
        zombie.locomotionHandler.MoveToTarget(zombie.target.transform.position);        
        yield return Execute();
    }

    public override IEnumerator Execute()
    {
        Debug.Log("executing new attack");
        
        while(zombie.target != null)
        {
            
            GameObject closestUnit = zombie.detectionHandler.GetClosestUnitInRange();
            if (closestUnit == null)
            {
                zombie.target = null;
                zombie.stateMachine.ChangeState(wander);
                yield break;
                
            }
            if(closestUnit != zombie.target && closestUnit != null)
                yield return Enter();
            
            zombie.locomotionHandler.UpdatePathToTarget(zombie.target.transform);
            
            yield return buffer;
            yield return null;
            
        }
        //if zombie is alive maybe
            if(zombie.target == null && !zombie.isDead)
            {
                Debug.Log("Target is now null and zombie is still alive");
                zombie.stateMachine.ChangeState(wander);
                yield break;
            }
        yield return null;
    }

    public override IEnumerator Exit()
    {
        yield return null;
    }
}
