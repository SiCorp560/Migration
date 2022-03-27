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

        // Temporary, just to show off new music
        AudioManager.S?.Pause("BackgroundMusic");
        AudioManager.S?.Play("Level 2");
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

