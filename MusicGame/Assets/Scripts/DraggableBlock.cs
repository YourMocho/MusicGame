using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(BoxCollider2D))]
public class DraggableBlock : DragAwayItem
{
    Vector3 selectionOffset;

    new BoxCollider2D collider;

    protected override void Awake()
    {
        base.Awake();

        collider = GetComponent<BoxCollider2D>();

        this.Dragged += UpdatePosition;
    }

    protected override void StartDrag(Vector2 position)
    {
        base.StartDrag(position);

        Vector3 placementPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, Camera.main.nearClipPlane));
        placementPosition.z = 0;

        selectionOffset = transform.position - placementPosition;

        UpdatePosition(position);
    }

    void UpdatePosition(Vector2 position)
    {
        Vector3 placementPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, Camera.main.nearClipPlane));
        placementPosition.z = 0;

        transform.position = placementPosition + selectionOffset;
    }

    protected override void EndDrag(Vector2 position)
    {
        base.EndDrag(position);

        UpdatePosition(position);

        isSelectable = true;
    }
    
    public override void Show(bool isVisible)
    {
        collider.enabled = isVisible;
    }
}
