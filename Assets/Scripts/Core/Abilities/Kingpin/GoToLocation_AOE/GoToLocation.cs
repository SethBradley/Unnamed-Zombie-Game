using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToLocation : Ability
{
    public int abilityLevel;
    public float abilityRadius;
    bool isUpgraded;
    GameObject indicator;

#region ModifyStats
    public void IncreaseRadius(float increaseAmount)
    {
        abilityRadius += increaseAmount;
        abilityIndicator.transform.localScale = new Vector3(abilityRadius, abilityRadius, 0f);
    }
    public void Upgrade()
    {
        isUpgraded = true;
        //Play some noises
        //Whatever
    }
#endregion
    public override IEnumerator Enter()
    {
        Debug.Log("Beginning to cast GoToLocation");
        isAimingAbility = true;
        indicator = Instantiate(abilityIndicator, Input.mousePosition, transform.rotation);
        Vector3 abilityRadiusVector = new Vector3(abilityRadius, abilityRadius, 0f);
        abilityIndicator.transform.localScale = abilityRadiusVector;

        while(isAimingAbility)
        {
            
            mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            indicator.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f);
            if(Input.GetMouseButtonDown(0))
            {
                yield return Execute();
                break;
            }
            yield return null;
        }

        

    }
    public override IEnumerator Execute()
    {
        foreach (NormalZombie zombie in leaderZombie.zombiesInCommandList)
        {
            WalkToAbilityLocation(zombie);
        }
        

        yield return Exit();
    }

    public override IEnumerator Exit()
    {
        Debug.Log("Exiting GotoLocation");
        AbilityCastController.instance.isCastingAbility = false;
        Object.Destroy(indicator);
        yield return null;
    }

    void WalkToAbilityLocation(NormalZombie zombie)
    {
        zombie.locomotionHandler.isMoving = true;
        float xCoord = UnityEngine.Random.Range((indicator.transform.position.x - indicator.transform.localScale.x/2), (indicator.transform.position.x + indicator.transform.localScale.x/2));
        float yCoord = UnityEngine.Random.Range((indicator.transform.position.y - indicator.transform.localScale.y/2), (indicator.transform.position.y + indicator.transform.localScale.y/2));
        Vector3 randomPosInAbilityRadius = new Vector3(xCoord,yCoord, 0f);


        Collider2D[] collisions = Physics2D.OverlapCircleAll(randomPosInAbilityRadius, 1f, zombie.unwalkableMask);
        //Debug.Log(collisions.Length);
        try
        {
            if(collisions.Length != 0)
            {
                //Debug.Log("UH OH I hit a wall OPSIE! sending again");
                WalkToAbilityLocation(zombie);
                return;
            }
        }
        catch
            {
                //Display red indicator
                return;
            }
            
//        Debug.Log("Should be starting to walk " + randomPosInAbilityRadius);
        zombie.locomotionHandler.MoveToTarget(randomPosInAbilityRadius);

    }

}
