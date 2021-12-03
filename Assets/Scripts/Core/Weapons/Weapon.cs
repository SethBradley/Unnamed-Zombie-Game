using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Unnamed Zombie Game/Weapon")]
public class Weapon : ScriptableObject 
{
    public string weaponName;
    public bool isRanged;
    public bool isSingleTarget;
    public float attackRange;
    public float knockbackAmount;
    public float attackRadius;
    public float cooldown;
    public int damage;
    public Vector3 attackPoint; 
    public AudioClip weaponAttackSound;
    public Sprite sprite;
    public GameObject projectile;
    public Vector3 shootPoint;
    public Vector3 offsetPosition;
    public Quaternion offsetRotation;

}
