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
        Debug.Log("Entering new Wander");
        
        
        yield return Execute();
    }

    public override IEnumerator Execute()
    {


//        Debug.Log("start of wander execute ");
         while(true)
         {
             if(!zombie.locomotionHandler.isMoving)
                WanderAroundLeader();

            Debug.Log("Is Wandering");
            yield return buffer;
            if(zombie.detectionHandler.GetClosestUnitInRange() != null)
            {
                Attack attack = new Attack(zombie);
                zombie.stateMachine.ChangeState(attack);
                yield break;
            }
            
            yield return null;
         }
    }

    public override IEnumerator Exit()
    {

        //Debug.Log("Done with wander");

        yield return null;
    }


    public void WanderAroundLeader()
    {
        
        float xCoord = UnityEngine.Random.Range((zombie.leader.transform.position.x - zombie.leader.circleRadius.bounds.extents.x), (zombie.leader.transform.position.x + zombie.leader.circleRadius.bounds.extents.x));
        float yCoord = UnityEngine.Random.Range((zombie.leader.transform.position.y - zombie.leader.circleRadius.bounds.extents.y), (zombie.leader.transform.position.y + zombie.leader.circleRadius.bounds.extents.y));
        Vector2 randomPosInLeaderRadius = new Vector2(xCoord,yCoord);


        Collider2D[] collisions = Physics2D.OverlapCircleAll(randomPosInLeaderRadius, 2f, zombie.unwalkableMask);
        //Debug.Log(collisions.Length);
        if(collisions.Length != 0)
        {
            //Debug.Log("UH OH I hit a wall OPSIE! sending again");
            WanderAroundLeader();
            return;
        }
            
        //Debug.Log("Should be starting to walk " + randomPosInLeaderRadius);
        zombie.locomotionHandler.MoveToTarget(randomPosInLeaderRadius);
    }
}
