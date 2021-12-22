using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
 
    
    
    public float panSpeed =30f;
    public float panBoarderThickness = 10f;
    private float scrollSpeed = 10f;
    private Camera zoomCamera;
    public float initialFOV;
    public float zoomInFOV;
    public float smooth;
    private float currentFOV;
    public float maxFOV;

    
    private void Start() {
        Camera.main.fieldOfView = initialFOV;
        
    }
   

    // Update is called once per frame
    void Update()
    {
    ChangeFOV();
    HandleMovement();

    
    
        
    currentFOV = Camera.main.fieldOfView;

    

    }
   
    private void HandleMovement()
    {
        if(Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBoarderThickness ) 
        {
         
           transform.Translate(Vector2.up * panSpeed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey("s") || Input.mousePosition.y <=  panBoarderThickness ) 
        {
         
           transform.Translate(Vector2.down * panSpeed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBoarderThickness ) 
        {
         
           transform.Translate(Vector2.right * panSpeed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey("a") || Input.mousePosition.x <=  panBoarderThickness ) 
        {
         
           transform.Translate(Vector2.left * panSpeed * Time.deltaTime, Space.World);
           
        }

   
        
    




    }

      private void ChangeFOV()
    {
        if(Input.mouseScrollDelta.y > 0)
        {
            if(Camera.main.fieldOfView > 1)
            {
                Camera.main.fieldOfView--;
            }
            
        }
        if(Input.mouseScrollDelta.y < 0)
        {
            if(Camera.main.fieldOfView < 100)
            {
            Camera.main.fieldOfView++;
            }
        }
        
    }
}

