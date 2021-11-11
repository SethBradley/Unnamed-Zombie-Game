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
    
    

    private void Start() {
        unit = GetComponent<Unit>();
        anim = GetComponent<Animator>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        spriteEffectOffset = transform.Find("Sprite").transform.localPosition;
        
        

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
    
}
