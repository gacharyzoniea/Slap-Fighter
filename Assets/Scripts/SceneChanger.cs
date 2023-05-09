using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public GameObject toggle;
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
        SceneManager.LoadScene("LavaLevel");
    }
    public void loadWater()
    {
        SceneManager.LoadScene("WaterLevel");
    }
}

