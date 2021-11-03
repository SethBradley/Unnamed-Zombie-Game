using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Unit
{
    public void MeleeAttack()
    {
        if(target == null || target.isDead)
            return;

        Debug.Log("CHOMP");
        
        float damageAmount = attackDamage; //+ some other modifiers?
        target.TakeDamage(damageAmount);
        StartCoroutine(StartCooldown());
    }
}
