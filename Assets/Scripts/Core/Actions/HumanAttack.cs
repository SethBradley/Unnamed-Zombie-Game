using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAttack : State
{
    protected Human unit;
    public HumanAttack(Human _unit)
    {
        unit = _unit;
    }
    public override IEnumerator Enter()
    {
        Debug.Log("Entering civilian attack state");
        yield return null;
    }

    public override IEnumerator Execute()
    {
        yield return null;
    }

    public override IEnumerator Exit()
    {
        yield return null;
    }

}
