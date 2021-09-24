using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectController : MonoBehaviour
{
    public GameObject currentSelected;

    private void Awake() 
    {

    }

    private void Update() 
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            currentSelected = GetUnitUnderMouse();
        }
    }


    public GameObject GetUnitUnderMouse()
    {
        //Camera.main is nasty toss this
        Vector2 raycastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(raycastPos, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.tag == "KingpinZombie")
            {
                return hit.collider.gameObject;    
            }
            
        }

        return null;
    }
}
