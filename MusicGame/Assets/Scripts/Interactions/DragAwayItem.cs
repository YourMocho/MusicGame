using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public abstract class DragAwayItem : MouseDownItem
{
    protected Vector2 startPosition;
    protected Vector2 endPosition;

    protected virtual void Awake ()
    {
        Selected += StartDrag;
        Deselected += EndDrag;
	}

    protected virtual void StartDrag(Vector2 position)
    {
        startPosition = position;
        isSelectable = false;
    }

    protected virtual void EndDrag(Vector2 position)
    {
        endPosition = position;
    }
}
