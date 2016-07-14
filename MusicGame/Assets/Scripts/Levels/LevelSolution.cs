using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSolution : BaseScriptableObject
{
    public List<TriggerRecord> TriggerRecords { get { return triggerRecords; } set { triggerRecords = value; } }

    public float Duration { get { return duration; } set { duration = value; } }

    [SerializeField]
    List<TriggerRecord> triggerRecords = new List<TriggerRecord>();

    [SerializeField]
    float duration;
}
