using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/Game Settings")]
public class GameSettings : BaseSingletonObject<GameSettings>
{
    public float SolutionLeeway { get { return solutionLeeway; } }

    public int LevelIndex { get { return levelIndex; } set { levelIndex = value; } }

    public List<Level> LevelOrder { get { return levelOrder; } set { levelOrder = value; } }

    [SerializeField, Range(0,100)]
    float solutionLeeway = 0;

    [SerializeField, Header("Index used to save new levels")]
    int levelIndex = 0;

    [SerializeField]
    List<Level> levelOrder;

    public void NextLevel()
    {
        for (int i = 0; i < levelOrder.Count; i++)
        {
            if (LevelManager.Instance.CurrentLevel == levelOrder[i])
            {
                if (i + 1 < levelOrder.Count)
                {
                    LevelManager.Instance.StartCoroutine(LevelManager.Instance.LoadLevel(levelOrder[i + 1]));
                    return;
                }
                else
                {
                    Debug.Log("No more levels left");
                }
            }
        }
    }

    public void LoadLevels()
    {
        Level[] levels = Resources.LoadAll<Level>("Levels");

        foreach (Level level in levels)
        {
            if (!levelOrder.Contains(level))
            {
                levelOrder.Add(level);
            }
        }
    }

    public int GetLevelID(Level level)
    {
        for (int i = 0; i < levelOrder.Count; i++)
        {
            if (levelOrder[i] == level)
            {
                return i + 1;
            }
        }

        return -1;
    }
}
