using UnityEngine;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    public List<Level> LevelOrder { get { return levelOrder; } set { levelOrder = value; } }

    [SerializeField]
    List<Level> levelOrder;

    void Start()
    {
        SolutionManager.Instance.LevelSolved += NextLevel;
    }

    void NextLevel()
    {
        for (int i = 0; i < levelOrder.Count; i++)
        {
            if (LevelManager.Instance.CurrentLevel == levelOrder[i])
            {
                if (i + 1 < levelOrder.Count)
                {
                    LevelManager.Instance.StartCoroutine(LevelManager.Instance.LoadLevel(levelOrder[i + 1]));
                }
                else
                {
                    Debug.Log("No more levels left");
                }
                return;
            }
        }
    }
}
