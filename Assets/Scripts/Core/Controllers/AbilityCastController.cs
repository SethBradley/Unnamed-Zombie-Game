using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCastController : MonoBehaviour
{
    public static AbilityCastController instance;
    public bool isCastingAbility;
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
                isCastingAbility = true;
            }
        }
    }

    private void CastAbilityOne()
    {
        GameObject selectedLeaderZombie = UnitSelectController.instance.currentSelected;
        StartCoroutine(selectedLeaderZombie.transform.Find("Abilities").GetChild(0).GetComponent<Ability>().Enter());
    }
}
