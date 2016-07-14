/*using UnityEngine;  
using UnityEditor;  
using UnityEditorInternal;

[CustomEditor(typeof(GameSettings))]
[CanEditMultipleObjects]
public class GameManagerEditor : Editor
{
    ReorderableList reorderableList = null;

    GameSettings settings;

    void OnEnable()
    {
        settings = (GameSettings)target;

        reorderableList = new ReorderableList(settings.LevelOrder, typeof(Level), true, true, false, false);

        reorderableList.drawHeaderCallback += DrawHeader;
        reorderableList.drawElementCallback += DrawElement;

        reorderableList.onAddCallback += AddItem;
        reorderableList.onRemoveCallback += RemoveItem;
    }

    void OnDisable()
    {
        reorderableList.drawHeaderCallback -= DrawHeader;
        reorderableList.drawElementCallback -= DrawElement;

        reorderableList.onAddCallback -= AddItem;
        reorderableList.onRemoveCallback -= RemoveItem;
    }

    void DrawHeader(Rect rect)
    {
        GUI.Label(rect, "Level Order");
    }

    private void DrawElement(Rect rect, int index, bool active, bool focused)
    {
        Level item = settings.LevelOrder[index];

        EditorGUI.BeginChangeCheck();

        EditorGUI.LabelField(new Rect(rect.x + 18, rect.y, rect.width - 18, rect.height), item.ToString());

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void AddItem(ReorderableList list)
    {
        GameSettings.Instance.LevelIndex++;
        Level level = ScriptableObject.CreateInstance<Level>();

        AssetDatabase.CreateAsset(level, "Assets/Levels/Level_" + GameSettings.Instance.LevelIndex.ToString() + ".asset");
        AssetDatabase.SaveAssets();

        settings.LevelOrder.Add(level);

        EditorUtility.SetDirty(target);
    }

    private void RemoveItem(ReorderableList list)
    {
        settings.LevelOrder.RemoveAt(list.index);

        EditorUtility.SetDirty(target);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        reorderableList.DoLayoutList();

        if (GUILayout.Button("Refresh Levels"))
        {
            settings.LoadLevels();
        }
    }
}
*/