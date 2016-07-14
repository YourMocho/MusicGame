using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundInstance : BaseScriptableObject
{
    public Octave Octave { get { return octave; } }

    public Note Note { get { return note; } }

    [SerializeField]
    Note note;

    [SerializeField]
    Octave octave;

    [SerializeField]
    List<SoundMapping> soundMappings = new List<SoundMapping>();

    [System.Serializable]
    class SoundMapping
    {
        public SoundSet soundSet;

        public AudioClip audioClip;
    }

    public void Initialize(Octave octave, Note note)
    {
        this.note = note;
        this.octave = octave;
    }

    public void AddSound(AudioClip sound, SoundSet set)
    {
        soundMappings.Add(new SoundMapping()
        {
            soundSet = set,
            audioClip = sound
        });
    }

    public AudioClip GetSound()
    {
        //Debug.Log(AudioSettings.Instance);
        foreach (SoundMapping mapping in soundMappings)
        {
            if (mapping.soundSet == AudioSettings.Instance.ActiveSoundSet)
            {
                return mapping.audioClip;
            }
        }

        Debug.LogError("Couldnt find sound in sound set");
        return null;
    }
}
