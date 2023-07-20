using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Singleton<T> : MonoBehaviour where  T : MonoBehaviour
{
    public static T I = null;

    public virtual void Awake()
    {
        if(I == null)
        {
            I = this as T;
        }
    }
}
