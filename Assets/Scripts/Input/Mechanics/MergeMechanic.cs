using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeMechanic : TouchBase
{
    Cell activeCell = null;
    Cell targetCell = null;

    [SerializeField] float lerpTime;

    public override void OnUpdate()
    {
        if (activeCell != null && targetCell != null)
        {
            if (activeCell.mobInside != null)
            {
                Vector3 lerped = Vector3.Lerp(activeCell.mobInside.transform.position, targetCell.transform.position, lerpTime * Time.deltaTime);
                activeCell.mobInside.transform.position = lerped.WithY(activeCell.mobInside.transform);
            }
        }
    }

    public override void OnDown()
    {
        Cell c = new Ray().GetObjectWithMouseRay<Cell>("Cell", 1 << 7);
       
        if (c != null)
        {
            if (c.mobInside != null)
            {
                activeCell = c;
                activeCell.mobInside.OnHold();
            }
        }
    }

    public override void OnDrag()
    {
        if (activeCell != null)
        {
            Cell c = new Ray().GetObjectWithMouseRay<Cell>("Cell", 1 << 7);

            if (c != null)
            {
                targetCell = c;
                //NO MOB HERE SO I CAN PLACE
                if (targetCell.mobInside == null)
                {
                    activeCell.mobInside.SetIndicatorColor(true);
                }
                //THERE IS MOB SO CHECK MERGE SITUATION
                else
                {
                    //if thats me
                    if (targetCell == activeCell)
                    {
                        activeCell.mobInside.SetIndicatorColor(true);
                    }
                    //CAN MERGE HERE
                    else if (targetCell.mobInside.LEVEL == activeCell.mobInside.LEVEL && targetCell.mobInside.MOBTYPE == activeCell.mobInside.MOBTYPE)
                    {
                        if (activeCell.mobInside.LEVEL <= 1)
                        {
                            activeCell.mobInside.SetIndicatorColor(true);
                        }
                        else
                        {
                            activeCell.mobInside.SetIndicatorColor(false);
                        }

                    }
                    //CANT PLACE HERE
                    else
                    {
                        activeCell.mobInside.SetIndicatorColor(false);
                    }
                }
            }
        }
    }

    public override void OnUp()
    {
        if (activeCell != null)
        {
            Cell c = new Ray().GetObjectWithMouseRay<Cell>("Cell", 1 << 7);

            if (c != null)
            {
                CheckMergeFor(c);
            }
            else
            {
                if (targetCell != null)
                {
                    CheckMergeFor(targetCell);
                }
                else
                {
                    activeCell.mobInside.GoBackToPlace();
                }
            }

        }


        activeCell = null;
        targetCell = null;
    }


    void CheckMergeFor(Cell c)
    {
        //NO MOB HERE SO I CAN PLACE
        if (c.mobInside == null)
        {
            activeCell.mobInside.DropMeToCell(c);
        }
        //THERE IS MOB SO CHECK MERGE SITUATION
        else
        {
            //if thats me
            if (c == activeCell)
            {
                activeCell.mobInside.GoBackToPlace();
            }
            //CAN MERGE HERE
            else if (c.mobInside.LEVEL == activeCell.mobInside.LEVEL && c.mobInside.MOBTYPE == activeCell.mobInside.MOBTYPE)
            {
                if (activeCell.mobInside.LEVEL <= 1)
                {
                    activeCell.mobInside.OnMerged();
                    c.mobInside.RemoveMeForMerge();
                    activeCell.mobInside.DropMeToCell(c);
                }
                else
                {
                    activeCell.mobInside.GoBackToPlace();
                }
            }
            else
            {
                activeCell.mobInside.GoBackToPlace();
            }
        }
    }

    public override void OnInitialized()
    {
        base.OnInitialized();
        DeckManager.I.HidePogs();
    }
}
