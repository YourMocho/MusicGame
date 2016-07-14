using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public abstract class MouseDownItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public event System.Action<Vector2> Selected;
    public event System.Action<Vector2> Deselected;
    public event System.Action<Vector2> Dragged;
    public event System.Action Reseting;

    public bool isSelected { get; private set; }

    public bool isSelectable = true;

    public void OnPointerDown(PointerEventData eventData)
    {
        Select(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Deselect(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Drag(eventData.position);
    }

    public virtual void Select(Vector2 position)
    {
        if (isSelectable)
        {
            if (Selected != null)
            {
                Selected(position);
            }
            isSelected = true;
        }
    }

    public virtual void Deselect(Vector2 position)
    {
        if (isSelected)
        {
            if (Deselected != null)
            {
                Deselected(position);
            }
            isSelected = false;
        }
    }

    public virtual void Drag(Vector2 position)
    {
        if (isSelected)
        {
            if (Dragged != null)
            {
                Dragged(position);
            }
        }
    }

    public virtual void Reset()
    {
        isSelectable = true;
        Show(false);

        if (Reseting != null)
        {
            Reseting();
        }
    }

    public abstract void Show(bool isVisible);
}
