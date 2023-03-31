using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    /******************WILL LOAD GAME, NOT YET CREATED******************/

    //public void loadGame()
    //{
    //    SceneManager.LoadScene(ConstantLabels.);
    //}

    public void exitGame()
    {
        Application.Quit();
    }

    public void loadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

