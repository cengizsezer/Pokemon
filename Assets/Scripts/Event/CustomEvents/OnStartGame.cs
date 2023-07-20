using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartGame : CustomEvent
{
    public static OnStartGame Create()
    {
        OnStartGame newEvent = new OnStartGame();
        return newEvent;
    }
}
