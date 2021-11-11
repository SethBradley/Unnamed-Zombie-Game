using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamagePopup : MonoBehaviour
{
    
    public static DamagePopup Create(Vector3 position, int attackDamage,Transform pfDamagePopup)
    {
        Transform damagePopupTransform = Instantiate(pfDamagePopup, position, Quaternion.identity);

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();

        //int randomNum = UnityEngine.Random.Range(1,100);
        damagePopup.Setup(attackDamage);

        return damagePopup;
        
    }
    
    private static int sortingOrder;

    private TextMeshPro textMesh;
    private float dissappearTimer;
    private Color textColor;
    private void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
    }
  public void Setup(float attackDamage){
    textMesh.SetText(attackDamage.ToString());
    textColor =textMesh.color;
    dissappearTimer = 1f;
    sortingOrder++;
    textMesh.sortingOrder = sortingOrder;

      
  }

   private void Update() {
    float moveYspeed = 5f;
    transform.position += new Vector3(0, moveYspeed) * Time.deltaTime;
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
