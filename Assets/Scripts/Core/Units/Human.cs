using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Unit, IMovable, ISelectable
{
    //Public Item equippedItem;
    public bool isArmed;
    public bool isDefendingSanctuary;
    public WeaponHandler weaponHandler;

    private void Start() 
    {
        //If heldWeapon != null
        HumanWander humanWander = new HumanWander(this);
        stateMachine = new StateMachine(this);
        stateMachine.ChangeState(humanWander);
        weaponHandler = GetComponent<WeaponHandler>();
    }

    

}
