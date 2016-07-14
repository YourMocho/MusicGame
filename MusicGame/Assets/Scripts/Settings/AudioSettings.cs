using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "AudioSettings", menuName = "Settings/Audio Settings")]
public class AudioSettings : BaseSingletonObject<AudioSettings>
{
    public float Volume { get { return volume; } }

    public SoundSet ActiveSoundSet { get { return soundSet; } }

    [SerializeField, Range(0, 1)]
    float volume = 1;

    [SerializeField]
    SoundSet soundSet;

}

public enum SoundSet
{
    Piano,
    Other
}
