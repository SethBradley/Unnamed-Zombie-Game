using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameplayHandler : MonoBehaviour
{
    public static GameplayHandler instance;

    [Header("Panic")]
    public PanicMeter panicMeter;
    public float totalPanic;
    //public int TotalPanic{get {return totalPanic;} set {totalPanic = Mathf.Clamp(value, 0, 100);}}

    private void Awake() 
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
         else 
            instance = this;
        
    }

#region Panic
    public void IncreasePanic(int increaseAmount)
    {
        
        totalPanic += increaseAmount;
        EventBus.TriggerEvent("IncreasePanic");
        if(totalPanic >= 100)
            Debug.Log("At maximum panic");
        

    }
#endregion

}
