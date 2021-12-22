using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PanAndZoom : MonoBehaviour
{
    [SerializeField]
    private float panSpeed =2f;
    [SerializeField]
    private float zoomSpeed =3f;
    [SerializeField]
    private float zoomInMax = 10f;
    private float zoomOutMax = 80f;
    public float panBoarderThickness = 10f;

    private CinemachineInputProvider inputProvider;
    private CinemachineVirtualCamera virtualCamera;
    private Transform cameraTransform;

    private void Awake() {
        inputProvider = GetComponent<CinemachineInputProvider>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cameraTransform = virtualCamera.VirtualCameraGameObject.transform;
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        //float x = inputProvider.GetAxisValue(0);
        //float y = inputProvider.GetAxisValue(1);
        //float z = inputProvider.GetAxisValue(2);
       // if(x != 0 || y != 0)
        {
        //    PanScreen(x,y);
        }
       // if (z != 0) {
          //  zoomScreen(z);
        //}
    }
    public void zoomScreen (float increment)
    {
        float fov = virtualCamera.m_Lens.OrthographicSize;
        float target = Mathf.Clamp(fov + increment, zoomInMax, zoomOutMax);
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(fov, target, zoomSpeed * Time.deltaTime );
    }
    public Vector2 PanDirection(float x, float y) {
        Vector2 direction = Vector2.zero;
        if (Input.GetKey("w")) 
        {
            direction.y += 1;
        }
        else if (Input.GetKey("s")) 
        {
            direction.y -=1;
        }
        if(Input.GetKey("a"))
        {
            direction.x +=1;
        }
        else if (Input.GetKey("d")) 
        {
            direction.x -= 1;
        }
        return direction;
    }

    public void PanScreen(float x, float y) 
    {
        Vector2 direction = PanDirection(x, y);
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTransform.position + (Vector3)direction, panSpeed * Time.deltaTime);
    }
}
