using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Change to WeaponHandler
public class WeaponHandler : MonoBehaviour
{
    public Unit unit;
    public Weapon heldWeapon;
    Transform equippedWeapon;
    public bool isOnCooldown;
    
    public GameObject heldWeaponGameObject;
    public GameObject attackPoint;

    public Vector3 AoESize;
    public Vector3 AoEOffset;
    [Header("Debugging")]
    public bool enableAoESizeDebugger;

    private void Start() 
    {
        unit = GetComponent<Unit>();
        equippedWeapon = transform.Find("EquippedWeapon"); 
    }
    


    public IEnumerator UseMeleeWeapon(Unit target)
    {

        if (!isOnCooldown)
        {
            if(!heldWeapon.isSingleTarget)
                AoeMeleeAttack(target);
        }
        else
            yield break;
        isOnCooldown = true;

        yield return new WaitForSeconds(heldWeapon.cooldown);

        isOnCooldown = false;
    }

    private void AoeMeleeAttack(Unit target)
    {
        RaycastHit2D[] targetsInRange = Physics2D.BoxCastAll(target.transform.position + AoEOffset, AoESize, 0, Vector2.zero, 10f, 256 );
        for (int i = 0; i < targetsInRange.Length; i++)
        {
            Unit currentTarget = targetsInRange[i].transform.gameObject.GetComponent<Unit>();
            currentTarget.TakeDamage(heldWeapon.damage);
            currentTarget.StartCoroutine(currentTarget.locomotionHandler.GetKnockedBack(heldWeapon.knockbackAmount, transform.position));
        }
    }
    public void EquipWeapon(Transform weapon)
    {
        Debug.Log("Equipping Weapon");
        heldWeaponGameObject = weapon.gameObject;
        
        heldWeapon = weapon.GetComponent<WeaponModel>().weapon;
        
        equippedWeapon.transform.position = Vector3.zero;
        equippedWeapon.localPosition = heldWeapon.offsetPosition;
        equippedWeapon.localRotation = heldWeapon.offsetRotation;
        equippedWeapon.GetComponent<SpriteRenderer>().sprite = heldWeapon.sprite;
        //equippedWeapon.gameObject.AddComponent<BoxCollider2D>();


        Object.Destroy(weapon.gameObject);
        //weapon.transform.SetParent(this.transform);
        //Need to destroy weapon then instantiate new held weapon
        //weapon.SetPositionAndRotation(heldWeapon.offsetPosition, heldWeapon.offsetRotation);
    }







    private void OnDrawGizmosSelected() 
    {
        if(attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.transform.position, heldWeapon.attackRadius);
    }

    private void OnDrawGizmos() 
    {

        if(enableAoESizeDebugger)
        {
            try
            {
                if (unit.target != null)
                {
                    Gizmos.DrawWireCube(unit.target.transform.position + AoEOffset, AoESize);   
                }
            }
            catch{} 
        }

    }
}
