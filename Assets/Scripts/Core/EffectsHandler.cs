using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsHandler : MonoBehaviour
{
    public float waitTime;
    public Vector3 spriteEffectOffset;
    public SpriteRenderer spriteRenderer;
    public ParticleSystem[] particles;
    private Animator anim;

    private void Start() {
        anim = GetComponent<Animator>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        spriteEffectOffset = transform.Find("Sprite").transform.localPosition;
    }


    public IEnumerator TakeDamageEffect()
    {
        anim.SetTrigger("Hit");
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
}
