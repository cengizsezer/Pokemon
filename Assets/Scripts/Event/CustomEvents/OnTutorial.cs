using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTutorial : CustomEvent
{
    public static OnTutorial Create()
    {
        OnTutorial newEvent = new OnTutorial();

        return newEvent;
    }
}
