using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        this.DrawDefaultInspector();
        LevelManager levelManager = (LevelManager)target;

        if (GUILayout.Button("Create New Level"))
        {
            levelManager.CreateNewLevel();
        }

        if (GUILayout.Button("Save Current Level"))
        {
            levelManager.SaveCurrentLevel();
        }

        if (GUILayout.Button("Load Level"))
        {
            levelManager.InstantlyLoadLevel(levelManager.debugLoadLevel);
        }
    }
}
