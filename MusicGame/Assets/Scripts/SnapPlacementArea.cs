using UnityEngine;

public class SnapPlacementArea : PlacementArea
{
    [SerializeField, Header("Number of snap points")]
    int width;

    [SerializeField]
    int height;

    [SerializeField]
    GameObject gridPointPrefab;

    Vector3 itemStartingPosition;

    protected override void Awake()
    {
        base.Awake();

        AssignListenersToItem();
    }

    protected override void AssignListenersToItem()
    {
        if (ItemToPlace != null)
        {
            itemToPlace.Selected += SaveStartingPosition;
            itemToPlace.Deselected += PlaceItem;
        }
    }

    protected override void RemoveListenersFromItem()
    {
        if (ItemToPlace != null)
        {
            itemToPlace.Selected -= SaveStartingPosition;
            itemToPlace.Deselected -= PlaceItem;
        }
    }

    void SaveStartingPosition(Vector2 position)
    {
        itemStartingPosition = itemToPlace.transform.position;

        DrawSnappingGrid();
    }

    public override void PlaceItem(Vector2 position)
    {
        Vector3 placementPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, Camera.main.nearClipPlane));
        placementPosition = itemToPlace.transform.position;
        Vector3 max = collider.bounds.max;
        Vector3 min = collider.bounds.min;

        float xRange = max.x - min.x;
        float yRange = max.y - min.y;

        float indexX = Mathf.Round((placementPosition.x - min.x - (xRange / width) / 2) / (xRange / width));
        float indexY = Mathf.Round((placementPosition.y - min.y - (yRange / height) / 2) / (yRange / height));

        //Debug.Log(indexX + " : " + indexY);

        if (indexX < 0 || indexX >= width || indexY < 0 || indexY >= height)
        {
            ReturnItem();
            return;
        }

        float snappedX = (indexX + 0.5f) * (xRange / width) + min.x;
        float snappedY = (indexY + 0.5f) * (yRange / height) + min.y;

        itemToPlace.transform.position = new Vector3(snappedX, snappedY, 0);

        ItemWasPlaced();

        this.DestroyChildren();
    }

    public void ReturnItem()
    {
        itemToPlace.transform.position = itemStartingPosition;
    }

    public void DrawSnappingGrid()
    {
        this.DestroyChildren();

        Vector3 max = collider.bounds.max;
        Vector3 min = collider.bounds.min;

        float xRange = max.x - min.x;
        float yRange = max.y - min.y;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(min.x + xRange / width * (x + 0.5f), min.y + yRange / height * (y + 0.5f), 1);

                GameObject gridPoint = Instantiate(gridPointPrefab, position, Quaternion.identity) as GameObject;
                gridPoint.transform.parent = transform;
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        collider = GetComponent<BoxCollider2D>();

        Gizmos.color = Color.yellow;

        Vector3 max = collider.bounds.max;
        Vector3 min = collider.bounds.min;

        float xRange = max.x - min.x;
        float yRange = max.y - min.y;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(min.x + xRange / width * (x + 0.5f), min.y + yRange / height * (y + 0.5f), 0);
                Gizmos.DrawSphere(position, 0.1f);
               // Handles.Label(position, position.x.ToString("F1") + " : " + position.y.ToString("F1"));
            }
        }
    }
}
