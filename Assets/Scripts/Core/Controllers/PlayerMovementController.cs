using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SethB.Utilities;
public class PlayerMovementController : MonoBehaviour
{
    public KingpinZombie playerZombie; 

    private void Awake() 
    {
        playerZombie = FindObjectOfType<KingpinZombie>(); 
    }

    private void Update() 
    {
       if(Input.GetMouseButtonDown(1) && !AbilityCastController.instance.isCastingAbility)
       {
           var newWalkPosition = Utilities.GetMouseWorldPosition();
           if(Utilities.CheckIfPositionIsWalkable(newWalkPosition))
           {
               playerZombie.locomotionHandler.MoveToTarget(newWalkPosition);
           }
       } 
    }
}
