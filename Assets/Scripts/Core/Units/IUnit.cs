using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    //IAction queuedAction {get;set;}
    public float health;
    public float attackDamage;
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
    public AnimationCurve animationCurve;
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

    public IEnumerator GetKnockedBack(float knockbackAmount, Vector3 attackOrigin)
    {
        locomotionHandler.StopCoroutine(locomotionHandler.FollowPath());

        Vector3 unitPos = transform.position;
        Vector3 knockbackDirection = (unitPos - attackOrigin ).normalized;
        Vector3 knockbackLocation = transform.position + (knockbackDirection * knockbackAmount); 
        
        float timeElapsed = 0;
        float lerpDuration = 1f;

        while(timeElapsed < lerpDuration)
        {
            Vector3 newUnitpos = Vector3.Lerp(unitPos, knockbackLocation, animationCurve.Evaluate(timeElapsed));
            transform.position = newUnitpos;

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    

}
