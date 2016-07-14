using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public abstract class SnappingDragAwayItem : DragAwayItem
{
    public bool IsSnapped { get { return isSnapped; } }

    public System.Action Snapped;

    public System.Action Unsnapped;

    [SerializeField]
    int numberOfSnapPoints = 4;

    [SerializeField, Header("In degrees")]
    float snapRange = 10;

    float degreesBetweenSnapPoints;

    float[] snapPoints;

    bool isSnapped = false;

    void Start()
    {
        degreesBetweenSnapPoints = 360 / numberOfSnapPoints;
        snapPoints = new float[numberOfSnapPoints];

        for (int i = 0; i < numberOfSnapPoints; i++)
        {
            snapPoints[i] = i * degreesBetweenSnapPoints;
        }
    }

    Vector2 ApplySnap(Vector2 position)
    {
        Vector2 vector = position - startPosition;

        float angle = Mathf.Atan2(vector.y, vector.x);
        angle *= Mathf.Rad2Deg;
        angle = this.ConvertToPositiveAngle(angle);

        isSnapped = false;

        foreach (float snapPoint in snapPoints)
        {
            if (this.GetDifferenceBetweenAngles(snapPoint, angle) < snapRange)
            {
                angle = snapPoint;

                isSnapped = true;

                if (Snapped != null)
                {
                    Snapped();
                }

                break;
            }
        }

        angle *= Mathf.Deg2Rad;

        float x = Mathf.Cos(angle) * vector.magnitude + startPosition.x;
        float y = Mathf.Sin(angle) * vector.magnitude + startPosition.y;

        return new Vector2(x, y);
    }

    public override void Drag(Vector2 position)
    {
        endPosition = ApplySnap(position);

        base.Drag(endPosition);
    }

    public override void Deselect(Vector2 position)
    {
        endPosition = ApplySnap(position);

        base.Deselect(endPosition);
    }
}
