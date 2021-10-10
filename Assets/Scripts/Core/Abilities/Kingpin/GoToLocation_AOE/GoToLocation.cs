using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToLocation : Ability
{
    public float abilityRadius;


    private void Start() 
    {
        Vector3 abilityRadiusVector = new Vector3(abilityRadius, abilityRadius, 0f);
        AbilityIndicator.transform.localScale = abilityRadiusVector;
    }

    public void IncreaseRadius(float increaseAmount)
    {
        abilityRadius += increaseAmount;
        AbilityIndicator.transform.localScale = new Vector3(abilityRadius, abilityRadius, 0f);
    }

}
