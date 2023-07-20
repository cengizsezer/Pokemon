using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTableMechanic : TouchBase
{
    [SerializeField] Transform hand;

    [SerializeField] float crntOffsetX;
    [SerializeField] float crntOffsetZ;
    [SerializeField] float xBoundary;
    [SerializeField] float zBoundary;
    [SerializeField] float zOffset;
    [SerializeField] LayerMask TutorialLayer;
    public bool isTutorialActive;

    public override void OnDown()
    {
        fp = new Ray().GetWorldPositionOnPlaneY(2f);
        crntOffsetX = hand.transform.position.x;
        crntOffsetZ = hand.transform.position.z;

        if (LevelManager.I.IsFirstLevel())
            UIManager.I.ShowSwipeTutorial(false);
    }

    public override void OnDrag()
    {
        if (isTutorialActive == true) return;
        //UIManager.I.ShowExtraPogButton(false);
        lp = new Ray().GetWorldPositionOnPlaneY(2f);
        dif = lp - fp;
        CheckFirstLevelTutorial();




        //hand.position += dif.WithY(0);
        HandMoving(dif.x, dif.z);
        fp = lp;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(RayPosition.transform.position, -Vector3.up * 10f);
    }

    [SerializeField] Transform RayPosition;
    void CheckFirstLevelTutorial()
    {
        RaycastHit hit = default;

        if (Physics.Raycast(RayPosition.transform.position, -Vector3.up, out hit, 1000f, TutorialLayer))
        {
            if (hit.transform != null)
            {
                // el otamatik gitsin.
                InputManager.I.Initialize(InputManager.TouchTypes.NONE);
                //hand.transform.position = new Vector3(-0.5f,2f,-0.5f);
                hand.transform.DOMove(new Vector3(-0.555f, hand.transform.position.y, -0.512f), 0.2f)
                    .OnComplete(() => HitTable());
                //isTutorialActive = true;
                //OnUp();
                //up olsun.
               
            }

        }
    }

    void HandMoving(float x, float z)
    {
        crntOffsetX = Mathf.Clamp(crntOffsetX + (x), -xBoundary - .3f, xBoundary - 0.9f);
        crntOffsetZ = Mathf.Clamp(crntOffsetZ + (z), zBoundary, zBoundary + zOffset);
        Vector3 pos = hand.transform.position;
        pos.x = crntOffsetX;
        pos.z = crntOffsetZ;
        hand.transform.position = pos.WithY(2);
    }

    public override void OnUp()
    {
        if (LevelManager.I.IsFirstLevel()) return;

        HitTable();
    }

    public override void OnInitialized()
    {
        hand.gameObject.SetActive(true);

    }

    public void RepeatState()
    {
        hand.gameObject.SetActive(true);
        hand.transform.DOMoveX(hand.transform.position.x - 10f, 0.3f);
    }

    public override void OnDeInitialized()
    {

    }

    void HitTable()
    {
        InputManager.I.Initialize(InputManager.TouchTypes.PowerBar);
        if (LevelManager.I.IsFirstLevel())
            UIManager.I.ShowTapTutorial(true);
    }
}
