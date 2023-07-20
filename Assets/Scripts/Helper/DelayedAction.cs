using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedAction
{
    private Action Action;
    private float Delay;


    public DelayedAction(Action action,float delay)
    {
        Action = action;
        Delay = delay;

    }

    public void Execute(MonoBehaviour parent)
    {
        parent.StartCoroutine(GetCoroutine());
    }


    private IEnumerator GetCoroutine()
    {
        yield return new WaitForSeconds(Delay);
        Action?.Invoke();
    }
}
