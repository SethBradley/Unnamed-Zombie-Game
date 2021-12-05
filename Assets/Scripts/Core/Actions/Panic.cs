using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panic : State
{
    protected Human unit;
    protected Unit nearestZombie;
    protected Transform nearestExit;
    private bool weaponIsNearby;
    public Panic(Unit _unit)
    {
        unit = _unit as Human;
                
    }

    public WaitForSeconds buffer = new WaitForSeconds(1.5f);
    public WaitForSeconds shortbuffer = new WaitForSeconds(0.25f);
    public override IEnumerator Enter()
    {
        Debug.Log("Entering Panic state");
        
        nearestExit = WaypointManager.instance.GetNearestExit(unit.transform);
        nearestZombie = unit.detectionHandler.GetClosestUnitInRange();
        yield return Execute();
    }

    public override IEnumerator Execute()
    {
        Debug.Log("Executing panic");
        unit.locomotionHandler.StopCoroutine("MoveToLocation");
        Transform nearbyWeapon = unit.detectionHandler.DetectNearestWeapon();
        Debug.Log("Nearest Weapon is " + nearbyWeapon);
        
        PanicAwayFromNearestZombie();
        yield return buffer; 
        
        if (nearbyWeapon != null && !unit.isArmed)
        {
             yield return PickUpWeapon(nearbyWeapon);
        }
            

        unit.StartCoroutine(unit.locomotionHandler.MoveToLocation(nearestExit.position));
        /*while(nearestZombie  && !unit.isArmed)
        {
            //run for the exit
            yield return null;
        }*/

        yield return buffer;
        yield return null;
    }


    public override IEnumerator Exit()
    {
        yield return null;
    }
    private void PanicAwayFromNearestZombie()
    {

        Vector3 humanPos = unit.transform.position;
        Vector3 zombiePos = nearestZombie.transform.position;
        Vector3 directionAwayFromZombie = (humanPos -zombiePos);

        unit.StartCoroutine(unit.locomotionHandler.MoveToLocation(directionAwayFromZombie));
        
    }

    private IEnumerator PickUpWeapon(Transform nearbyWeapon)
    {

        while(nearbyWeapon != null && Vector2.Distance(unit.transform.position, nearbyWeapon.position) > 1f)
        {
            if(nearbyWeapon.gameObject == null)
                yield break;
            Debug.Log("Running to weapon  " + Vector2.Distance(unit.transform.position, nearbyWeapon.position) );
            unit.StartCoroutine(unit.locomotionHandler.MoveToLocation(nearbyWeapon.position));

            if(Vector2.Distance(unit.transform.position, nearbyWeapon.position) < 1f)
            {
                Debug.Log("Close enough; breaking");
                break;
            }
                
                
            yield return shortbuffer;
        }

        if(nearbyWeapon == null)
            yield break;
            
        Debug.Log("Should be picking up weapon now");
        unit.weaponHandler.EquipWeapon(nearbyWeapon);
        GoToSanctuary goToSanctuary = new GoToSanctuary(unit);
        unit.stateMachine.ChangeState(goToSanctuary);
        //enter civilianAttack state
        yield return null;
    }

}
