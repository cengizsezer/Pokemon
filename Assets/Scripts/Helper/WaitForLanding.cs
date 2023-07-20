using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForLanding
{
    List<MobBase> ls;
    public WaitForLanding(List<MobBase> list)
    {
        ls = list;
    }

    public void Execute(MonoBehaviour parent, CustomEvent OnEvent)
    {
        parent.StartCoroutine(WaitLandingRoutine(OnEvent));
    }
    IEnumerator WaitLandingRoutine(CustomEvent OnEvent)
    {
        Debug.Log("waitlanding");
        yield return new WaitUntil(()=>HasAllLanding());
        yield return new WaitForSeconds(.3f);
        Debug.Log("bureaya mi girmedi");
        EventManager.Send(OnEvent);
    }

    bool HasAllLanding()
    {
        foreach (var pm in ls)
        {
            if (pm.settled)
            {
                return true;
            }
        }
        return false;
    }
}
