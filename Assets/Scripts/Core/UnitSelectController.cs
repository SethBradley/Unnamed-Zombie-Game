using System;
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
        


        if (Input.GetMouseButtonDown(1) 
            && currentSelected.GetComponent<IMovable>() != null)
        {
            MoveSelectedUnit();
        }       
        
    }

    private void MoveSelectedUnit()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentSelected.GetComponent<LocomotionHandler>().MoveToMouseLocation(mousePos);

    }

    public GameObject GetUnitUnderMouse()
    {
        //Camera.main is nasty toss this
        Vector2 raycastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(raycastPos, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.GetComponent<ISelectable>() != null)
            {
                return hit.collider.gameObject;    
            }
            
        }
        DeselectUnit();
        return null;
    }

    private void DeselectUnit()
    {
        currentSelected = null;
    }
}
