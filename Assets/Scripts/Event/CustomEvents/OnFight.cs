using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFight : CustomEvent
{
    public bool on;
    public static OnFight Create()
    {
        OnFight newEvent = new OnFight();
        newEvent.on = false;
        return newEvent;
    }
}
