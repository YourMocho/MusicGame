using UnityEngine;
using System.Collections;
using System;

public class PlayerPlacement : PlacementArea
{
    void Start()
    {
        Selected += PlaceItem;

        //pass mouse events on to player
        Selected += ItemToPlace.Select;
        Deselected += ItemToPlace.Deselect;
        Dragged += ItemToPlace.Drag;

        PlacedItem += SolutionManager.Instance.StopPlayback;
    }

    protected override void ResetItem()
    {
        base.ResetItem();

        SolutionManager.Instance.ClearCurrentTriggerRecords();
    }

    protected override void AssignListenersToItem()
    {
        throw new NotImplementedException();
    }

    protected override void RemoveListenersFromItem()
    {
        throw new NotImplementedException();
    }
}
