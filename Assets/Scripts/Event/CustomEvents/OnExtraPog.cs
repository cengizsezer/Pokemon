using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnExtraPog : CustomEvent
{
    public static OnExtraPog Create()
    {
        OnExtraPog newEvent = new OnExtraPog();
        return newEvent;
    }
}
