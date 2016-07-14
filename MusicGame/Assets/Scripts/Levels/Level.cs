using UnityEngine;
using System.Collections.Generic;

public class Level : BaseScriptableObject
{
    public int ID { get { return GameSettings.Instance.GetLevelID(this); } }

    public LevelSolution Solution { get { return solution; } set { solution = value; } }

    public List<Object> LevelBlocks { get { return levelBlocks; } set { levelBlocks = value; } }

    [SerializeField]
    LevelSolution solution = null;

    [SerializeField]
    List<Object> levelBlocks = new List<Object>();
}
