using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceMechanic : TouchBase
{
    [SerializeField] ChoiceHandController Hand;
    PogBehaviour choosePog;
    public override void OnDown()
    {
        InputManager.I.Initialize(InputManager.TouchTypes.NONE);
        Hand.repeatable = false;
        Hand.StopRoutine();
        Hand.ClosedChooseHand();
        Hand.Rest();
        DeckManager.I.ClosedChooseBox();
        choosePog = Hand.crntPog;
        choosePog.transform.parent = null;
        choosePog.transform.DOMoveY(2f, 0.2f);
        choosePog.ExtraPogBehaviour(.4f);

    }

    public override void OnDrag()
    {

    }

    public override void OnUp()
    {

    }
}
