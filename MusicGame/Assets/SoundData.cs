using UnityEngine;
using System.Collections;

[System.Serializable]
public class SoundData : MonoBehaviour
{
    public Octave Octave { get { return octave; } set { octave = value; } }

    public Note Note { get { return note; } set { note = value; } }

    [SerializeField]
    Octave octave;

    [SerializeField]
    Note note;
}
