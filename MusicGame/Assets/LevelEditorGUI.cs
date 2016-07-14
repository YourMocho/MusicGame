using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.IO;

public class LevelEditorGUI : MonoBehaviour
{
    [SerializeField]
    GameObject createPanel;

    [SerializeField]
    GameObject editPanel;

    [SerializeField]
    GameObject mainPanel;

    [SerializeField]
    GameObject savePanel;

    [SerializeField]
    GameObject playPanel;

    [SerializeField]
    Button showGUIButton;

    [SerializeField]
    Dropdown octaveDropDown;

    [SerializeField]
    Dropdown noteDropDown;

    [SerializeField]
    Dropdown levelLoadDropDown;

    GameObject activePanel;

    public BlockSound CurrentBlockSound { get { return currentBlockSound; } set { currentBlockSound = value; } }

    public string CurrentLoadLevelName { get { return currentLoadLevelName; } }

    BlockSound currentBlockSound = new BlockSound()
    {
        note = Note.A,
        octave = Octave.Three
    };

    string currentLoadLevelName;

    [System.Serializable]
    public class BlockSound
    {
        public Note note;
        public Octave octave;
    }

    void Start()
    {
        showGUIButton.gameObject.SetActive(false);

        showGUIButton.onClick.AddListener(LoadFileNamesIntoDropDown);

        SetupDropDowns();

        activePanel = createPanel;
    }

    public void CreateView()
    {
        activePanel.SetActive(false);

        activePanel = createPanel;

        activePanel.SetActive(true);
    }

    public void EditView()
    {
        activePanel.SetActive(false);

        activePanel = editPanel;

        activePanel.SetActive(true);
    }

    public void SaveView()
    {
        activePanel.SetActive(false);

        activePanel = savePanel;

        activePanel.SetActive(true);
    }

    public void PlayView()
    {
        activePanel.SetActive(false);

        activePanel = playPanel;

        activePanel.SetActive(true);
    }

    public void ShowGUI(bool isVisible)
    {
        mainPanel.SetActive(isVisible);

        showGUIButton.gameObject.SetActive(!isVisible);
    }

    public void SetupDropDowns()
    {
        noteDropDown.options.Clear();
        octaveDropDown.options.Clear();

        Array notes = Enum.GetValues(typeof(Note));
        
        foreach (Note note in notes)
        {
            noteDropDown.options.Add(new Dropdown.OptionData(note.ToString()));
        }

        Array octaves = Enum.GetValues(typeof(Octave));

        foreach (Octave octave in octaves)
        {
            octaveDropDown.options.Add(new Dropdown.OptionData(octave.ToString()));
        }

        noteDropDown.onValueChanged.AddListener(SetCurrentNote);
        octaveDropDown.onValueChanged.AddListener(SetCurrentOctave);

        LoadFileNamesIntoDropDown();

        noteDropDown.RefreshShownValue();
        octaveDropDown.RefreshShownValue();
    }

    void SetCurrentNote(int i)
    {
        CurrentBlockSound.note = noteDropDown.options[i].text.ToEnum<Note>();
    }

    void SetCurrentOctave(int i)
    {
        CurrentBlockSound.octave = octaveDropDown.options[i].text.ToEnum<Octave>();
    }

    void SetCurrentLoadLevel(int i)
    {
        currentLoadLevelName = levelLoadDropDown.options[i].text;
    }

    void LoadFileNamesIntoDropDown()
    {
        string path = Application.persistentDataPath;

        DirectoryInfo info = new DirectoryInfo(path);
        FileInfo[] files = info.GetFiles();

        levelLoadDropDown.options.Clear();

        foreach (FileInfo file in files)
        {
            levelLoadDropDown.options.Add(new Dropdown.OptionData(file.Name));
        }

        levelLoadDropDown.onValueChanged.AddListener(SetCurrentLoadLevel);

        levelLoadDropDown.RefreshShownValue();
    }
}
