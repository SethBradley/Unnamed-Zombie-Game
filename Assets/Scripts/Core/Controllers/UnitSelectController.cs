using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectController : MonoBehaviour
{
    public static UnitSelectController instance;
    public List<Unit> selectedZombiesList;
    public GameObject currentSelected;
    private Vector3 clickStartPos;
    public Camera mainCamera;
    public GameObject selectionArea;
    public IEnumerator dragSelection_Co;
    private Collider2D[] collider2DArray;
    

    private void Awake() 
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
         else 
            instance = this;

        mainCamera = Camera.main;
        selectionArea = transform.Find("SelectionArea").gameObject;
        dragSelection_Co = null;
    }

    private void Update() 
    {
        if(Input.GetMouseButtonDown(0) && !AbilityCastController.instance.isCastingAbility)
        {
            // Left Mouse Button Pressed
            clickStartPos = GetMouseWorldPosition();
            
            if(Input.GetKey(KeyCode.LeftControl))
                SingleSelectZombie();
            else if (dragSelection_Co == null)
            {
                dragSelection_Co = DragSelection();
                StartCoroutine(dragSelection_Co);
            }


        }
    }

    private void SingleSelectZombie()
    {
        //Single Selection (ctrl + left click)
        NormalZombie singleSelectZombie = GetZombieUnderMouse();
        if (selectedZombiesList.Contains(GetZombieUnderMouse()))
            DeselectZombie(singleSelectZombie);
        else
        {
            SelectZombie(singleSelectZombie);
        }
    }

    public IEnumerator DragSelection()
    {
        if(!Input.GetKey(KeyCode.LeftControl))
            DeselectAllZombies();
        while(Input.GetMouseButton(0))
        {
            DrawSelectionBox();
            SelectZombiesWithinDragArea();
            yield return null;
        }
        selectionArea.SetActive(false);
        dragSelection_Co = null;
    }

    private void SelectZombie(NormalZombie singleSelectZombie)
    {
        if(singleSelectZombie != null)
        {
            selectedZombiesList.Add(singleSelectZombie);
            singleSelectZombie.effectsHandler.SetSelectIndicatorActive(true);
        }

    }

    private void DrawSelectionBox()
    {
        selectionArea.SetActive(true);
        Vector3 currentMousePos = GetMouseWorldPosition();
        Vector3 lowerLeft = new Vector3(
            Mathf.Min(clickStartPos.x, currentMousePos.x),
            Mathf.Min(clickStartPos.y, currentMousePos.y)
        );

        Vector3 upperRight = new Vector3(
            Mathf.Max(clickStartPos.x, currentMousePos.x),
            Mathf.Max(clickStartPos.y, currentMousePos.y)
        );

        selectionArea.transform.position = lowerLeft;
        selectionArea.transform.localScale = upperRight - lowerLeft;
    }

    private void SelectZombiesWithinDragArea()
    {
        collider2DArray = Physics2D.OverlapAreaAll(clickStartPos, GetMouseWorldPosition());

        foreach (Collider2D collider2D in collider2DArray)
        {
            NormalZombie selectedZombie = collider2D.GetComponent<NormalZombie>();
            if (selectedZombie != null)
            {
                if(!selectedZombiesList.Contains(selectedZombie))
                {
                    Debug.Log("Zombie has been selected");
                    SelectZombie(selectedZombie);
                }

            }
        }
    }
    private NormalZombie GetZombieUnderMouse()
    {
        RaycastHit2D[] hit =  Physics2D.RaycastAll(GetMouseWorldPosition(), Vector2.zero);
        if(hit.Length > 0)
            return hit[0].transform.GetComponent<NormalZombie>();
        else
            return null;
    }

    private void DeselectZombie(NormalZombie zombie)
    {
        if(selectedZombiesList.Contains(zombie))
        {
            selectedZombiesList.Remove(zombie);
            zombie.effectsHandler.SetSelectIndicatorActive(false);
        }
    }


    private void DeselectAllZombies()
    {
        foreach (Zombie zombie in selectedZombiesList)
            zombie.effectsHandler.SetSelectIndicatorActive(false);
        selectedZombiesList.Clear();
    }


    public Vector3 GetMouseWorldPosition() 
    {
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0f;
            return worldPosition;
    }
}
