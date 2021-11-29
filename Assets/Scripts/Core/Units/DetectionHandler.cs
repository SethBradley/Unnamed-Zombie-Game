using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionHandler : MonoBehaviour
{
    public List<Unit> nearbyUnits;
    public List<GameObject> nearbyHumans;
    public List<Zombie> nearbyZombies;
    public List<Transform> nearbyWeapons;
    public float unitDetectionRadius;
    public LayerMask detectableUnits;
    public LayerMask detectableWeapons;
    
    public bool isDetecting;

    private void Start() 
    {
        nearbyUnits = new List<Unit>();
        nearbyHumans = new List<GameObject>();
        nearbyZombies = new List<Zombie>();

        StartCoroutine(UpdateNearbyUnits());
        
    }
    WaitForSeconds buffer = new WaitForSeconds(0.5f);
    
    //need to create an UpdateNearbyHumans and UpdateNearbyZombies
    public IEnumerator UpdateNearbyUnits()
    {
        while(isDetecting)
        {
            
            yield return buffer;
            nearbyHumans.Clear();
            nearbyZombies.Clear();
            nearbyUnits.Clear();
            Collider2D[] nearbyUnitsArray= Physics2D.OverlapCircleAll(transform.position,unitDetectionRadius,detectableUnits);

            for (int i = 0; i < nearbyUnitsArray.Length; i++)
            {
                Unit nearbyUnit = nearbyUnitsArray[i].gameObject.GetComponent<Unit>();
                if(!nearbyUnits.Contains(nearbyUnit) && !nearbyUnit.isDead && nearbyUnit != this.GetComponent<Unit>())
                {
                    nearbyUnits.Add(nearbyUnit);
                }
            }

            AllocateNearbyUnitsIntoLists();
        }
    }

    private void AllocateNearbyUnitsIntoLists()
    {
//        Debug.Log("Allocating");
        foreach (var unit in nearbyUnits)
        {
            if (unit.gameObject.layer == 7)
            {
                nearbyHumans.Add(unit.gameObject);
            }
            else if(unit.gameObject.layer == 8)
            {
                nearbyZombies.Add(unit.GetComponent<Zombie>());
            }
        }
    }

    public Unit GetClosestUnitInRange()
    {
        Unit closestUnit = null;
        if(nearbyUnits.Count == 0)
            return null;

        else if(nearbyUnits.Count >= 1)
            closestUnit = nearbyUnits[0];

        for (int i = 0; i < nearbyUnits.Count; i++)
            {
                try
                {
                        float distanceFromClosestUnit = Vector3.Distance(transform.position, closestUnit.transform.position);
                        float distanceFromOtherUnit = Vector3.Distance(transform.position, nearbyUnits[i].transform.position);
                        if(distanceFromOtherUnit <= distanceFromClosestUnit)
                        {
                            closestUnit = nearbyUnits[i];
                        }
                }
                catch
                {
                    Debug.Log("Unit mightve dissapeared");
                }
            }
                return closestUnit;
    }


    public Transform DetectNearestWeapon()
    {
        Collider2D[] nearbyWeaponsArray= Physics2D.OverlapCircleAll(transform.position,unitDetectionRadius,detectableWeapons);
        Transform closestWeapon = null;
        if(nearbyWeaponsArray.Length == 0)
            return null;

        else if(nearbyWeaponsArray.Length == 1)
            return nearbyWeaponsArray[0].transform;
        
        closestWeapon = nearbyWeaponsArray[0].transform;
        for (int i = 0; i < nearbyWeaponsArray.Length; i++)
            {   
                if(Vector3.Distance(transform.position, nearbyWeaponsArray[i].transform.position) > Vector3.Distance(transform.position, closestWeapon.position))
                {
                    closestWeapon = nearbyWeaponsArray[i].transform;
                }
                
            }
        Debug.Log(closestWeapon.name);
        return closestWeapon;
    }



    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position,unitDetectionRadius);
        
    }




}
