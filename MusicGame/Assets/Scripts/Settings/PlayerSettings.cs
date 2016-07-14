using UnityEngine;
using System.Collections;

//[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/Player Settings")]
public class PlayerSettings : Singleton<PlayerSettings>
{
    public float MinDragDistance { get { return minDragDistance * Screen.width; } }

    public float MaxDragDistance { get { return maxDragDistance * Screen.width; } }

    [SerializeField, Header("Percentage of Screen Width"), Range(0,1)]
    float minDragDistance = 0.05f;

    [SerializeField, Range(0, 1)]
    float maxDragDistance = 0.2f;
}
