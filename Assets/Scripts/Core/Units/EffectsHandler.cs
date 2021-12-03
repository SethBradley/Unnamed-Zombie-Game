using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EffectsHandler : MonoBehaviour
{
    public float waitTime;
    public Vector3 spriteEffectOffset;
    public SpriteRenderer spriteRenderer;
    public ParticleSystem[] particles;
    private Animator anim;
    public Unit unit;
    public GameObject xpGameObj;
    public AnimationCurve xpPositionCurve;
    
    public bool onHitEffectRunning;
    private Vector2 oldPosition;
    private Quaternion flipX = Quaternion.Euler(0f,180f,0f);
    [Header("HealthBar and Combat Text")]
    public Image healthBar;
    private float maxHealth;
    public GameObject pfDamagePopup;
    
    private void Start() 
    {
        oldPosition = transform.position;
        unit = GetComponent<Unit>();
        anim = GetComponent<Animator>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        spriteEffectOffset = transform.Find("Sprite").transform.localPosition;
        //StartCoroutine(DropExp());
        

        healthBar = unit.transform.Find("Healthbar").GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        healthBar.transform.parent.gameObject.SetActive(false);
        maxHealth = unit.health;
    }
    private void Update() 
    {
        if(!unit.locomotionHandler.isGettingKnockedBack)
        {
            if(transform.position.x < oldPosition.x)
            {
                transform.rotation = flipX;
            }
            else if (transform.position.x > oldPosition.x)
                transform.rotation = Quaternion.Euler(0,0,0);
    
            oldPosition = transform.position;
        }

    }


    public IEnumerator TakeDamageEffect()
    {
        onHitEffectRunning = true;
        spriteRenderer.color = Color.white;
        
        //anim.SetTrigger("Hit");
        DecreaseHealthUI();
        Instantiate(particles[0], this.transform.position + spriteEffectOffset, this.transform.rotation, this.transform);
        float elapsedTime = 0f;
        while(elapsedTime <= waitTime)
        {
            //Debug.Log("still going red");
            elapsedTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(Color.white, Color.red,elapsedTime/waitTime);
            yield return null;
        }
        elapsedTime = 0f;
        while(elapsedTime <= waitTime)
        { 
            //Debug.Log("still going to normal");
            elapsedTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(Color.red,Color.white,elapsedTime/waitTime);
            yield return null;
        }
        onHitEffectRunning = false;
        
    }
 public void DecreaseHealthUI()
    { 
        if (!healthBar.transform.parent.gameObject.activeSelf)
        {
            healthBar.transform.parent.gameObject.SetActive(true);
        }

        Debug.Log($"{unit.health} / {maxHealth}");
        healthBar.fillAmount = (unit.health / maxHealth);

        if (unit.isDead)
        {
            healthBar.transform.parent.gameObject.SetActive(false);
        }

    }
    public void ResetEffects()
    {
        spriteRenderer.color = Color.white;
    }

    public IEnumerator DropExpFor(Unit leader)
    {
        for (int i = 0; i < unit.xpYield; i++)
        {
            GameObject expOrb = Instantiate(xpGameObj,transform.position,this.transform.rotation);
            StartCoroutine(SendExpInRandomDirection(expOrb));
            yield return new WaitForSeconds(0.15f);
        }
        
        
    }

    private IEnumerator SendExpInRandomDirection(GameObject expOrb)
    {

        Vector3 unitPos = unit.transform.position;
        Vector3 peakOfHeight = new Vector3(unit.transform.position.x, unit.transform.position.y + 10, 0f);
        Vector3 endDestination = new Vector3(UnityEngine.Random.Range(unit.transform.position.x - 2, unit.transform.position.x + 2 ), UnityEngine.Random.Range(unit.transform.position.y - 2, unit.transform.position.y + 2 ), 0f);
        // peakOfHeight = B \ endDestination = C

        float elapsedTime = 0f;
        float timeToWait = 1f;
        while(elapsedTime <= timeToWait)
        {
            elapsedTime += Time.deltaTime;
            Vector3 ab = Vector3.Lerp(unitPos, peakOfHeight, elapsedTime / timeToWait);
            Vector3 bc = Vector3.Lerp(peakOfHeight, endDestination, elapsedTime/timeToWait);

            expOrb.transform.position = Vector3.Lerp(ab, bc, elapsedTime / timeToWait);
            
            yield return null;

        }
        
    }

    public void StartRunningAnimation()
    {
        anim.SetBool("Moving", true);
    }
    public void StopRunningAnimation()
    {
        anim.SetBool("Moving", false);
    }
}
