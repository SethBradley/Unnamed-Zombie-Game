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
    public bool isOnCooldown;
    public float onHitSlowdown;
    public bool isAgainstWall;
    public AudioSource audioSource;
    public EffectsHandler effectsHandler;
    public Animator anim;
    public DetectionHandler detectionHandler;
    public LocomotionHandler locomotionHandler;
    public EmoticonHandler emoticonHandler;
    public DebugHandler debugHandler;
    public virtual void Awake() 
    {
        effectsHandler = GetComponent<EffectsHandler>();
        locomotionHandler = GetComponent<LocomotionHandler>();
        detectionHandler = GetComponent<DetectionHandler>();
        emoticonHandler = GetComponent<EmoticonHandler>();
        stateMachine = new StateMachine(this);
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        debugHandler = GetComponent<DebugHandler>(); 
    }
    public IEnumerator StartCooldown()
    {
        //If need performance move WaitForSeconds to start
        isOnCooldown = true;
        //Debug.Log("Is on Cooldown");
        yield return new WaitForSeconds(cooldown);
        //Debug.Log("Is OFFF Cooldown");
        isOnCooldown = false;
    }
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if(this is Human)
            DamagePopup.Create(transform.position, damageAmount, effectsHandler.pfDamagePopup.transform);
            
        //if(!effectsHandler.onHitEffectRunning)
        StartCoroutine(effectsHandler.TakeDamageEffect());
        Debug.Log(health);
        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if(this is Human)
        {
            Debug.Log("Transition to Human Death state"); 
            HumanDeath humanDeath = new HumanDeath(this);
            anim.SetTrigger("Death");
            stateMachine.ChangeState(humanDeath);
        }

        else if (this is Zombie)
        {
            Debug.Log("Transition to Zombie Death state"); 
            anim.SetTrigger("Death");
            ZombieDeath zombieDeath = new ZombieDeath(this);
            stateMachine.ChangeState(zombieDeath);
           
        }
    }

    public void TakeDamage(float damageAmount, Unit attacker)
    {
        TakeDamage(damageAmount);
        Unit expOwner = null;
        
        if(attacker is NormalZombie)
            expOwner = attacker.GetComponent<NormalZombie>().leader;
        else if(attacker is LeaderZombie)
            expOwner = attacker.GetComponent<LeaderZombie>();
        
        if(health <= 0)
            effectsHandler.StartCoroutine(effectsHandler.DropExpFor(expOwner));
    }



}
