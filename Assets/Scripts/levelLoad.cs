using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelLoad : MonoBehaviour
{
    public string FirstLevelName;
    public string SecondLevelName;
    public string ThirdLevelName;
    
    public void play1()
    {
        AudioManager.S?.Pause("TitleMusic");
        AudioManager.S?.Pause("Level 2");
        AudioManager.S?.Pause("Level 3");
        AudioManager.S?.Play("Level 1");

        SceneManager.LoadScene(FirstLevelName);
    }

    public void play2()
    {
        AudioManager.S?.Pause("TitleMusic");
        AudioManager.S?.Pause("Level 1");
        AudioManager.S?.Pause("Level 3");
        AudioManager.S?.Play("Level 2");

        SceneManager.LoadScene(SecondLevelName);
    }

    public void play3()
    {
        AudioManager.S?.Pause("TitleMusic");
        AudioManager.S?.Pause("Level 1");
        AudioManager.S?.Pause("Level 2");
        AudioManager.S?.Play("Level 3");

        SceneManager.LoadScene(ThirdLevelName);
    }

    public void exit()
    {
        AudioManager.S?.Pause("Level 1");
        AudioManager.S?.Pause("Level 2");
        AudioManager.S?.Pause("Level 3");
        AudioManager.S?.Play("TitleMusic");

        SceneManager.LoadScene("TitleScreen");
    }
}
