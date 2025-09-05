using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class MainMenu : MonoBehaviour
{
    //VARIABLES
    private LevelLoader lvlLoader;
    private Leaderboard lb;
    private GameObject camera;
    private Animator settingsBook;
    private Animator settingsMenu;
    private int activeMenu = 0;

    //CAM VARIABLES
    private Vector3 mainMenuCam = new Vector3 (300F, 100F, -10F);
    private Quaternion mainMenuCamRot = Quaternion.Euler(0F, -15F, 0F);

    private Vector3 leaderBoardCam = new Vector3 (400F, 162F, 725F);
    private Quaternion leaderBoardCamRot = Quaternion.Euler(0F, -35F, 0F);

    private Vector3 htpCam = new Vector3 (-80F, -75F, 444F);
    private Quaternion htpCamRot = Quaternion.Euler(0F, 0F, 0F);

    private Vector3 settingsCam = new Vector3 (245F, 100F, 269F);
    private Quaternion settingsCamRot = Quaternion.Euler(0F, -15F, 0F);

    private float cameraSpeed = 1000F;
    private float cameraRotateSpeed = 80F;

    //INDEXING:
    // 0 - MAIN MENU
    // 1 - LEADERBOARD
    // 2 - HOW TO PLAY
    // 3 - SETTINGS
    void Start()
    {
        lvlLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
        lb = GameObject.FindGameObjectWithTag("Leaderboard").GetComponent<Leaderboard>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        settingsBook = GameObject.FindGameObjectWithTag("SettingsBook").GetComponent<Animator>();
        settingsMenu = GameObject.FindGameObjectWithTag("SettingsMenu").GetComponent<Animator>();

        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        //gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).gameObject.SetActive(false);

        lb.clearScoreText();
    }

    private void Update()
    {
        switch (activeMenu)
        {
            case 0:
                if (camera.transform.position != mainMenuCam)
                {
                    camera.transform.position = Vector3.MoveTowards(camera.transform.position, mainMenuCam, Time.deltaTime * cameraSpeed * 2);
                }
                if (camera.transform.rotation != mainMenuCamRot)
                {
                    camera.transform.rotation = Quaternion.RotateTowards(camera.transform.rotation, mainMenuCamRot, Time.deltaTime * cameraRotateSpeed);
                }

                break;
            case 1:
                if (camera.transform.position != leaderBoardCam)
                {
                    camera.transform.position = Vector3.MoveTowards(camera.transform.position, leaderBoardCam, Time.deltaTime * cameraSpeed * 2);
                }
                if (camera.transform.rotation != leaderBoardCamRot)
                {
                    camera.transform.rotation = Quaternion.RotateTowards(camera.transform.rotation, leaderBoardCamRot, Time.deltaTime * cameraRotateSpeed);
                }

                break;
            case 2:
                if (camera.transform.position != htpCam)
                {
                    camera.transform.position = Vector3.MoveTowards(camera.transform.position, htpCam, Time.deltaTime * cameraSpeed * 2);
                }
                if (camera.transform.rotation != htpCamRot)
                {
                    camera.transform.rotation = Quaternion.RotateTowards(camera.transform.rotation, htpCamRot, Time.deltaTime * cameraRotateSpeed);
                }
                break;
            case 3:
                if (camera.transform.position != settingsCam)
                {
                    camera.transform.position = Vector3.MoveTowards(camera.transform.position, settingsCam, Time.deltaTime * cameraSpeed * 2);
                }
                if (camera.transform.rotation != settingsCamRot)
                {
                    camera.transform.rotation = Quaternion.RotateTowards(camera.transform.rotation, settingsCamRot, Time.deltaTime * cameraRotateSpeed);
                }
                break;
            default:
                Debug.Log("Case Broke");
                break;
        }
    }

    //MENU BUTTON FUNCTIONS
    //STARTS GAME
    public void PlayButton() {
        
        lvlLoader.GetComponentInChildren<AudioManager>().playUISFX();
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
        activeMenu = 2;
        lvlLoader.GetComponentInChildren<AudioManager>().playUISFX();
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).gameObject.SetActive(true);
        gameObject.transform.GetChild(3).gameObject.SetActive(false);
    }

    //OPENS LEADERBOARD MENU
    public void leaderboardButton() {
        activeMenu = 1;
        lvlLoader.GetComponentInChildren<AudioManager>().playUISFX();
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).gameObject.SetActive(false);
        lb.GetScores();
    }

    //OPENS SETTINGS MENU
    public void settingsButton()
    {
        gameObject.transform.GetChild(3).gameObject.SetActive(true);
        settingsBook.SetTrigger("SettingsBookOpen");
        settingsMenu.SetTrigger("OpenSettingsMenu");
        activeMenu = 3;
        lvlLoader.GetComponentInChildren<AudioManager>().playUISFX();
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);

        //Invoke("EnableSettings", 0.4F);
    }

    //EXITS FROM WHATEVER MENU BACK TO MAIN MENU
    public void exitToMainMenu() {
        
        if (activeMenu == 3)
        {
            settingsBook.SetTrigger("SettingsBookClose");
            settingsMenu.SetTrigger("CloseSettingsMenu");
        } 

        activeMenu = 0;
        lvlLoader.GetComponentInChildren<AudioManager>().playUISFX();
        //lb.clearScoreText();
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).gameObject.SetActive(false);
    }

    //FOR ANIMATION
    //private void EnableSettings() {
    //    gameObject.transform.GetChild(3).gameObject.SetActive(true);
    //}
}
