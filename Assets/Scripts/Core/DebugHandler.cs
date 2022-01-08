using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugHandler : MonoBehaviour
{
    public GameObject headerDebugWindow;
    public TMP_Text debugText;

    private void Start() 
    {
        debugText = headerDebugWindow.transform.Find("DebugTextCanvas").Find("DebugText").GetComponent<TMP_Text>();
    }


    public void SetDebugText(string newText)
    {
        debugText.text = newText; 
    }

}
