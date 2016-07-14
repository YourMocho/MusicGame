using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelManager : Singleton<LevelManager>
{
    public Level CurrentLevel { get { return currentLevel; } }

    public System.Action LevelLoaded;

    [SerializeField]
    Level currentLevel;

    [SerializeField]
    public Level debugLoadLevel;

    [SerializeField]
    Image levelTitleIcon;

    [SerializeField]
    Text levelTitleText;

    [SerializeField]
    float fadeOutSpeed = 0.01f;

    [SerializeField]
    PlayerPlacement playerPlacement;

    List<SpriteRenderer> levelBlockRenderers = new List<SpriteRenderer>();

    protected override void Awake()
    {
        base.Awake();

        StartCoroutine(LoadLevel(currentLevel));
    }

    void GetChildRenderers()
    {
        levelBlockRenderers.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            levelBlockRenderers.Add(transform.GetChild(i).GetComponent<SpriteRenderer>());
        }
    }

    public void InstantlyLoadLevel(Level level)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying && EditorUtility.DisplayDialog("Make sure to save the current Level: " + currentLevel.name, "Are you sure you want to continue?", "Load Level", "Cancel"))
        {
#endif
            this.DestroyChildren();

            currentLevel = level;

            foreach (Object prefab in currentLevel.LevelBlocks)
            {
                GameObject gameObject = Instantiate(prefab) as GameObject;
                gameObject.transform.parent = transform;
            }

            if (LevelLoaded != null)
            {
                LevelLoaded();
            }
#if UNITY_EDITOR
        }
#endif
    }

    public IEnumerator LoadLevel(Level level)
    {
        playerPlacement.isSelectable = false;

        //fade out current level
        GetChildRenderers();
        float alpha = 1;

        while (alpha > 0)
        {
            foreach (SpriteRenderer renderer in levelBlockRenderers)
            {
                alpha -= fadeOutSpeed;
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, alpha);
            }

            yield return new WaitForEndOfFrame();
        }

        this.DestroyChildren();

        currentLevel = level;

        //fade in level title
        levelTitleIcon.gameObject.SetActive(true);
        levelTitleText.gameObject.SetActive(true);
        levelTitleText.text = currentLevel.ID.ToString();
        alpha = 0;
        Color titleIconColor = ColorSettings.Instance.BlockColor;

        while (alpha < 1)
        {
            alpha += fadeOutSpeed;
            levelTitleIcon.color = new Color(titleIconColor.r, titleIconColor.g, titleIconColor.b, alpha);

            yield return new WaitForEndOfFrame();
        }

        //fade out level title
        while (alpha > 0)
        {
            alpha -= fadeOutSpeed;
            levelTitleIcon.color = new Color(titleIconColor.r, titleIconColor.g, titleIconColor.b, alpha);

            yield return new WaitForEndOfFrame();
        }

        levelTitleIcon.gameObject.SetActive(false);
        levelTitleText.gameObject.SetActive(false);

        //Start Playing Solution Sound
        SolutionManager.Instance.PlaySolution(currentLevel.Solution);

        //instantiate new level
        foreach (Object prefab in currentLevel.LevelBlocks)
        {
            GameObject gameObject = Instantiate(prefab) as GameObject;
            gameObject.transform.parent = transform;
        }

        //fade in new level
        GetChildRenderers();
        alpha = 0;

        foreach (SpriteRenderer renderer in levelBlockRenderers)
        {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, alpha);
        }

        while (alpha < 1)
        {
            foreach (SpriteRenderer renderer in levelBlockRenderers)
            {
                alpha += fadeOutSpeed;
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, alpha);
            }

            yield return new WaitForEndOfFrame();
        }

        if (LevelLoaded != null)
        {
            LevelLoaded();
        }

        playerPlacement.isSelectable = true;
    }

    public void SaveCurrentLevel()
    {
        if (currentLevel == null)
        {
            Debug.LogError("There is no current level loaded, create a new level first.");
        }
#if UNITY_EDITOR
        if (EditorUtility.DisplayDialog("This will overwrite level: " + currentLevel.name, "Are you sure you want to continue?", "Save", "Cancel"))
        {
            List<Object> blockPrefabs = new List<Object>();

            string folderPath = "Assets/Prefabs/" + currentLevel.name;

            if (AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.DeleteAsset(folderPath);
            }

            AssetDatabase.CreateFolder("Assets/Prefabs", currentLevel.name);

            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject gameObject = transform.GetChild(i).gameObject;
                string path = "Assets/Prefabs/" + currentLevel.name + "/" + gameObject.name + ".prefab";
                Object prefab = PrefabUtility.CreateEmptyPrefab(path);
                PrefabUtility.ReplacePrefab(gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
                Object newPrefab = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
                blockPrefabs.Add(newPrefab);
            }

            currentLevel.LevelBlocks = blockPrefabs;

            AssetDatabase.SaveAssets();
        }
#endif
    }

    public void CreateNewLevel()
    {
        if (currentLevel == null)
        {
            CreateNewLevelAsset();
        }
        else
        {
#if UNITY_EDITOR
            if (EditorUtility.DisplayDialog("This will overwrite level: " + currentLevel.name, "You will loose all unsaved changes to this level. Are you sure you want to continue?", "Create New Level", "Cancel"))
            {
                CreateNewLevelAsset();
            }
#endif
        }
    }

    void CreateNewLevelAsset()
    {
        GameSettings.Instance.LevelIndex++;
        Level level = ScriptableObject.CreateInstance<Level>();
#if UNITY_EDITOR
        AssetDatabase.CreateAsset(level, "Assets/Resources/Levels/Level_" + GameSettings.Instance.LevelIndex.ToString() + ".asset");
        AssetDatabase.SaveAssets();
#endif
        currentLevel = level;
        this.DestroyChildren();
    }

    public void ResetCurrentLevel()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            AudioBlock block = transform.GetChild(i).GetComponent<AudioBlock>();
            block.triggerable.Reset();
        }
    }
}
