using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionHandler : MonoBehaviour
{
    public List<Unit> nearbyUnits;
    public float unitDetectionRadius;
    public LayerMask detectableUnits;
    
    public bool isDetecting;

    private void Start() 
    {
        nearbyUnits = new List<Unit>();
        StartCoroutine(UpdateNearbyUnits());
        
    }
    WaitForSeconds buffer = new WaitForSeconds(0.5f);
    
    public IEnumerator UpdateNearbyUnits()
    {
        while(isDetecting)
        {
            
            yield return buffer;

            nearbyUnits.Clear();
            Collider2D[] nearbyUnitsArray= Physics2D.OverlapCircleAll(transform.position,unitDetectionRadius,detectableUnits);

            for (int i = 0; i < nearbyUnitsArray.Length; i++)
            {
                Unit nearbyUnit = nearbyUnitsArray[i].gameObject.GetComponent<Unit>();
                if(!nearbyUnits.Contains(nearbyUnit) && !nearbyUnit.isDead)
                {
                    nearbyUnits.Add(nearbyUnit);
                }
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
            if (!closestUnit.isDead)
                return closestUnit;
            else
                return null;

    }


    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(this.transform.position,unitDetectionRadius);
    }




}
