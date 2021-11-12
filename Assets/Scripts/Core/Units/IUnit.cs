using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    //IAction queuedAction {get;set;}

    public float health;
    public float attackDamage;
    public int xpYield; 
    public int threshold; 
    public Unit target;
    public StateMachine stateMachine;
    public bool isDead;
    public float cooldown;
    public float onHitSlowdown;
    public bool isOnCooldown;
    public EffectsHandler effectsHandler;
    public Animator anim;
    public DetectionHandler detectionHandler;
    public LocomotionHandler locomotionHandler;
    private void Awake() 
    {
        effectsHandler = GetComponent<EffectsHandler>();
        locomotionHandler = GetComponent<LocomotionHandler>();
        detectionHandler = GetComponent<DetectionHandler>();

        anim = GetComponent<Animator>();
    }
    public IEnumerator StartCooldown()
    {
        //If need performance move WaitForSeconds to start
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        Debug.Log(health);
        //Apply slowdown on attack
        if(health <= 0)
        {
            isDead = true;
            StopAllCoroutines();
            //anim playdeath?
        }
        StartCoroutine(effectsHandler.TakeDamageEffect());
    }

    



}
