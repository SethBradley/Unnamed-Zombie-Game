using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    //IAction queuedAction {get;set;}
    public float health;
    public float attackDamage;
    public GameObject target;
    public StateMachine stateMachine;
    public bool isDead;
    public float cooldown;
    public bool isOnCooldown;
    public Animator anim;

    public IEnumerator StartCooldown()
    {
        //If need performance move WaitForSeconds to start
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }
    public void TakeDamage(float damageAmount)
    {
        //anim set takeDamage to true?
        health -= damageAmount;
        Debug.Log(health);
        if(health <= 0)
        {
            isDead = true;
            StopAllCoroutines();
            //anim playdeath?
        }
    }
}
