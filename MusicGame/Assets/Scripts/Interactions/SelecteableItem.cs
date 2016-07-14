using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public delegate void SelectHandler(bool isNewSelection);
public delegate void DeselectHandler();

public abstract class SelecteableItem : MonoBehaviour, IPointerClickHandler
{
    public bool isSelected { get; set; }

    public event SelectHandler Selected;

    public event DeselectHandler Deselected;

    public void OnPointerClick(PointerEventData eventData)
    {
        Select();
    }

    public void Select()
    {
        if (Selected != null)
        {
            if (isSelected)
            {

                Selected(false);
            }
            else
            {
                Selected(true);
            }
        }
        isSelected = true;
        Debug.Log("Selected");
    }

    public void Deselect()
    {
        if (isSelected)
        {
            if (Selected != null)
            {
                Deselected();
            }
            isSelected = false;
            Debug.Log("Deselected");
        }
    }
}
