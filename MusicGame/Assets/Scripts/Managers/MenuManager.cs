using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : Singleton<MenuManager>
{
    public void LoadGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void LoadLevelEditor()
    {
        SceneManager.LoadScene("LevelEditor");
    }
}
