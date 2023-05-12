using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public Toggle toggle;
    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    /******************WILL LOAD GAME, NOT YET CREATED******************/

    public void loadGame()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void loadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void loadJungle()
    {
        SceneManager.LoadScene("JungleLevel");
    }
    public void loadLava()
    {
        if (toggle.isOn)
        {
            SceneManager.LoadScene("LavaLevelMonster");
        }
        else
        {
            SceneManager.LoadScene("LavaLevel");
        }
    }
    public void loadWater()
    {
        SceneManager.LoadScene("WaterLevel");
    }

}

