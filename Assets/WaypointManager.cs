using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public GameObject waypointParent;
    public GameObject exitParent;
    public static WaypointManager instance; 
    public List<Transform> waypoints;
    public List<Transform> exits;

    private void Awake() 
    {

        if (instance != null && instance != this)
            Destroy(this.gameObject);
         else 
            instance = this;

        for (int i = 0; i < waypointParent.transform.childCount; i++)
            waypoints.Add(waypointParent.transform.GetChild(i));

        for (int i = 0; i < exitParent.transform.childCount; i++)
            exits.Add(exitParent.transform.GetChild(i));
        
    }


    public Transform GetRandomWaypoint()
    {
        int random = UnityEngine.Random.Range(0, waypoints.Count);
        Transform waypoint = waypoints[random];
        return waypoint;
    }
    public Transform GetFarthestExit(Unit unit)
    {
        //could change this to nearest easily
        int random = UnityEngine.Random.Range(0, exits.Count);
        Transform currentExit = exits[random];
        float currentExitDistance = Vector3.Distance(currentExit.transform.position, unit.transform.position);
        for (int i = 0; i < exits.Count; i++)
        {
            Debug.Log($"current exit distance is {currentExitDistance} and new exit distance is{Vector3.Distance(exits[i].transform.position, unit.transform.position)} if second is bigger, set that to current exit");
            if(currentExitDistance < Vector3.Distance(exits[i].transform.position, unit.transform.position))
            {
                
                currentExit = exits[i].transform;
                Debug.Log($"Changed current exit to {currentExit}");
            }
        }
        return currentExit;
    }
}
