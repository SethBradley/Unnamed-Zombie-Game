using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionHandler : MonoBehaviour
{
    public List<GameObject> nearbyUnits;
    public NormalZombie unit;
    public float detectionRadius;
    public LayerMask detectableUnits;
    
    public bool isDetecting;

    private void Start() 
    {
        nearbyUnits = new List<GameObject>();
        StartCoroutine(UpdateNearbyUnits());
        
    }
    WaitForSeconds buffer = new WaitForSeconds(0.5f);
    
    public IEnumerator UpdateNearbyUnits()
    {
        while(isDetecting)
        {
            
            yield return buffer;

            nearbyUnits.Clear();
            Collider2D[] nearbyUnitsArray= Physics2D.OverlapCircleAll(unit.transform.position,detectionRadius,detectableUnits);

            for (int i = 0; i < nearbyUnitsArray.Length; i++)
            {
                if(!nearbyUnits.Contains(nearbyUnitsArray[i].gameObject))
                {
                    nearbyUnits.Add(nearbyUnitsArray[i].gameObject);
                }
            }
        }
    }

    public GameObject GetClosestUnitInRange()
    {
        GameObject closestUnit = null;
        if(nearbyUnits.Count == 0)
            return null;

        
        else if(nearbyUnits.Count >= 1)
            closestUnit = nearbyUnits[0];

        
        for (int i = 0; i < nearbyUnits.Count; i++)
            {
                float distanceFromClosestUnit = Vector3.Distance(unit.transform.position, closestUnit.transform.position);
                float distanceFromOtherUnit = Vector3.Distance(unit.transform.position, nearbyUnits[i].transform.position);
                if(distanceFromOtherUnit <= distanceFromClosestUnit)
                {
                    closestUnit = nearbyUnits[i].gameObject;
                }
            }
        return closestUnit;

    }


    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(unit.transform.position,detectionRadius);
    }




}
