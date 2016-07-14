using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SoundLoader))]
public class SoundLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SoundLoader soundLoader = (SoundLoader)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Load All Sounds into Sound Instances"))
        {
            soundLoader.CreateSoundInstances();
        }
    }
}
