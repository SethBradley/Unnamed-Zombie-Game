using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Unnamed Zombie Game/Weapon")]
public class Weapon : ScriptableObject 
{
    public string weaponName;
    public bool isRanged;
    public float attackRange;
    public float attackRadius;
    public float cooldown;
    public int damage;
    public Vector3 attackPoint; 
    
    public Sprite sprite;
    public Sprite projectile;
    public Vector3 offsetPosition;
    public Quaternion offsetRotation;

}
