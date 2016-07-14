using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "ColorSettings", menuName = "Settings/Color Settings")]
public class ColorSettings : BaseSingletonObject<ColorSettings>
{
    public Color TrailColor { get { return trailColor; } }

    public Color SnappedTrailColor { get { return snappedTrailColor; } }

    public Color BlockColor { get { return blockColor; } }

    public Color TextColor { get { return textColor; } }

    public Color SelectedColor { get { return selectedColor; } }

    [SerializeField]
    Color trailColor;

    [SerializeField]
    Color snappedTrailColor;

    [SerializeField]
    Color blockColor;

    [SerializeField]
    Color textColor;

    [SerializeField, Header("Level Editor")]
    Color selectedColor;
}
