using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TurnHumans : Ability
{
    public int abilityLevel;
    public float abilityRadius;
    float humanInRadiusCount; 
    GameObject indicator;
    public List<Human> humansWithinRadius = new List<Human>();
    public AudioClip countSound;
    public AudioSource audioSource;
    WaitForSeconds shortBuffer = new WaitForSeconds(0.10f);
    
    
    
    
    public override IEnumerator Enter()
    {
        
        audioSource = GetComponent<AudioSource>();
        Debug.Log("Beginning To Cast TurnHumans");
      
        isAimingAbility = true;
        
        
        indicator = Instantiate(abilityIndicator, Input.mousePosition, transform.rotation);
        
        Vector3 abilityRadiusVector = new Vector3(abilityRadius, abilityRadius, 0f);
        abilityIndicator.transform.localScale = abilityRadiusVector;
        
        StartCoroutine(HandleTurnCountIndicator());
        while(isAimingAbility)
        {
            
            mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            indicator.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f);

            UpdateHumansWithinRadius();
            if(Input.GetMouseButtonDown(0))
            {
                yield return Execute();
                break;
            }
            yield return null;
        }


        yield return null;
    }

    private void UpdateHumansWithinRadius()
    {
        humansWithinRadius.Clear();

        SpriteRenderer indicatorSprite = indicator.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        Collider2D[] humanDetectionCircle = Physics2D.OverlapCircleAll(mousePosition, indicatorSprite.bounds.extents.x/2.1f ,128);
        foreach (var human in humanDetectionCircle)
        {
            Human currentHuman = human.GetComponent<Human>();
            if(!humansWithinRadius.Contains(currentHuman) && currentHuman.isDead)
            {
                humansWithinRadius.Add(currentHuman);
            }
        }
    }

    private IEnumerator HandleTurnCountIndicator()
    {   
        GameObject turnCountIndicator = indicator.transform.Find("TurnIndicatorUI").gameObject;
        TMP_Text turnCountText = turnCountIndicator.transform.Find("Canvas").Find("Text (TMP)").GetComponent<TMP_Text>();

        int currentCount = humansWithinRadius.Count;
        while(isAimingAbility)
        {
            while(int.Parse(turnCountText.text) == humansWithinRadius.Count)
            {
                if(!isAimingAbility)
                    yield break;
    
                yield return shortBuffer;
            }
            if(humansWithinRadius.Count > 0)
            {
                turnCountIndicator.SetActive(true);
                audioSource.pitch = 1 + humansWithinRadius.Count/5f;
                audioSource.PlayOneShot(countSound);
                
                turnCountText.text = $"{humansWithinRadius.Count}";
            }
            else
            {
                turnCountIndicator.SetActive(false);
                audioSource.pitch = 1 + humansWithinRadius.Count/5f;
                audioSource.PlayOneShot(countSound);
                turnCountText.text = "0";
            }
            yield return null;  
        }
    
        StopCoroutine(HandleTurnCountIndicator());

    }

    public override IEnumerator Execute()
    {
        isAimingAbility = false; 
        foreach (var human in humansWithinRadius)
        {
            Debug.Log("Attempting to turn " + human.name);
        }
        yield return Exit();
    }

    public override IEnumerator Exit()
    {
        AbilityCastController.instance.isCastingAbility = false;
        UnityEngine.Object.Destroy(indicator);
        yield return new WaitForSeconds(0.10f);
    }
}
