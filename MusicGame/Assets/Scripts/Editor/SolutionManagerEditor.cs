using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SolutionManager))]
public class SolutionManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SolutionManager solutionManager = (SolutionManager)target;

        if (GUILayout.Button("Add As Solution"))
        {
            solutionManager.SaveCurrentRunAsSolution();
        }

        if (GUILayout.Button("Compare against Debug Solution"))
        {
            solutionManager.CheckIfLevelIsSolved();
            solutionManager.ClearCurrentTriggerRecords();
        }

        if (GUILayout.Button("Play Debug Solution"))
        {

            solutionManager.PlaySolution(solutionManager.debugSolution);
        }
    }
}
