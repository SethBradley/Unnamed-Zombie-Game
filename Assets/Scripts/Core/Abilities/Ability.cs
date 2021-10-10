using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public LeaderZombie leaderZombie;
    public bool isAimingAbility;
    public GameObject abilityIndicator;
    public Camera mainCamera;
    public Vector3 mousePosition;


    private void Awake() 
    {
        mainCamera = Camera.main;
        leaderZombie = transform.parent.parent.GetComponent<LeaderZombie>();        
    }
    public virtual IEnumerator Enter()
    {
        yield return null;
    }
    public virtual IEnumerator Execute()
    {
        yield return null;
    }

    public virtual IEnumerator Exit()
    {
        
        yield return null;
    }


}
