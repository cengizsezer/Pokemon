using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolObject:MonoBehaviour
{
    public abstract void OnDeactivate();


    public abstract void OnSpawn();


    public abstract void OnCreated();
    
}
