using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SethB.Utilities
{
public class Utilities
{
    public static Camera mainCamera = Camera.main;


    public static Vector3 GetMouseWorldPosition() 
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0f;
        return worldPosition;
    }

    public static bool CheckIfPositionIsWalkable(Vector3 pos)
    {
        RaycastHit2D[] hitsArray = Physics2D.RaycastAll(pos, Vector2.zero);
        if (hitsArray.Length > 0)
        {
            foreach(RaycastHit2D hit in hitsArray)
            {
                if(hit.transform.gameObject.layer == 6)
                    return false;
                else
                    return true;
            }
        }

        return true;
    }

}
}