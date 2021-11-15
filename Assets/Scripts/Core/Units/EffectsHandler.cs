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
    public Image Healthbar;
    public GameObject xpGameObj;
    public AnimationCurve xpPositionCurve;
    
    

    private void Start() {
        unit = GetComponent<Unit>();
        anim = GetComponent<Animator>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        spriteEffectOffset = transform.Find("Sprite").transform.localPosition;
        //StartCoroutine(DropExp());
        

//        Healthbar = unit.transform.Find("Healthbar").GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
//        Healthbar.transform.parent.gameObject.SetActive(false);
    }


    public IEnumerator TakeDamageEffect()
    {
        anim.SetTrigger("Hit");
        //DecreaseHealthUI();
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
    }
 public void DecreaseHealthUI()
    { 
        if (!Healthbar.transform.parent.gameObject.activeSelf)
        {
            Healthbar.transform.parent.gameObject.SetActive(true);
        }
        Healthbar.fillAmount = unit.health;

        if (Healthbar.fillAmount <= 0)
        {
            Healthbar.transform.parent.gameObject.SetActive(false);
        }

    }
    

    public IEnumerator DropExpFor()
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
}
