using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "SoundLoader", menuName = "Tools/Sound Loader")]
public class SoundLoader : BaseSingletonObject<SoundLoader>
{
    [SerializeField]
    string soundFilesPath = "Sounds/Piano 1";

    [SerializeField]
    string soundInstancesPath = "Assets/Resources/Sound Instances/";

    [SerializeField]
    SoundSet currentSoundSet;

    List<SoundInstance> allNotes = new List<SoundInstance>();

    public void CreateSoundInstances()
    {
        allNotes.Clear();

        AudioClip[] allSounds = Resources.LoadAll<AudioClip>(soundFilesPath);

        Debug.Log(allSounds.Length);

        foreach (AudioClip audioClip in allSounds)
        {
            string fileName = audioClip.name;

            string octaveString = fileName.Substring(fileName.Length - 1, 1);
            string noteString = fileName.Substring(0, fileName.Length - 1);

            Octave octave = this.NumberToWords(int.Parse((octaveString))).ToEnum<Octave>();
            Note note = noteString.ToEnum<Note>();

            SoundInstance instance = GetSoundInstance(octave, note, true);
            instance.AddSound(audioClip, currentSoundSet);

            Debug.Log(octave + " : " + note);
        }

        foreach (SoundInstance notes in allNotes)
        {

        }
    }

    SoundInstance CreateInstance(Octave octave, Note note)
    {
        SoundInstance soundInstance = ScriptableObject.CreateInstance<SoundInstance>();
        soundInstance.Initialize(octave, note);

#if UNITY_EDITOR
        AssetDatabase.CreateAsset(soundInstance, soundInstancesPath + note.ToString() + octave.ToString() + ".asset");
        AssetDatabase.SaveAssets();
#endif

        allNotes.Add(soundInstance);

        return soundInstance;
    }

    SoundInstance GetSoundInstance(Octave octave, Note note, bool createIfNeeded)
    {
        foreach (SoundInstance instance in allNotes)
        {
            if (instance.Note == note && instance.Octave == octave)
            {
                return instance;
            }
        }

        if (createIfNeeded)
        {
            return CreateInstance(octave, note);
        }
        else
        {
            Debug.LogError("Couldnt find sound instance: " + note + octave);
            return null;
        }
    }

    public AudioClip GetSound(Octave octave, Note note)
    {
        foreach (SoundInstance instance in allNotes)
        {
            if (instance.Note == note && instance.Octave == octave)
            {
                return instance.GetSound();
            }
        }

        Debug.LogError("Sound instance does not exist");
        return null;
    }

}

public enum Octave
{
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight
}

public enum Note
{
    A,
    B,
    C,
    D,
    E,
    F,
    G
}