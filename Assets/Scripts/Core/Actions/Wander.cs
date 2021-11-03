using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : State
{
    NormalZombie zombie;
    public Wander(NormalZombie _zombie)
    {
        zombie = _zombie;
    }
        WaitForSeconds buffer = new WaitForSeconds(1.5f);
    public override IEnumerator Enter()
    {
        //Debug.Log("Entering new Wander");
        WanderAroundLeader();
        
        yield return Execute();
    }

    public override IEnumerator Execute()
    {
//        Debug.Log("start of wander execute ");
        
         while(zombie.locomotionHandler.isMoving)
         {
            //Debug.Log("Is moving");
            yield return buffer;
            if(zombie.detectionHandler.GetClosestUnitInRange() != null)
            {
                Attack attack = new Attack(zombie);
                zombie.stateMachine.ChangeState(attack);
                yield break;
            }
            yield return null;
         }
       
        
        //Debug.Log("end of wander execute ");
        yield return Enter();
    }

    public override IEnumerator Exit()
    {

        //Debug.Log("Done with wander");
        yield return null;
    }


    public void WanderAroundLeader()
    {
        zombie.locomotionHandler.isMoving = true;
        float xCoord = UnityEngine.Random.Range((zombie.leader.transform.position.x - zombie.leader.circleRadius.bounds.extents.x), (zombie.leader.transform.position.x + zombie.leader.circleRadius.bounds.extents.x));
        float yCoord = UnityEngine.Random.Range((zombie.leader.transform.position.y - zombie.leader.circleRadius.bounds.extents.y), (zombie.leader.transform.position.y + zombie.leader.circleRadius.bounds.extents.y));
        Vector3 randomPosInLeaderRadius = new Vector3(xCoord,yCoord, 0f);


        Collider2D[] collisions = Physics2D.OverlapCircleAll(randomPosInLeaderRadius, 1f, zombie.unwalkableMask);
        //Debug.Log(collisions.Length);
        if(collisions.Length != 0)
        {
            //Debug.Log("UH OH I hit a wall OPSIE! sending again");
            WanderAroundLeader();
            return;
        }
            
//        Debug.Log("Should be starting to walk " + randomPosInLeaderRadius);
        zombie.locomotionHandler.MoveToTarget(randomPosInLeaderRadius);
    }
}
