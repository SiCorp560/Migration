using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class titleMenu : MonoBehaviour
{
    public GameObject Title;
    public GameObject Settings;
    public string FirstLevelName;

    public void PlayGame () 
    {
        SceneManager.LoadScene(FirstLevelName);

        // Start the Level 1 music when leaving the title menu
        AudioManager.S?.Pause("TitleMusic");
        AudioManager.S?.Play("Level 1");
    }

    public void QuitGame ()
    {
        Application.Quit();
    }

    public void SettingsLoad ()
    {
        Title.SetActive(false);
        Settings.SetActive(true);
    }

    public void SettingsReturn ()
    {
        Title.SetActive(true);
        Settings.SetActive(false);
    }
}

