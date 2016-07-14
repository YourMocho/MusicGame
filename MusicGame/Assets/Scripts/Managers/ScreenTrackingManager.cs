using UnityEngine;
using System.Collections;

public class ScreenTrackingManager : Singleton<ScreenTrackingManager>
{
    [SerializeField]
    GameObject objectToTrack;

    public System.Action ObjectHasLeftScreen;

    public System.Action ObjectHasEnteredScreen;

    Vector2 screenPosition;

    Rect screenRect;

    bool isOnScreen = false;

    bool wasOnScreen = false;

    void Start()
    {
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
    }

    void Update()
    {
        screenPosition = Camera.main.WorldToScreenPoint(objectToTrack.transform.position);

        wasOnScreen = isOnScreen;

        if (screenRect.Contains(screenPosition))
        {
            isOnScreen = true;
        }
        else
        {
            isOnScreen = false;
        }

        if (wasOnScreen == true && isOnScreen == false)
        {
            if (ObjectHasLeftScreen != null)
            {
                ObjectHasLeftScreen();
            }
        }

        if (wasOnScreen == false && isOnScreen == true)
        {
            if (ObjectHasEnteredScreen != null)
            {
                ObjectHasEnteredScreen();
            }
        }
    }
}
