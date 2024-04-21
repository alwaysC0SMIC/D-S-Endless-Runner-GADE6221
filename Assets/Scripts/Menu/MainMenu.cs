using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class MainMenu : MonoBehaviour
{
    //VARIABLES
    //private bool mainMenu = true;
    //private bool leaderboardMenu = false;
    //private bool settingsMenu = false;
    //private bool htpMenu = false;

    private LevelLoader lvlLoader;

    //INDEXING:
    // 1 - LEADERBOARD
    // 2 - HOW TO PLAY
    // 3 - SETTINGS
    void Start()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).gameObject.SetActive(false);

        lvlLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    //MENU BUTTON FUNCTIONS
    //STARTS GAME
    public void PlayButton() {
        lvlLoader.PlayTransition();
        Invoke("changeToGame", 0.5F);
        Time.timeScale = 1;
    }

    //CLOSES GAME
    public void CloseGame() {
        Application.Quit();
    }

    private void changeToGame() {
        SceneManager.LoadSceneAsync(sceneName: "Game");
    }

    //OPENS HOW TO PLAY MENU
    public void HowToPlayButton() {
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).gameObject.SetActive(true);
        gameObject.transform.GetChild(3).gameObject.SetActive(false);
    }

    //OPENS LEADERBOARD MENU
    public void leaderboardButton() {
        
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).gameObject.SetActive(false);
    }

    //OPENS SETTINGS MENU
    public void settingsButton()
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).gameObject.SetActive(true);
    }

    //EXITS FROM WHATEVER MENU BACK TO MAIN MENU
    public void exitToMainMenu() {
        
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).gameObject.SetActive(false);
    }
}
