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
        unit.emoticonHandler.InstantiateScreamEmoticon();
        unit.audioSource.PlayOneShot(DatabaseMaster.instance.GetSoundFXByName("PanicEntrance"));
        GameplayHandler.instance.IncreasePanic(12);
        unit.debugHandler.SetDebugText("Panic : Enter");
        yield return Execute();
    }

    public override IEnumerator Execute()
    {
        Debug.Log("Executing panic");
        Transform nearbyWeapon = unit.detectionHandler.DetectNearestWeapon();
        Debug.Log("Nearest Weapon is " + nearbyWeapon);
        unit.debugHandler.SetDebugText("Panic : Execute");
        PanicAwayFromNearestZombie();
        yield return buffer; 
        unit.emoticonHandler.ExitActiveEmoticon();
        if (nearbyWeapon != null && !unit.isArmed)
        {
             yield return PickUpWeapon(nearbyWeapon);
        }
            
        yield return CallPolice(); 
        unit.locomotionHandler.MoveToTarget(nearestExit.position);
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

        unit.locomotionHandler.MoveToTarget(directionAwayFromZombie * 5);
        
    }

    private IEnumerator PickUpWeapon(Transform nearbyWeapon)
    {

        while(nearbyWeapon != null && Vector2.Distance(unit.transform.position, nearbyWeapon.position) > 1f)
        {
            if(nearbyWeapon.gameObject == null)
                yield break;
            Debug.Log("Running to weapon  " + Vector2.Distance(unit.transform.position, nearbyWeapon.position) );
            unit.locomotionHandler.MoveToTarget(nearbyWeapon.position);

            if(Vector2.Distance(unit.transform.position, nearbyWeapon.position) < 0.5f)
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


    private IEnumerator CallPolice()
    {
        
        unit.locomotionHandler.DisableMovement();
        unit.emoticonHandler.InstantiatePhoneCallEmote();
        unit.debugHandler.SetDebugText("Panic : CallPolice");
        while(unit.emoticonHandler.elapsedPhoneCallTime <= 6)
            yield return null;
        
        unit.debugHandler.SetDebugText("Panic : Execute > MoveToExit");
        unit.locomotionHandler.EnableMovement();
        unit.emoticonHandler.ExitActiveEmoticon();
        GameplayHandler.instance.IncreasePanic(60);

    }


}
