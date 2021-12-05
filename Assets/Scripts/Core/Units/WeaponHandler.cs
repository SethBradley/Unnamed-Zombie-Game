using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Change to WeaponHandler
public class WeaponHandler : MonoBehaviour
{
    public Unit unit;
    public Weapon heldWeapon;
    Transform equippedWeapon;
    
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

    public void Shoot()
    {
        //Get Shoot Direction/Rotation
        float hitChance = UnityEngine.Random.Range(0,1);
        Vector3 bulletDir;
        if (hitChance < 0.9f)
        {
            bulletDir = unit.target.transform.position - transform.position;
        }
        else
            bulletDir = new Vector3(UnityEngine.Random.Range(unit.target.transform.position.x - 2, unit.target.transform.position.x + 2 ), UnityEngine.Random.Range(unit.target.transform.position.y - 2, unit.target.transform.position.y + 2 ), 0f) - transform.position;
         
        //if(heldWeapon is a pistol)
        ShootPistol(bulletDir);
    }

    public void ShootPistol(Vector3 bulletDir)
    {
        
        
        //Quaternion bulletRotation = new Quaternion(0f,0f,Vector2.Angle(transform.position, unit.target.transform.position),0);
        //Debug.Log(Vector2.SignedAngle(transform.position, unit.target.transform.position));
        GameObject newBullet = Instantiate(heldWeapon.projectile, transform.position + heldWeapon.shootPoint, Quaternion.identity );
        
        newBullet.GetComponent<BulletProjectile>().Init(bulletDir, heldWeapon.damage);
    }
    


    public void UseMeleeWeapon()
    {
        if(unit.target.isDead)
        {
            unit.target = null;
            return;
        }
            

        if (!unit.isOnCooldown)
        {
            if(!heldWeapon.isSingleTarget)
                AoeMeleeAttack(unit.target);
        }
        // else
        //     singleTargetATtack();
        
        
        StartCoroutine(unit.StartCooldown());


    }

    private void AoeMeleeAttack(Unit target)
    {
        Collider2D[] targetsInRange = Physics2D.OverlapBoxAll(target.transform.position + AoEOffset, AoESize, 0f, 256);
        for (int i = 0; i < targetsInRange.Length; i++)
        {
            Unit currentTarget = targetsInRange[i].transform.gameObject.GetComponent<Unit>();
//            Debug.Log(currentTarget.name);
            currentTarget.TakeDamage(heldWeapon.damage);
            //if (!currentTarget.locomotionHandler.isGettingKnockedBack)
                //currentTarget.StartCoroutine(currentTarget.locomotionHandler.GetKnockedBack(heldWeapon.knockbackAmount, transform.position));
        }
    }
    public void EquipWeapon(Transform weapon)
    {
        Debug.Log("Equipping Weapon");

        //try
        //{
            heldWeaponGameObject = weapon.gameObject;
        
            heldWeapon = weapon.GetComponent<WeaponModel>().weapon;
            equippedWeapon.GetComponent<SpriteRenderer>().sprite = heldWeapon.sprite;
            Object.Destroy(weapon.gameObject);

            
            unit.cooldown = heldWeapon.cooldown;
            equippedWeapon.transform.position = Vector3.zero;
            equippedWeapon.localPosition = heldWeapon.offsetPosition;
            equippedWeapon.localRotation = heldWeapon.offsetRotation;
            
            unit.attackDamage += heldWeapon.damage;
   
        //}
        //catch
        //{
       //     Debug.Log("Weapon already picked up - carry on without it");
        //}

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
