using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Unit, IMovable, ISelectable
{
    //Public Item equippedItem;
    public bool isArmed;
    private void Start() 
    {
        CivilianWander civilianWander = new CivilianWander(this);
        stateMachine = new StateMachine(this);
        stateMachine.ChangeState(civilianWander);
    }
}
