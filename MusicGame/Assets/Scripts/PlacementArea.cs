using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class PlacementArea : MouseDownItem
{
    public MouseDownItem ItemToPlace
    {
        get
        {
            return itemToPlace;
        }

        set
        {
            if (itemToPlace != value)
            {
                RemoveListenersFromItem();

                itemToPlace = value;

                AssignListenersToItem();
            }
        }
    }

    [SerializeField]
    protected MouseDownItem itemToPlace = null;

    public event System.Action PlacedItem;

    new protected BoxCollider2D collider;

    protected virtual void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    protected abstract void AssignListenersToItem();

    protected abstract void RemoveListenersFromItem();

    public virtual void PlaceItem(Vector2 position)
    {
        if (!itemToPlace.isSelectable)
        {
            ResetItem();
        }

        Vector3 placementPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, Camera.main.nearClipPlane));
        placementPosition.z = 0;
        itemToPlace.transform.position = placementPosition;
        itemToPlace.Show(true);

        ItemWasPlaced();
    }

    public void ItemWasPlaced()
    {
        if (PlacedItem != null)
        {
            PlacedItem();
        }
    }

    protected virtual void ResetItem()
    {
        itemToPlace.Reset();
    }

    public override void Show(bool isVisible)
    {
        collider.enabled = isVisible;
    }
}
