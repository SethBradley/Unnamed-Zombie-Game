using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public bool isAimingAbility;
    public GameObject AbilityIndicator;
    public Vector3 mousePosition;


    public IEnumerator DisplayAbilityIndicator()
    {
        while (isAimingAbility)
        {
            mousePosition = Input.mousePosition;
            yield return null;
        }
    }
}
