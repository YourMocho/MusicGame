using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class TriggerRecord
{
    public float relativeTime;
    public AudioClip audio;
}

public class SolutionManager : Singleton<SolutionManager>
{
    public System.Action LevelSolved;

    List<TriggerRecord> triggerRecords = new List<TriggerRecord>();

    float triggerTimer = 0;
    float firstTriggerTime = 0;

    public LevelSolution debugSolution;
    
    void Start()
    {
        SetupTriggerListeners();

        LevelManager.Instance.LevelLoaded += SetupTriggerListeners;
        ScreenTrackingManager.Instance.ObjectHasLeftScreen += CheckIfLevelIsSolved;

        LevelSolved += GameSettings.Instance.NextLevel;
    }

    void SetupTriggerListeners()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            AudioBlock audioBlock = child.GetComponent<AudioBlock>();

            if (audioBlock != null)
            {
                audioBlock.triggerable.Use += RecordTriggering;
            }
        }
    }

	void RecordTriggering(AudioClip audioClip)
    {
        float time = Time.time;

        //first record
        if (triggerRecords.Count == 0)
        {
            firstTriggerTime = time;
        }

        triggerTimer = time - firstTriggerTime;

        //Debug.Log(audioClip.GetInstanceID() + " : " + triggerTimer);

        TriggerRecord record = new TriggerRecord()
        {
            relativeTime = triggerTimer,
            audio = audioClip
        };

        triggerRecords.Add(record);
	}

    public void SaveCurrentRunAsSolution()
    {
        if (triggerRecords.Count <= 1)
        {
            Debug.LogError("Record list was too small");
            return;
        }

        LevelSolution solution = ScriptableObject.CreateInstance<LevelSolution>();
        solution.Duration = triggerRecords[triggerRecords.Count - 1].relativeTime;
        solution.TriggerRecords = NormalizeRecordList(triggerRecords);
        LevelManager.Instance.CurrentLevel.Solution = solution;
#if UNITY_EDITOR
        AssetDatabase.CreateAsset(solution, "Assets/Resources/Level Solutions/" + LevelManager.Instance.CurrentLevel.name + "_Solution.asset");
        AssetDatabase.SaveAssets();
#endif
    }

    List<TriggerRecord> NormalizeRecordList(List<TriggerRecord> recordList)
    {
        if (recordList.Count <= 1)
        {
            Debug.LogError("Record list was too small");
            return recordList;
        }

        List<TriggerRecord> normalizedRecordList = new List<TriggerRecord>(recordList); //TODO make deep copy --------------------------------- 

        float length = recordList[recordList.Count - 1].relativeTime;

        float normalizationRatio = 100 / length;

        foreach (TriggerRecord record in recordList)
        {
            record.relativeTime *= normalizationRatio;
        }

        return normalizedRecordList;
    }

    public void CheckIfLevelIsSolved()
    {
        if (triggerRecords.Count > 1)
        {
            if (CompareAgainstLevelSolution())
            {
                Debug.Log("Level has been solved.");

                if (LevelSolved != null)
                {
                    LevelSolved();
                    return;
                }
            }
        }
        LevelManager.Instance.ResetCurrentLevel();
        PlaySolution(LevelManager.Instance.CurrentLevel.Solution);
    }

    bool CompareAgainstLevelSolution() //make private
    {
        if (LevelManager.Instance.CurrentLevel.Solution == null)
        {
            Debug.LogWarning("No Solution Exists for this Level.");
            return false;
        }

        List<TriggerRecord> solutionRecords = LevelManager.Instance.CurrentLevel.Solution.TriggerRecords;
        List<TriggerRecord> normalizedCurrentRecords = NormalizeRecordList(triggerRecords);

        if (solutionRecords.Count != normalizedCurrentRecords.Count)
        {
            Debug.Log("Current attempt was a different length than solution");
            return false;
        }

        for (int i = 0; i < solutionRecords.Count; i++)
        {
            TriggerRecord solutionRecord = solutionRecords[i];
            TriggerRecord currentRecord = normalizedCurrentRecords[i];

            //check for order
            if (solutionRecord.audio != currentRecord.audio)
            {
                Debug.Log("Out of order");
                return false;
            }

            //check for time
            float timeDifference = solutionRecord.relativeTime - currentRecord.relativeTime;

            if (Mathf.Abs(timeDifference) > GameSettings.Instance.SolutionLeeway)
            {
                Debug.Log("bad timing, was: " + Mathf.Abs(timeDifference) + " and max allowable is: " + GameSettings.Instance.SolutionLeeway);
                return false;
            }
        }

        return true;
    }

    public void ClearCurrentTriggerRecords()
    {
        triggerRecords.Clear();
    }

    public void PlaySolution(LevelSolution solution)
    {
        StartCoroutine(Play(solution));
    }

    public void StopPlayback()
    {
        StopAllCoroutines();
    }

    IEnumerator Play(LevelSolution solution)
    {
        if (solution == null)
        {
            yield return null;
        }

        int index = 0;
        float currentProgress = 0;

        while (index < solution.TriggerRecords.Count)
        {
            TriggerRecord record = solution.TriggerRecords[index];

            yield return new WaitForSeconds((record.relativeTime - currentProgress) / 100 * solution.Duration);

            currentProgress = record.relativeTime;

            AudioPlayer.Instance.PlayAudio(record.audio);

            index++;
        }
    }
}
