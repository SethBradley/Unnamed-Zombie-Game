using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamagePopup : MonoBehaviour
{
    private static int sortingOrder;

    private TextMeshPro textMesh;
    private float dissappearTimer;
    private Color textColor;
    private Vector3 randomPoint;
    private float random;
    
    public static DamagePopup Create(Vector3 position, float attackDamage,Transform pfDamagePopup)
    {
        Transform damagePopupTransform = Instantiate(pfDamagePopup, position, Quaternion.identity);
        
        
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();

        //int randomNum = UnityEngine.Random.Range(1,100);
        damagePopup.Setup(attackDamage,position);

        return damagePopup;
        
    }
    

    private void Awake() 
    {
      random = UnityEngine.Random.Range(-1f,1f);
      textMesh = transform.GetComponent<TextMeshPro>();
        
    }
  public void Setup(float attackDamage, Vector3 position){
    textMesh.SetText(attackDamage.ToString());
    textColor =textMesh.color;
    dissappearTimer = 1f;
    sortingOrder++;
    textMesh.sortingOrder = sortingOrder;
    randomPoint = new Vector3(position.x + UnityEngine.Random.Range(-1f,1f), position.y + UnityEngine.Random.Range(-1f,1f), 0f);

      
  }

   private void Update() {
    //float moveYspeed = 5f;

    transform.position += new Vector3(random, random) * Time.deltaTime;
    transform.localScale -= new Vector3(0.01f,0.01f,0);// * Time.deltaTime;
    dissappearTimer -= Time.deltaTime;
    if(dissappearTimer < 0) {
        float dissappearSpeed = 3f;
        textColor.a -= dissappearSpeed * Time.deltaTime;
        textMesh.color = textColor;
        if(textColor.a < 0 ){
            Destroy(gameObject);
        }
    }
  }  


}
