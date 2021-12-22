using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCastController : MonoBehaviour
{
    public static AbilityCastController instance;
    GameObject selectedLeaderZombie;
    public bool isCastingAbility;
    public  KingpinZombie kingpinZombie;
    

    private void Start() 
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
         else 
            instance = this;
    }

    private void Update() 
    {
        if (UnitSelectController.instance.currentSelected != null)
        {
            
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                CastAbilityOne();
            }
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                CastAbilityTwo();
            }
        }
    }

    private void CastAbilityOne()
    {
        if(isCastingAbility)
        {
            CancelAllAbilities();
        }
        selectedLeaderZombie = UnitSelectController.instance.currentSelected;
        CancelAllAbilities();
        isCastingAbility = true;
        StartCoroutine(selectedLeaderZombie.transform.Find("Abilities").GetChild(0).GetComponent<Ability>().Enter());
    }

    private void CastAbilityTwo()
    {
    if(kingpinZombie.CanTurnHumans()){
        if(isCastingAbility)
        {
            CancelAllAbilities();
        }
        selectedLeaderZombie = UnitSelectController.instance.currentSelected;
        CancelAllAbilities();
        isCastingAbility = true;
        StartCoroutine(selectedLeaderZombie.transform.Find("Abilities").GetChild(1).GetComponent<Ability>().Enter());
    }
}
    private void CancelAllAbilities()
    {
        Transform abilitiesParent = selectedLeaderZombie.transform.Find("Abilities").transform;

        for (int i = 0; i < abilitiesParent.childCount; i++)
        {
            StartCoroutine(abilitiesParent.GetChild(i).GetComponent<Ability>().Exit());
        }
    }
}
