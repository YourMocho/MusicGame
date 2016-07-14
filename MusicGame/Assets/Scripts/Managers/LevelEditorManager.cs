using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LevelEditorManager : Singleton<LevelEditorManager>
{
    [SerializeField]
    PlacementArea blockPlacementArea;

    [SerializeField]
    float rotationPerClick = 15;

    [SerializeField]
    float scalingPerClick = 0.2f;

    [SerializeField]
    MouseDownItem selectedItem;

    [SerializeField]
    Vector3 creationPosition;

    [SerializeField]
    DraggableBlock editorBlockPrefab;

    [SerializeField]
    AudioBlock levelBlockPrefab;

    List<MouseDownItem> blocks = new List<MouseDownItem>();

    LevelEditorGUI guiManager;

    int currentLevelIndex = 0;

    public void Start()
    {
        LoadAllBlocks();

        foreach (MouseDownItem block in blocks)
        {
            AddListenerToBlock(block);
        }

        guiManager = GetComponent<LevelEditorGUI>();
    }

    void AddListenerToBlock(MouseDownItem block)
    {
        MouseDownItem tempBlock = block;

        block.Selected += (position) =>
        {
            SelectBlock(tempBlock);
        };
    }

    void LoadAllBlocks()
    {
        MouseDownItem[] children = GetComponentsInChildren<MouseDownItem>();

        blocks.AddRange(children);
    }

    void SelectBlock(MouseDownItem block)
    {
        if (block != selectedItem)
        {
           // Debug.Log("Selected: " + block.gameObject.name);

            if (selectedItem != null)
            {
                selectedItem.GetComponent<SpriteRenderer>().color = ColorSettings.Instance.BlockColor;
            }

            selectedItem = block;

            selectedItem.GetComponent<SpriteRenderer>().color = ColorSettings.Instance.SelectedColor;

            blockPlacementArea.ItemToPlace = selectedItem;
            ((SnapPlacementArea)blockPlacementArea).DrawSnappingGrid();
        }
    }

    public void RotateSelected(int multiplier)
    {
        if (selectedItem == null)
        {
            return;
        }

        selectedItem.transform.Rotate(new Vector3(0, 0, multiplier * rotationPerClick));
    }

    public void ScaleSelectedVertically(int multiplier)
    {
        if (selectedItem == null)
        {
            return;
        }

        selectedItem.transform.localScale = new Vector3(selectedItem.transform.localScale.x, 
            selectedItem.transform.localScale.y + multiplier * scalingPerClick, 
            selectedItem.transform.localScale.z);
    }

    public void ScaleSelectedHorizontally(int multiplier)
    {
        if (selectedItem == null)
        {
            return;
        }

        selectedItem.transform.localScale = new Vector3(selectedItem.transform.localScale.x + multiplier * scalingPerClick,
            selectedItem.transform.localScale.y,
            selectedItem.transform.localScale.z);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(creationPosition, 0.2f);
    }

    public void CreateNewBlock()
    {
        DraggableBlock newBlock = Instantiate<DraggableBlock>(editorBlockPrefab);
        newBlock.transform.position = creationPosition;
        newBlock.transform.parent = transform;

        SoundData soundData = newBlock.gameObject.GetComponent<SoundData>();
        Debug.Log(soundData);
        soundData.Octave = guiManager.CurrentBlockSound.octave;
        soundData.Note = guiManager.CurrentBlockSound.note;

        blocks.Add(newBlock);

        AddListenerToBlock(newBlock);
    }

    public void DeleteSelectedBlock()
    {
        if (selectedItem == null)
        {
            return;
        }

        blocks.Remove(selectedItem);
        Destroy(selectedItem.gameObject);
    }

    public void SaveCurrentLevel()
    {
        if (blocks.Count == 0)
        {
            return;
        }

        string path = Application.persistentDataPath;

        DebugText.Instance.SetText(path);

        FileInfo file;
        
        file = new FileInfo(path + "\\level_" + currentLevelIndex + ".txt");

        while (file.Exists)
        {
            currentLevelIndex++;
            file = new FileInfo(path + "\\level_" + currentLevelIndex + ".txt");
        }

        StreamWriter w;
        w = file.CreateText();

        foreach (MouseDownItem block in blocks)
        {
            string blockString = "";
            blockString += block.transform.GetString() + ";";

            blockString += block.gameObject.GetComponent<SoundData>().Octave + ";";
            blockString += block.gameObject.GetComponent<SoundData>().Note;

            w.WriteLine(blockString);
        }

        w.Close();

        DebugText.Instance.SetText("DONE: " + file.FullName);
    }

    public void ClearEditor()
    {
        this.DestroyChildren();
        blocks.Clear();
    }

    public void LoadLevelToEdit()
    {
        ClearEditor();

        string path = Application.persistentDataPath + "\\" + guiManager.CurrentLoadLevelName;

        Debug.Log(path);

        string[] levelsInfo = File.ReadAllLines(path);

        foreach (string savedBlock in levelsInfo)
        {
            string[] data = savedBlock.Split(';');

            DraggableBlock newBlock = Instantiate<DraggableBlock>(editorBlockPrefab);
            newBlock.gameObject.transform.position = this.Vector3FromString(data[0]);
            newBlock.gameObject.transform.rotation = this.QuaternionFromString(data[1]);
            newBlock.gameObject.transform.localScale = this.Vector3FromString(data[2]);

            SoundData soundData = newBlock.GetComponent<SoundData>();
            soundData.Octave = data[3].ToEnum<Octave>();
            soundData.Note = data[4].ToEnum<Note>();

            newBlock.transform.parent = transform;

            blocks.Add(newBlock);
            AddListenerToBlock(newBlock);
        }
    }

    public void LoadLevelToPlay()
    {
        this.DestroyChildren();

        string path = Application.persistentDataPath + "\\" + guiManager.CurrentLoadLevelName;

        Debug.Log(path);

        string[] levelsInfo = File.ReadAllLines(path);

        foreach (string savedBlock in levelsInfo)
        {
            string[] data = savedBlock.Split(';');

            AudioBlock newBlock = Instantiate<AudioBlock>(levelBlockPrefab);
            newBlock.gameObject.transform.position = this.Vector3FromString(data[0]);
            newBlock.gameObject.transform.rotation = this.QuaternionFromString(data[1]);
            newBlock.gameObject.transform.localScale = this.Vector3FromString(data[2]);

            newBlock.Octave = data[3].ToEnum<Octave>();
            newBlock.Note = data[4].ToEnum<Note>();

            newBlock.transform.parent = transform;
        }
    }

    public void SaveSolution()
    {
        Debug.Log("TODO");
    }

    public void PlaySolution()
    {
        Debug.Log("TODO");
    }
}
