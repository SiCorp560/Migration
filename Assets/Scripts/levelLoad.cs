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
        SceneManager.LoadScene(FirstLevelName);
    }

    public void play2()
    {
        SceneManager.LoadScene(SecondLevelName);
    }

    public void play3()
    {
        SceneManager.LoadScene(ThirdLevelName);
    }

    public void exit()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
