using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{

    
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
