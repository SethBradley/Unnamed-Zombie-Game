using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Change to WeaponHandler
public class WeaponHandler : MonoBehaviour
{
    public Human human;
    public Weapon heldWeapon;
    Transform equippedWeapon;
    public bool isReadyToAttack;
    
    public GameObject heldWeaponGameObject;
    public GameObject attackPoint;

    private void Start() 
    {
        human = GetComponent<Human>();
        equippedWeapon = transform.Find("EquippedWeapon"); 
    }
    


    public IEnumerator UseMeleeWeapon()
    {
        if(isReadyToAttack)
        {
            
        }
        isReadyToAttack = false;

        yield return new WaitForSeconds(heldWeapon.cooldown);

        isReadyToAttack = true;
    }

    public void EquipWeapon(Transform weapon)
    {
        Debug.Log("Equipping Weapon");
        heldWeaponGameObject = weapon.gameObject;
        
        heldWeapon = weapon.GetComponent<WeaponModel>().weapon;
        //SetAttackPoint();
        
        equippedWeapon.transform.position = Vector3.zero;
        equippedWeapon.localPosition = heldWeapon.offsetPosition;
        equippedWeapon.localRotation = heldWeapon.offsetRotation;
        equippedWeapon.GetComponent<SpriteRenderer>().sprite = heldWeapon.sprite;


        Object.Destroy(weapon.gameObject);
        //weapon.transform.SetParent(this.transform);
        //Need to destroy weapon then instantiate new held weapon
        //weapon.SetPositionAndRotation(heldWeapon.offsetPosition, heldWeapon.offsetRotation);
    }

    private void SetAttackPoint()
    {
        //heldWeaponGameObject.transform
    }

    public IEnumerator Attack()
    {
        
        human.anim.SetTrigger("MeleeAttack");
        HashSet<Unit> attackedUnits = new HashSet<Unit>();

        while(!human.isOnCooldown)
        {
            Collider2D[] enemiesAttackedArray = Physics2D.OverlapCircleAll(attackPoint.transform.position, heldWeapon.attackRadius,256);

            foreach(var enemy in enemiesAttackedArray)
            {
                Unit uniqueTarget = enemy.GetComponent<Unit>(); 
                attackedUnits.Add(uniqueTarget);
            }
        }

        attackedUnits.Clear(); 
        yield return null;
    }


    private void OnDrawGizmosSelected() 
    {

        Gizmos.DrawWireSphere(attackPoint.transform.position, heldWeapon.attackRadius);
    }
}
