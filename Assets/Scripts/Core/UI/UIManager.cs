using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum WindowType
{
    PauseMenu,
    VictoryScreen,
    DefeatScreen
}
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    List<WindowController> windowControllerList;
    public WindowController lastActiveWindow;
    private void Awake() 
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
         else 
            instance = this;
        
        windowControllerList = GetComponentsInChildren<WindowController>(true).ToList();
        windowControllerList.ForEach(window => window.gameObject.SetActive(false));

    }

    public void SwitchWindow(WindowType toWindowType)
    {
        if(lastActiveWindow != null)
            lastActiveWindow.gameObject.SetActive(false);
        

        WindowController desiredWindow = windowControllerList.Find(window => window.windowType == toWindowType);
        
        if(desiredWindow != null)
        {
            desiredWindow.gameObject.SetActive(true);
            lastActiveWindow = desiredWindow;
        }
    }

    
}
