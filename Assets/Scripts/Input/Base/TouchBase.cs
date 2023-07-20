using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TouchBase:MonoBehaviour
{
    [HideInInspector]
    public Vector3 fp, lp, dif;

    public virtual void OnInitialized()
    {

    }

    public virtual void OnDeInitialized()
    {

    }

    public virtual void OnUpdate()
    {

    }

    public abstract void OnDown();
    public abstract void OnDrag();
    public abstract void OnUp();
}
