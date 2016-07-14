using UnityEngine;
using System.Collections;

public class AimingTrail : MonoBehaviour
{
    //public Vector3 StartPosition { get { return startPosition; } set { startPosition = value; } }

   // public Vector3 EndPosition { get { return endPosition; } set { endPosition = value; } }

    [SerializeField]
    SnappingDragAwayItem displayedItem = null;

    [SerializeField]
    Sprite circleSprite = null;

    [SerializeField]
    int numberOfCircles = 1;

    [SerializeField]
    float sizeOfCircles = 1;

    [SerializeField]
    float sizeChange = 0;

    [SerializeField, Range(0,1)]
    float minimumOpacity = 0;

    bool isDisplayed = false;

    Vector3 startPosition;

    Vector3 endPosition;

    Vector2 screenStartPosition;

    Vector2 screenEndPosition;

    void Awake()
    {
        displayedItem.Selected += StartDrag;
        displayedItem.Deselected += EndDrag;
        displayedItem.Dragged += UpdateDrag;
    }

    void StartDrag(Vector2 position)
    {
        startPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, Camera.main.nearClipPlane));
        startPosition.z = 0;

        screenStartPosition = position;
    }

    void UpdateDrag(Vector2 position)
    {
        endPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, Camera.main.nearClipPlane));

        endPosition.z = 0;

        screenEndPosition = position;

        if ((screenStartPosition - screenEndPosition).magnitude > PlayerSettings.Instance.MinDragDistance)
        {
            if (!isDisplayed)
            {
                isDisplayed = true;
                StartCoroutine(Display());
            }
        }
        else
        {
            //stop displaying unti the drag distance is big enough again
            EndDrag(position);
        }
    }

    void EndDrag(Vector2 position)
    {
        isDisplayed = false;
        StopAllCoroutines();
        this.DestroyChildren();
    }

    IEnumerator Display()
    {
        this.DestroyChildren();

        Vector2 screenMoveVector = (screenStartPosition - screenEndPosition);

        Vector3 moveVector = (startPosition - endPosition);

        if (screenMoveVector.magnitude > PlayerSettings.Instance.MaxDragDistance)
        {
            float factor = PlayerSettings.Instance.MaxDragDistance / screenMoveVector.magnitude;
            moveVector *= factor;
        }

        moveVector /= (float)numberOfCircles;

        for (int i = 1; i < numberOfCircles; i++)
        {
            Vector2 circlePosition = startPosition + moveVector * i;
            GameObject circleObject = new GameObject();
            circleObject.transform.parent = this.transform;
            circleObject.transform.position = circlePosition;
            circleObject.transform.localScale = new Vector2(sizeOfCircles - i * sizeChange, sizeOfCircles - i * sizeChange);
            SpriteRenderer circleRenderer = circleObject.AddComponent<SpriteRenderer>();
            circleRenderer.sprite = circleSprite;

            Color trailColor;

            if (displayedItem.IsSnapped)
            {
                trailColor = ColorSettings.Instance.SnappedTrailColor;
            }
            else
            {
                trailColor = ColorSettings.Instance.TrailColor;
            }

            circleRenderer.color = new Color(trailColor.r, trailColor.g, trailColor.b, 1 - ((1 - minimumOpacity) / numberOfCircles) * i);
        }

        yield return new WaitForEndOfFrame();

        StartCoroutine(Display());
    }
}
