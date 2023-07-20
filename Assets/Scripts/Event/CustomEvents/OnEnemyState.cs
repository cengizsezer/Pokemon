using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnemyState : CustomEvent
{
    public bool on;
    public static OnEnemyState Create()
    {
        OnEnemyState newEvent = new OnEnemyState();
        newEvent.on = true;
        return newEvent;
    }
}
